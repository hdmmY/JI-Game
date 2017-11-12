using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWhenDead : MonoBehaviour {

    public string m_sceneName;

    // Load specific scene when this gameobject dead.
    private void OnDestroy()
    {
        SceneManager.LoadScene(m_sceneName, LoadSceneMode.Single);     
    }
}
