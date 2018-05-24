using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerExtension
{
    /// <summary>
    /// Searches through the Scenes in the build settings for a Scene with the given name.
    /// </summary>
    /// <param name="name">Name of Scene to find.</param>
    /// <returns>The correspond build index of the Scene, if valid. If not, -1 is returned.</returns>
    public static int GetBuildIndexByName (string name)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (SceneManager.GetSceneByBuildIndex (i).name == name)
                return i;
        }

        return -1;
    }
}