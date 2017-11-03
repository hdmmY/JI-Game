using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UbhShotCtrl))]
public class UbhShotCtrlEditor : Editor {

    public override void OnInspectorGUI()
    {
        UbhShotCtrl script = target as UbhShotCtrl;

        if(script.m_FirstNotInLoop)
        {
            script.m_InitFristEleDelay = EditorGUILayout.FloatField("First Element Delay", script.m_InitFristEleDelay);
        }

        base.OnInspectorGUI();
    }
}
