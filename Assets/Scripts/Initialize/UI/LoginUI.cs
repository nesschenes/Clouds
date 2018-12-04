using Clouds.Enums;
using Clouds.Load;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Clouds.Login
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField]
        Button m_LoginBtn = null;

        void Awake()
        {
            m_LoginBtn.onClick.AddListener(OnLogin);
        }

        void OnLogin()
        {
            m_LoginBtn.interactable = false;

            LoadMain.Instance.LoadScenes(new Scenes[2] { Scenes.Game, Scenes.GameUI }, LoadSceneMode.Additive, OnGameLoaded);
        }

        void OnGameLoaded()
        {
            LoadMain.Instance.UnLoadScene(Scenes.Initialize);
        }
    }
}