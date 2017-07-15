using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshSaverEditor : EditorWindow
{
    [MenuItem("CONTEXT/MeshFilter/Save Mesh")]
    static void SaveMeshInPace(MenuCommand command)
    {
        MeshFilter meshFilter = command.context as MeshFilter;

        if (meshFilter == null)
        {
            Debug.Log("mesh filter is null!");
            return;
        }

        Mesh mesh = meshFilter.mesh;

        SaveMesh(mesh, mesh.name, false, true);
    }

    [MenuItem("CONTEXT/MeshFilter/Save as new instance")]
    static void SaveMeshInstance(MenuCommand command)
    {
        MeshFilter meshFilter = command.context as MeshFilter;

        if (meshFilter == null)
        {
            Debug.Log("mesh filter is null!");
            return;
        }

        Mesh mesh = meshFilter.mesh;

        SaveMesh(mesh, mesh.name, true, true);
    }

    static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Mesh", "./Assets", name, "asset");

        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = makeNewInstance ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh) MeshUtility.Optimize(mesh);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
