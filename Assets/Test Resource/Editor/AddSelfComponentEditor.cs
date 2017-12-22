using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddSelfComponent))]
public class AddSelfComponentEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var targetScript = (AddSelfComponent)target;

        if(GUILayout.Button("Add Event Master"))
        {
            targetScript.AddEventMaster();
        }
    }

}
