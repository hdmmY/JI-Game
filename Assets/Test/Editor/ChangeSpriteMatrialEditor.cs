using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ChangeSpriteMatrial))]
public class ChangeSpriteMatrialEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var _target = (ChangeSpriteMatrial)target;

        if(GUILayout.Button("Change"))
        {
            _target.ChangeMaterial();
            Debug.Log("Change!");
        }
    }

}
