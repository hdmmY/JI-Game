using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blabla1 : MonoBehaviour
{                 
    public List<GameObject> m_gos;

    public string m_sceneName;

    private bool _load = false;

    private void Update()
    {
        foreach(var go in m_gos)
        {
            if (go != null) return;
        }

        if (_load == false)
        {
            StartCoroutine(WaitForLoad());
            _load = true;
        }
            
        
    }


    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(m_sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    
}
