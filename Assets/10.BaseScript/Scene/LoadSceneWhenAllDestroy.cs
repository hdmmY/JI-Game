using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

// Load specific scenes when all monitored gameobjects dead
public class LoadSceneWhenAllDestroy : MonoBehaviour
{
    public List<GameObject> m_monitoredGameobjects;

    public string m_loadSceneName;

    // Effect that control the brigntness of the screen
    public PostProcessingBehaviour m_brightnessEffect;

    private bool _loading = false;

    private void Update ()
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

        if (allDead && !_loading)
        {
            _loading = true;
            StartCoroutine (LoadScene ());
        }
    }

    /// <summary>
    /// Fade brightness to zero and then load the scene
    /// </summary>
    private IEnumerator LoadScene ()
    {
        var nextScene = SceneManagerExtension.GetBuildIndexByName (m_loadSceneName);

        if (nextScene < 0)
        {
            Debug.LogError ("Invalid Scene Name!");
            yield break;
        }

        PostProcessingProfile effect = Instantiate<PostProcessingProfile> (
            m_brightnessEffect.profile);
        ColorGradingModel.Settings effectSetting = effect.colorGrading.settings;

        m_brightnessEffect.profile = effect;

        // Fade brightness
        float timer = 0;
        while (timer < 2)
        {
            JITimer.Instance.TimeScale = 0;
            timer += JITimer.Instance.RealDeltTime;
            effectSetting.basic.postExposure = -5 * timer;
            effect.colorGrading.settings = effectSetting;
            yield return null;
        }

        EventManager.Instance.Raise (new BeforeChangeToNextStageEvent (
            SceneManager.GetActiveScene ().buildIndex, nextScene));

        SceneManager.LoadSceneAsync (m_loadSceneName, LoadSceneMode.Single);
    }
}