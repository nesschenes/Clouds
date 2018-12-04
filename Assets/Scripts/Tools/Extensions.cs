using Clouds.Enums;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Clouds.Extensions
{
    public static class JsonTools
    {
        public static string Serialize(this object _input)
        {
            return JsonUtility.ToJson(_input);
        }

        public static T Deserialize<T>(this string _input) where T : struct
        {
            return JsonUtility.FromJson<T>(_input);
        }
    }

    public static class SceneTools
    {
        public static void LoadSceneAsync(Scenes _scene, LoadSceneMode _mode = LoadSceneMode.Additive, Action<float> _onProgress = null, Action _onDone = null)
        {
            UndeadMono.Instance.StartCoroutine(LoadSceneAsync_co(_scene, _mode, _onProgress, _onDone));
        }

        public static void LoadScenesAsync(Scenes[] _scene, LoadSceneMode _mode = LoadSceneMode.Additive, Action<float> _onProgress = null, Action _onDone = null)
        {
            UndeadMono.Instance.StartCoroutine(LoadScenesAsync_co(_scene, _mode, _onProgress, _onDone));
        }

        public static void UnLoadSceneAsync(Scenes _scene, Action<float> _onProgress = null, Action _onDone = null)
        {
            UndeadMono.Instance.StartCoroutine(UnLoadSceneAsync_co(_scene, _onProgress, _onDone));
        }

        public static void UnLoadScenesAsync(Scenes[] _scene, Action<float> _onProgress = null, Action _onDone = null)
        {
            UndeadMono.Instance.StartCoroutine(UnLoadScenesAsync_co(_scene, _onProgress, _onDone));
        }

        static IEnumerator LoadSceneAsync_co(Scenes _scene, LoadSceneMode _mode, Action<float> _onProgress, Action _onDone)
        {
            Debug.LogFormat("Start --- LoadSceneAsync : {0}", _scene.ToString());

            var asyncLoad = SceneManager.LoadSceneAsync((int)_scene, _mode);

            while (!asyncLoad.isDone)
            {
                // Load progress is 0 to 0.9, activate progress is 0.9 to 1
                _onProgress?.Invoke(asyncLoad.progress / 0.9f);
                yield return null;
            }

            _onDone?.Invoke();

            CheckActiveScene();

            Debug.LogFormat("Complete --- LoadSceneAsync : {0}", _scene.ToString());
        }

        static IEnumerator LoadScenesAsync_co(Scenes[] _scene, LoadSceneMode _mode, Action<float> _onProgress, Action _onDone)
        {
            Debug.LogFormat("Start --- LoadScenesAsync : {0}", _scene.ToString());

            for (int i = 0; i < _scene.Length; i++)
            {
                Action<float> onSingleSceneProgress = (progress) =>
                {
                    _onProgress?.Invoke((i + progress) / _scene.Length);
                };

                yield return LoadSceneAsync_co(_scene[i], _mode, onSingleSceneProgress, null);
            }

            _onDone?.Invoke();

            Debug.LogFormat("Complete --- LoadScenesAsync : {0}", _scene.ToString());
        }

        static IEnumerator UnLoadSceneAsync_co(Scenes _scene, Action<float> _onProgress, Action _onDone)
        {
            Debug.LogFormat("Start --- UnLoadSceneAsync : {0}", _scene.ToString());

            var asyncLoad = SceneManager.UnloadSceneAsync((int)_scene);

            while (!asyncLoad.isDone)
            {
                _onProgress?.Invoke(asyncLoad.progress / 0.9f);
                yield return null;
            }

            _onDone?.Invoke();

            CheckActiveScene();

            Debug.LogFormat("Complete --- UnLoadSceneAsync : {0}", _scene.ToString());
        }

        static IEnumerator UnLoadScenesAsync_co(Scenes[] _scene, Action<float> _onProgress, Action _onDone)
        {
            Debug.LogFormat("Start --- UnLoadSceneAsync : {0}", _scene.ToString());

            for (int i = 0; i < _scene.Length; i++)
            {
                Action<float> onSingleSceneProgress = (progress) =>
                {
                    _onProgress?.Invoke(i + (progress / 0.9f) / _scene.Length);
                };

                yield return UnLoadSceneAsync_co(_scene[i], onSingleSceneProgress, null);
            }

            _onDone?.Invoke();

            Debug.LogFormat("Complete --- UnLoadScenesAsync : {0}", _scene.ToString());
        }

        static void CheckActiveScene()
        {
            var buildIndex = SceneManager.GetSceneAt(0).buildIndex;

            for (int i = 1; i < SceneManager.sceneCount; i++)
                buildIndex = SceneManager.GetSceneAt(i).buildIndex < buildIndex ? SceneManager.GetSceneAt(i).buildIndex : buildIndex;

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
        }
    }
}