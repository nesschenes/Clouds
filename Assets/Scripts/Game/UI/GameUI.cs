using Clouds.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Clouds.Game.UI
{
    public class GameUI : MonoSingleton<GameUI>
    {
        [SerializeField]
        Text m_TemperatureTxt = null;

        protected override void Awake()
        {
            base.Awake();

            GameEvents.OnCloudsRefresh += OnCloudsRefresh;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            GameEvents.OnCloudsRefresh -= OnCloudsRefresh;
        }

        void Start()
        {
            SetTemperature(GameMain.Instance.Temperature);
        }

        public void SetTemperature(float _value)
        {
            m_TemperatureTxt.text = string.Format("{0}℃", _value);
        }

        void OnCloudsRefresh()
        {
            SetTemperature(GameMain.Instance.Temperature);
        }
    }
}