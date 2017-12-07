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

        foreach(var pathInfo in targetScript.m_Paths)
        {
            if (pathInfo.m_PathData == null) continue;

            var controPoints = pathInfo.m_PathData.m_controlPoints;
            var name = pathInfo.m_PathData.m_pathName;

            if (controPoints.Count > 0)
            {
                //path begin and end labels:
                Handles.Label(controPoints[0], "'" + name + "' Begin", style);
                Handles.Label(controPoints[controPoints.Count - 1], "'" + name + "' End", style);
            }
        }

    }
}
