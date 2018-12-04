using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Clouds.Load.UI
{
    public class LoadUI : MonoBehaviour
    {
        [SerializeField]
        Canvas m_Canvas = null;
        [SerializeField]
        Animator m_Animator = null;
        [SerializeField]
        Image m_LoadBarImg = null;

        Action Event_OnFadeInDone = delegate { };

        Coroutine mProgressCo = null;

        const float ANIM_DURATION = 0.5f;

        void Awake()
        {
            m_Canvas.enabled = false;
            m_Animator.enabled = false;
            m_LoadBarImg.fillAmount = 0f;
        }

        public void Show(Action _onDone = null)
        {
            m_Animator.enabled = true;

            Event_OnFadeInDone += _onDone;
            m_Animator.SetTrigger("FadeIn");
        }

        public void Hide()
        {
            m_Animator.SetTrigger("FadeOut");
        }

        public void SetProgress(float _value)
        {
            if (mProgressCo != null)
                StopCoroutine(mProgressCo);

            mProgressCo = StartCoroutine(Progress_co(_value));
        }

        void OnFadeInDone()
        {
            Event_OnFadeInDone?.Invoke();
            Event_OnFadeInDone = delegate { };
        }

        void OnFadeOutDone()
        {
            m_Animator.enabled = false;
        }

        IEnumerator Progress_co(float _value)
        {
            var lerp = 0f;
            var currentProgress = m_LoadBarImg.fillAmount;

            while (currentProgress < _value)
            {
                lerp += Time.deltaTime / ANIM_DURATION;
                currentProgress = Mathf.Lerp(currentProgress, _value, lerp);
                m_LoadBarImg.fillAmount = currentProgress;

                yield return null;
            }

            m_LoadBarImg.fillAmount = _value;

            mProgressCo = null;
        }
    }
}