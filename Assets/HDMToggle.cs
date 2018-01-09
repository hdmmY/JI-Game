using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HDMToggle : MonoBehaviour
{
    public TimeManager m_timeManager;

    public string m_stageName;

    [Button("OnClick", ButtonSizes.Medium)]
    public void OnClick()
    {
        string projectPath = "F:/JI-Game/Assets/Game/MoveDatas";

        System.IO.Directory.CreateDirectory(projectPath + "/" + m_stageName);
        foreach(var timeGameObject in m_timeManager.m_timeGos)
        {
            foreach(var pathControl in timeGameObject.Go.GetComponentsInChildren<JiPathMoveCtrl>(true))
            {
                int id = pathControl.gameObject.GetInstanceID();
                string name = timeGameObject.Go.name + pathControl.gameObject.name;
                string path = string.Format("{0}/{1}/{2}_{3}.json", projectPath, m_stageName, name, id);

                var writer = new System.IO.StreamWriter(path, false, System.Text.Encoding.Unicode);
                writer.Write(UnityEditor.EditorJsonUtility.ToJson(pathControl, true));
                writer.Close();
            }
        }


    }
}
