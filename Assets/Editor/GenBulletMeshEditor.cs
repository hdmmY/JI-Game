using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Toggles.GenerateBulletMesh))]
public class GenBulletMeshEditor : Editor
{
    private Toggles.GenerateBulletMesh _genBulMeshScript;


    public override void OnInspectorGUI()
    {
        _genBulMeshScript = (Toggles.GenerateBulletMesh)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Bullet Mesh"))
        {
            Undo.RecordObject(_genBulMeshScript, "Generate Bullet Mesh");
            _genBulMeshScript.GenerateBullet();
        }
    }


}
