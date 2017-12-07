using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(JITimer))]
public class JITimerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        JITimer targetScript = (JITimer)target;

        //  targetScript.Pause = EditorGUILayout.Toggle("Pause", targetScript.Pause);

        targetScript.TimeScale = EditorGUILayout.FloatField("Time Scale", targetScript.TimeScale);

        EditorGUILayout.FloatField("Delt Time", targetScript.DeltTime);

        EditorGUILayout.FloatField("Real Delt Time", targetScript.RealDeltTime);
    }
}
