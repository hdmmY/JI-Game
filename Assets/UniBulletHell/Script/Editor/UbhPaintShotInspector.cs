using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UbhPaintShot), true)]
public class UbhPaintShotInspector : Editor
{
    public override void OnInspectorGUI ()
    {
        serializedObject.Update();
        DrawProperties();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawProperties ()
    {
        UbhPaintShot obj = target as UbhPaintShot;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Shot")) {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy) {
                obj.Shot();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (obj._BulletPrefab == null || obj._PaintDataText == null) {
            Color guiColor = GUI.color;
            GUI.color = Color.yellow;

            EditorGUILayout.LabelField("*****WARNING*****");

            if (obj._BulletPrefab == null) {
                EditorGUILayout.LabelField("BulletPrefab has not been set!");
            }

            if (obj._PaintDataText == null) {
                EditorGUILayout.LabelField("PaintDataText has not been set!");
            }

            GUI.color = guiColor;
        }

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }
}