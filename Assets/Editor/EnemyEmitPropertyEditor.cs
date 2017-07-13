using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy_Emit_Property))]
public class EnemyEmitPropertyEditor : Editor
{
    private Enemy_Emit_Property _targetScript;

    private bool useBound;

    public override void OnInspectorGUI()
    {
        _targetScript = (Enemy_Emit_Property)target;

        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Use Bound");
        _targetScript.m_useBound = EditorGUILayout.Toggle(_targetScript.m_useBound);
        EditorGUILayout.EndHorizontal();

        if(_targetScript.m_useBound)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bounded Transform");
            _targetScript.m_FollowedTransform = EditorGUILayout.ObjectField(_targetScript.m_FollowedTransform, typeof(Transform)) as Transform;
            EditorGUILayout.EndHorizontal();
        }
              
        _targetScript.m_LocalOffset = EditorGUILayout.Vector3Field("LocalOffset", _targetScript.m_LocalOffset);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Emit Radius");
        _targetScript.m_EmitRadius = EditorGUILayout.Slider(_targetScript.m_EmitRadius, 0f, 10f);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Emit Line Number");
        _targetScript.m_EmitLineNumber = EditorGUILayout.IntSlider(_targetScript.m_EmitLineNumber, 1, 20);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Emit Point Angle Offset");
        _targetScript.m_EmitPointAngleOffset = EditorGUILayout.IntField(_targetScript.m_EmitPointAngleOffset);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Emit Interval");
        _targetScript.m_EmitInterval = EditorGUILayout.Slider(_targetScript.m_EmitInterval, 0.03f, 5f);
        EditorGUILayout.EndHorizontal();

                                           
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Emit Line Angle Offset");
        _targetScript.m_EmitDirAngleOffset = EditorGUILayout.IntField(_targetScript.m_EmitDirAngleOffset);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Emit Angle Range");
        _targetScript.m_EmitAngleRange = EditorGUILayout.IntSlider(_targetScript.m_EmitAngleRange, 0, 360);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bullet Pool");
        _targetScript.m_bulletPool = EditorGUILayout.ObjectField(_targetScript.m_bulletPool, typeof(BulletPool)) as BulletPool;
        EditorGUILayout.EndHorizontal();

        Undo.RecordObject(_targetScript, "Change Emit Property");

    }


    

}
