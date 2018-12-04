using Clouds.Enums;
using Clouds.Extensions;
using Clouds.Load.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Clouds.Load
{
    public class LoadMain : UndeadMonoSingleton<LoadMain>
    {
        [SerializeField]
        LoadUI m_LoadUI = null;

        public void LoadScene(Scenes _scene, LoadSceneMode _mode = LoadSceneMode.Additive, Action _onDone = null)
        {
            Action onLoadBegin = () =>
            {
                Action onLoadEnd = () => { OnDone(); _onDone?.Invoke(); };

                SceneTools.LoadSceneAsync(_scene, _mode, OnProgress, onLoadEnd);
            };

            m_LoadUI.Show(onLoadBegin);
        }

        /// <summary>Load scenes one by one</summary>
        public void LoadScenes(Scenes[] _scenes, LoadSceneMode _mode = LoadSceneMode.Additive, Action _onDone = null)
        {
            Action onLoadBegin = () =>
            {
                Action onLoadEnd = () => { OnDone(); _onDone?.Invoke(); };

                SceneTools.LoadScenesAsync(_scenes, _mode, OnProgress, onLoadEnd);
            };

            m_LoadUI.Show(onLoadBegin);
        }

        public void UnLoadScene(Scenes _scene)
        {
            SceneTools.UnLoadSceneAsync(_scene);
        }

        public void UnLoadScenes(Scenes[] _scene)
        {
            SceneTools.UnLoadScenesAsync(_scene);
        }

        void OnProgress(float _value)
        {
            m_LoadUI.SetProgress(_value);
        }

        void OnDone()
        {
            m_LoadUI.Hide();
        }
    }
}