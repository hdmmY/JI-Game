using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bullet_Property))]
public class BulletPropertyEditor : Editor
{
    Bullet_Property _targetScript;

    bool _showBaseProperty = false;
    bool _showMotionProperty = false;


    public override void OnInspectorGUI()
    {
        _targetScript = (Bullet_Property)target;

        _showBaseProperty = EditorGUILayout.Foldout(_showBaseProperty, "Bullet Base Property");

        if (_showBaseProperty)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bullet Life Time");
            _targetScript.m_LifeTime = EditorGUILayout.Slider(_targetScript.m_LifeTime, 0.05f, 20f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bullet Color");
            _targetScript.m_BulletColor = EditorGUILayout.ColorField(_targetScript.m_BulletColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bullet Transparent");
            _targetScript.m_Alpha = EditorGUILayout.Slider(_targetScript.m_Alpha, 0, 1);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Align With Bullet Velocity");
            _targetScript.m_AlignWithVelocity = EditorGUILayout.Toggle(_targetScript.m_AlignWithVelocity);
            EditorGUILayout.EndHorizontal();

            if (!_targetScript.m_AlignWithVelocity)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Bullet Direction");
                _targetScript.m_SpriteDirection = EditorGUILayout.IntSlider(_targetScript.m_SpriteDirection, 0, 360);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        _showMotionProperty = EditorGUILayout.Foldout(_showMotionProperty, "Bullet Motion Property");

        if (_showMotionProperty)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bullet Speed");
            _targetScript.m_BulletSpeed = EditorGUILayout.FloatField(_targetScript.m_BulletSpeed);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bullet Acceleration");
            _targetScript.m_Accelerate = EditorGUILayout.FloatField(_targetScript.m_Accelerate);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Acceleration Direction");
            _targetScript.m_AcceleratDir = EditorGUILayout.IntSlider(_targetScript.m_AcceleratDir, 0, 365);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Velocity Horizontal Factor");
            _targetScript.m_HorizontalVelocityFactor = EditorGUILayout.FloatField(_targetScript.m_HorizontalVelocityFactor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Velocity Vertical Factor");
            _targetScript.m_VerticalVelocityFactor = EditorGUILayout.FloatField(_targetScript.m_VerticalVelocityFactor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        Undo.RecordObject(_targetScript, "Change Bullet Property");

    }










}
