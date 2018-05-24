using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneByInput : MonoBehaviour
{
    // Load the scene correspond to the keyboard input
    [System.Serializable]
    public struct SceneInput
    {
        public string m_sceneName;
        public KeyCode m_input;
    }

    public List<SceneInput> m_SceneInputs;

    private void Update ()
    {
        if (m_SceneInputs == null) return;

        foreach (var sceneInput in m_SceneInputs)
        {
            if (Input.GetKeyDown (sceneInput.m_input))
            {
                var nextScene = SceneManagerExtension.GetBuildIndexByName (sceneInput.m_sceneName);

                if (nextScene >= 0)
                {
                    EventManager.Instance.Raise (new BeforeChangeToNextStageEvent (
                        SceneManager.GetActiveScene ().buildIndex, nextScene));

                    SceneManager.LoadSceneAsync (sceneInput.m_sceneName, LoadSceneMode.Single);
                    return;
                }
                else
                {
                    Debug.LogError ("Invalid Scene Name!");
                }
            }
        }
    }

}