using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blabla : MonoBehaviour
{                 
    public GameObject m_go;

    public string m_sceneName;

    private bool _load = false;

    private void Update()
    {
        if (m_go == null && _load == false)
        {
            StartCoroutine(WaitforLoad());
            _load = true;
        }
    }

    IEnumerator WaitforLoad()
    {
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(m_sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
