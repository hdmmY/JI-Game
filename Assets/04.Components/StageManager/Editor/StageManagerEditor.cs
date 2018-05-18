using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(StageManager))]
public class StageManagerEditor : Editor
{
    StageManager targetScript;

    public override void OnInspectorGUI()
    {
        targetScript = (StageManager)target;

        // 
        if (GUILayout.Button("Set All TimeGO Deactive"))
        {
            for (int i = 0; i < targetScript.m_stages.Count; i++)
            {
                if(targetScript.m_stages[i].m_stageDataGO != null)
                    targetScript.m_stages[i].m_stageDataGO.SetActive(false);
            }
        }

        // 
        if (GUILayout.Button("Set All TimeGO Active"))
        {
            for (int i = 0; i < targetScript.m_stages.Count; i++)
            {
                if (targetScript.m_stages[i].m_stageDataGO != null)
                    targetScript.m_stages[i].m_stageDataGO.SetActive(true);
            }
        }

        base.OnInspectorGUI();
    }

}
