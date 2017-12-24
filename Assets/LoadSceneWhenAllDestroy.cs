using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneWhenAllDestroy : MonoBehaviour
{
    public List<GameObject> m_gos;

    public string m_loadSceneName;

    private void Awake()
    {
        StartCoroutine(OnSceneLoad());
    }

    private void Update()
    {
        bool allDead = true;

        for (int i = 0; i < m_gos.Count; i++)
        {
            if (m_gos[i] != null)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            StartCoroutine(LoadOtherScene());
        }
    }

    IEnumerator LoadOtherScene()
    {
        var effect = JIGlobalRef.MainCamera.GetComponent<BrightnessSaturationAndContrast>();

        float timer = 0;
        while (timer < 1)
        {
            JITimer.Instance.TimeScale = 0;
            timer += JITimer.Instance.RealDeltTime;
            effect.m_brightness = 1 - timer;
            yield return null;
        }
        effect.m_brightness = 0;

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_loadSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    IEnumerator OnSceneLoad()
    {
        JITimer.Instance.TimeScale = 0;

        var effect = JIGlobalRef.MainCamera.GetComponent<BrightnessSaturationAndContrast>();

        float timer = 0;
        while (timer < 1)
        {
            JITimer.Instance.TimeScale = 0;
            timer += JITimer.Instance.RealDeltTime;
            effect.m_brightness = timer;
            yield return null;
        }

        effect.m_brightness = 1;

        JITimer.Instance.TimeScale = 1;
    }

}
