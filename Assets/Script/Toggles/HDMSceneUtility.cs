using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HDMSceneUtility : MonoBehaviour
{                            
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
	
    public void LoadSceneWhenDistroy(string sceneName, GameObject go)
    {
        if(go == null)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }                          
    }

}
