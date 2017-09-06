using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UbhShotCtrl))]
public class UbhShotCtrlInspector : Editor
{
    public override void OnInspectorGUI ()
    {
        serializedObject.Update();
        DrawProperties();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawProperties ()
    {
        UbhShotCtrl obj = target as UbhShotCtrl;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Shot Routine")) {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy) {
                obj.StartShotRoutine();
            }
        }
        if (GUILayout.Button("Stop Shot Routine")) {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy) {
                obj.StopShotRoutine();
            }
        }
        EditorGUILayout.EndHorizontal();

        Color guiColor = GUI.color;
        if (obj._ShotList == null || obj._ShotList.Count <= 0) {
            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("*****WARNING*****");
            EditorGUILayout.LabelField("Size of ShotList is 0!");
            GUI.color = guiColor;

        } else {
            bool isShotErr = true;
            foreach (UbhShotCtrl.ShotInfo shotInfo in obj._ShotList) {
                if (shotInfo._ShotObj != null) {
                    isShotErr = false;
                    break;
                }
            }
            bool isDelayErr = true;
            foreach (UbhShotCtrl.ShotInfo shotInfo in obj._ShotList) {
                if (0f < shotInfo._AfterDelay) {
                    isDelayErr = false;
                    break;
                }
            }
            if (isShotErr || isDelayErr) {
                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("*****WARNING*****");
                if (isShotErr) {
                    EditorGUILayout.LabelField("Some ShotObj of ShotList has not been set!");
                }
                if (isDelayErr) {
                    EditorGUILayout.LabelField("All AfterDelay of ShotList is zero!");
                }
                GUI.color = guiColor;
            }
        }

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }
}