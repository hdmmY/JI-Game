using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneWhenAllDestroy : MonoBehaviour
{
    public List<GameObject> m_gos;

    public string m_loadSceneName;

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
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_loadSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

}
