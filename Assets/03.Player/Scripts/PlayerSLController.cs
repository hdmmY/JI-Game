using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Player save and load controller
/// </summary>
[RequireComponent (typeof (PlayerProperty))]
public class PlayerSLController : MonoBehaviour
{
    private PlayerProperty _player;

    private void OnEnable ()
    {
        _player = GetComponent<PlayerProperty> ();

        EventManager.Instance.AddListener<BeforeChangeToNextStageEvent> (OnChangeToNextStage);
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable ()
    {
        EventManager.Instance.RemoveListener<BeforeChangeToNextStageEvent> (OnChangeToNextStage);
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    void OnApplicationQuit ()
    {
        File.Delete ("Saves/player.binary");
    }

    private void OnChangeToNextStage (BeforeChangeToNextStageEvent changeEvent)
    {
        if (IsStageScene (changeEvent.NextSceneBuildIndex))
        {
            SavePlayerStates ();
        }
    }

    private void OnActiveSceneChanged (Scene current, Scene next)
    {
        if (current.name == null && IsStageScene (next.buildIndex))
        {
            LoadPlayerStates ();
        }
    }

    private bool IsStageScene (int sceneBuildIndex)
    {
        if (sceneBuildIndex >= 0 && sceneBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            string sceneName = SceneUtility.GetScenePathByBuildIndex (sceneBuildIndex);
            int start = sceneName.LastIndexOf ('/') + 1;
            int end = sceneName.LastIndexOf ('.');
            sceneName = sceneName.Substring (start, end - start);

            return (sceneName == "Stage1") ||
                (sceneName == "Stage2") ||
                (sceneName == "Stage3");
        }
        else
        {
            return false;
        }
    }

    private void LoadPlayerStates ()
    {
        FileStream saveFile = null;

        try
        {
            saveFile = File.Open ("Saves/player.binary", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter ();
            PlayerStatistics loadData = (PlayerStatistics) formatter.Deserialize (saveFile);
            loadData.Load (_player);
            Debug.Log ("Load success!" + Time.time);
        }
        catch (Exception e)
        {

        }
        finally
        {
            if (saveFile != null)
                saveFile.Close ();
        }
    }

    private void SavePlayerStates ()
    {
        if (!Directory.Exists ("Saves"))
        {
            Directory.CreateDirectory ("Saves");
        }

        BinaryFormatter formatter = new BinaryFormatter ();
        FileStream saveFile = File.Create ("Saves/player.binary");
        formatter.Serialize (saveFile, new PlayerStatistics (_player));
        saveFile.Close ();

        Debug.Log ("Save sucess!" + Time.time);
    }

}