using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(JiPathMoveCtrl))]
public class JiPathMoveCtrlEditor : Editor
{
    JiPathMoveCtrl targetScript;

    // Draw the contorl point handle and path name.
    void OnSceneGUI()
    {
        targetScript = (JiPathMoveCtrl)target;

        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;

        string pathName = targetScript.gameObject.name;
        foreach (var pathInfo in targetScript.m_Paths)
        {
            if (pathInfo.m_controlPoints == null || pathInfo.m_controlPoints.Count == 0)
                continue;

            ////path begin and end labels:
            //Handles.Label(point, "'" + name + "' Begin", style);
            //Handles.Label(point, "'" + name + "' End", style);
        }
    }
}
