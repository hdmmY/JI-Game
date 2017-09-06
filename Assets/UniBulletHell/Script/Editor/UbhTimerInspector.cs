using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UbhTimer))]
public class UbhTimerInspector : Editor
{
    float _OrgTimeScale;

    public override void OnInspectorGUI ()
    {
        serializedObject.Update();
        DrawProperties();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawProperties ()
    {
        UbhTimer obj = target as UbhTimer;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Pause UniBulletHell")) {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy) {
                UbhTimer.Instance.Pause();
            }
        }
        if (GUILayout.Button("Resume UniBulletHell")) {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy) {
                UbhTimer.Instance.Resume();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Pause TimeScale")) {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy) {
                _OrgTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
        }
        if (GUILayout.Button("Resume TimeScale")) {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy && Time.timeScale == 0f) {
                Time.timeScale = _OrgTimeScale;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }
}
