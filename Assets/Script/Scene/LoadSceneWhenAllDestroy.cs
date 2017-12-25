using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Load specific scenes when all monitored gameobjects dead
public class LoadSceneWhenAllDestroy : MonoBehaviour
{
    public List<GameObject> m_monitoredGameobjects;

    public string m_loadSceneName;

    // Effect that control the brigntness of the screen
    public BrightnessSaturationAndContrast m_brightnessEffect;

    private void Update()
    {
        bool allDead = true;

        for (int i = 0; i < m_monitoredGameobjects.Count; i++)
        {
            if (m_monitoredGameobjects[i] != null)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            StartCoroutine(LoadScene());
        }
    }

    /// <summary>
    /// Fade brightness to zero and then load the scene
    /// </summary>
    private IEnumerator LoadScene()
    {   
        float timer = 0;
        m_brightnessEffect.enabled = true;

        // Fade brightness from one to zero in one second
        while (timer < 1)
        {
            JITimer.Instance.TimeScale = 0;
            timer += JITimer.Instance.RealDeltTime;
            m_brightnessEffect.m_brightness = 1 - timer;
            yield return null;
        }
        m_brightnessEffect.m_brightness = 0;

        SceneUtil.LoadSceneAsync(m_loadSceneName);
    }             

}
