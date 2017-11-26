using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UbhTimer))]
public class UbhTimerInspector : Editor
{
    private UbhTimer _targetScript;

    public override void OnInspectorGUI()
    {
        _targetScript = (UbhTimer)target;

        base.OnInspectorGUI();

        _targetScript.Pause = EditorGUILayout.Toggle("Pause", _targetScript.Pause);
        _targetScript.TimeScale = EditorGUILayout.FloatField("Time Scale", _targetScript.TimeScale);
        EditorGUILayout.IntField("Frame Count", _targetScript.FrameCount);                                          
    }

}
