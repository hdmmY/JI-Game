using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(TimeManager))]
public class TimeManagerEditor : Editor
{
    TimeManager targetScript;

    public override void OnInspectorGUI()
    {               
        targetScript = (TimeManager)target;

        // 
        if (GUILayout.Button("Set All TimeGO Deactive"))
        {
            for(int i = 0; i < targetScript.m_timeGos.Count; i++)
            {
                targetScript.m_timeGos[i].Go.SetActive(false);
            }
        }

        // 
        if (GUILayout.Button("Set All TimeGO Active"))
        {
            for (int i = 0; i < targetScript.m_timeGos.Count; i++)
            {
                if(targetScript.m_timeGos[i].Go != null)
                    targetScript.m_timeGos[i].Go.SetActive(true);
            }
        }

        base.OnInspectorGUI();
    }

}
