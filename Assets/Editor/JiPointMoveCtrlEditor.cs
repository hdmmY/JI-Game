using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(JiPointMoveCtrl))]
public class JiPointMoveCtrlEditor : Editor
{
    private JiPointMoveCtrl _targetScript;


    // Draw the point handle and point index
    public void OnSceneGUI()
    {
        _targetScript = (JiPointMoveCtrl)target;

        GUIStyle textStyle = new GUIStyle();
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;

        // Show point label
        Handles.Label(_targetScript.m_startPoint, "0");
        for(int i = 0; i < _targetScript.m_Paths.Count; i++)
        {
            Handles.Label(_targetScript.m_Paths[i].m_destination, (i + 1).ToString());
        }


        // Show point handle
        _targetScript.m_startPoint = Handles.PositionHandle(_targetScript.m_startPoint, Quaternion.identity);
        for(int i = 0; i < _targetScript.m_Paths.Count; i++)
        {
            _targetScript.m_Paths[i].m_destination = 
                Handles.PositionHandle(_targetScript.m_Paths[i].m_destination, Quaternion.identity);
        }
    }


    private void ShowPoint(Vector3 position, GUIStyle textStyle)
    {
              

    }

}
