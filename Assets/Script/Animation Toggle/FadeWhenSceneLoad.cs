using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimationToggle
{
    // Lock the time, and fade scene brightness from zero to one.
    public class FadeWhenSceneLoad : MonoBehaviour
    {
        public BrightnessSaturationAndContrast m_brightnessEffect;

        [Range(0.01f, 3f)]
        public float m_fadeTime = 1;

        private void Awake()
        {
            StartCoroutine(OnSceneLoad());
        }                  

        private IEnumerator OnSceneLoad()
        {
            JITimer.Instance.Pause = true;

            m_brightnessEffect.enabled = true;

            float timer = 0;
            while (timer < m_fadeTime)
            {
                JITimer.Instance.TimeScale = 0;
                timer += JITimer.Instance.RealDeltTime;
                m_brightnessEffect.m_brightness = timer;
                yield return null;
            }
            m_brightnessEffect.m_brightness = 1;

            JITimer.Instance.TimeScale = 1;
        }
    }

}


