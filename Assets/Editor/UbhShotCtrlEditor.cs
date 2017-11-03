using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UbhShotCtrl))]
public class UbhShotCtrlEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UbhShotCtrl script = target as UbhShotCtrl;

        script.m_loop = EditorGUILayout.Toggle("Loop", script.m_loop);

        script.m_FirstNotInLoop = EditorGUILayout.Toggle("First Not In Loop", script.m_FirstNotInLoop);

        if(script.m_FirstNotInLoop)
        {
            script.m_InitFristEleDelay = EditorGUILayout.FloatField("First Element Delay", script.m_InitFristEleDelay);
        }
    }
}
