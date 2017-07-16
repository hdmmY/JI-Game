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


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Debug use: Velocity");
        EditorGUILayout.Vector2Field(GUIContent.none, _targetScript.m_Velocity);
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("use attrack");
        _targetScript.m_useBulletAttrack = EditorGUILayout.Toggle(_targetScript.m_useBulletAttrack);
        EditorGUILayout.EndHorizontal();

        if(_targetScript.m_useBulletAttrack)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("attrack factor");
            _targetScript.m_attrackFactor = EditorGUILayout.Slider(_targetScript.m_attrackFactor, 0, 100f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("target tarnsform");
            _targetScript.m_targetTrans = EditorGUILayout.ObjectField(_targetScript.m_targetTrans, typeof(Transform)) as Transform;
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("use reject");
        _targetScript.m_useBulletReject = EditorGUILayout.Toggle(_targetScript.m_useBulletReject);
        EditorGUILayout.EndHorizontal();

        if(_targetScript.m_useBulletReject)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("reject factor");
            _targetScript.m_rejectFactor = EditorGUILayout.Slider(_targetScript.m_rejectFactor, 0, 100f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("target tarnsform");
            _targetScript.m_targetTrans = EditorGUILayout.ObjectField(_targetScript.m_targetTrans, typeof(Transform)) as Transform;
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bullet Damage");
        _targetScript.m_BulletDamage = EditorGUILayout.IntField(_targetScript.m_BulletDamage);
        EditorGUILayout.EndHorizontal();


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
