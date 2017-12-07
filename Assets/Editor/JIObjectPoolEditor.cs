using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JIObjectPool))]
public class JIObjectPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        JIObjectPool targetScript = (JIObjectPool)target;

        targetScript.m_prefab = EditorGUILayout.ObjectField("Prefab", targetScript.m_prefab, typeof(GameObject), true) as GameObject;

        targetScript.m_instancesToPreallocate = EditorGUILayout.IntField("Instance To Preallocate", targetScript.m_instancesToPreallocate);

        JIObjectPool.InstanceToPreAllocateIfEmpty = EditorGUILayout.IntField("Instance To Preallocate If Empty", JIObjectPool.InstanceToPreAllocateIfEmpty);

        targetScript.m_enbleMaxLimit = EditorGUILayout.Toggle("Enable Max Limit", targetScript.m_enbleMaxLimit);
        if(targetScript.m_enbleMaxLimit)
        {
            targetScript.m_maxLimit = EditorGUILayout.IntField("Max Limit", targetScript.m_maxLimit);
        }

        targetScript.m_cullExcessPrefabs = EditorGUILayout.Toggle("Cull Excess Prefabs", targetScript.m_cullExcessPrefabs);
        if(targetScript.m_cullExcessPrefabs)
        {
            targetScript.m_instantesToMaintainInPool = EditorGUILayout.IntField("Instance To Maintain In Pool", targetScript.m_instantesToMaintainInPool);
            targetScript.m_cullInterval = EditorGUILayout.FloatField("Cull Interval", targetScript.m_cullInterval);
        }

        EditorGUILayout.IntField("Spawn Instance Count", targetScript.SpawnedInstanceCount);
        EditorGUILayout.IntField("In Pool Object Count", targetScript.InPoolObjectCount);
        
        if(GUILayout.Button("Initialize"))
        {
            Undo.RecordObject(targetScript, "Initialize the pool");
            targetScript.Initialize();
        }

        if(GUILayout.Button("Spawn"))
        {
            Undo.RecordObject(targetScript, "Spawn from the pool");
            targetScript.Spawn();
        }

        if(GUILayout.Button("Cull Excess Objects"))
        {
            Undo.RecordObject(targetScript, "Cull Excess Objects");
            targetScript.CullExcessObjects();
        }

        if(GUILayout.Button("Clear"))
        {
            Undo.RecordObject(targetScript, "Clear the pool");
            targetScript.Clear();
        }
    }

}
