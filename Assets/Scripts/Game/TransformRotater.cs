using Clouds.Extensions;
using UnityEngine;

namespace Clouds.Game
{
    public class TransformRotater : MonoBehaviour
    {
        [SerializeField]
        float m_MinSpeed = 10f;
        [SerializeField]
        float m_MaxSpeed = 200f;
        [SerializeField]
        float m_TouchSensitivity = 0.1f;

        float mSpeed = 10f;

        Vector3 mDirection = Vector3.forward;

        void Awake()
        {
            mSpeed = m_MinSpeed;
        }

        void Update()
        {
            CheckTouch();

            mSpeed = Mathf.Clamp(mSpeed, 0, m_MaxSpeed);

            gameObject.transform.Rotate(mDirection, mSpeed * Time.deltaTime);
        }

        void CheckTouch()
        {
            var touches = InputHelper.GetTouches();
            if (touches.Length <= 0) return;

            var touch = touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Stationary:
                    mSpeed = 0f; // stop rotate
                    break;
                case TouchPhase.Moved:
                    mSpeed += Mathf.Abs(touch.deltaPosition.x) * m_TouchSensitivity;
                    mDirection = touch.deltaPosition.x < 0 ? Vector3.forward : Vector3.back;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    mSpeed = m_MinSpeed;
                    break;
            }
        }
    }
}