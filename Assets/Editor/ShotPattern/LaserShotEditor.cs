//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;


//[CanEditMultipleObjects]
//[CustomEditor(typeof(LaserShot))]
//public class LaserShotEditor : Editor
//{
//    LaserShot _script;

//    public override void OnInspectorGUI()
//    {
//        _script = (LaserShot)target;

//        if(_script.m_bulletPrefab == null)
//        {
//            EditorGUILayout.LabelField("*****WARNING*****");
//            EditorGUILayout.LabelField("BulletPrefab has not been set!");
//        }

//        EditorGUILayout.Space();

//        _script.m_bulletPrefab = EditorGUILayout.ObjectField("Bullet Prefab",
//                    _script.m_bulletPrefab, typeof(GameObject), true) as GameObject;

//        _script.m_angle = EditorGUILayout.FloatField("Angle", _script.m_angle);

//        _script.m_width = EditorGUILayout.FloatField("Width", _script.m_width);

//        _script.m_startAlpha = EditorGUILayout.FloatField("Start Alpha", _script.m_startAlpha);

//        _script.m_laserDisappearSpeed = EditorGUILayout.FloatField(
//                            "Laser Disappear Speed", _script.m_laserDisappearSpeed);

//        _script.m_waitingTime = EditorGUILayout.FloatField("Waiting Time", _script.m_waitingTime);

//        _script.m_sustainTime = EditorGUILayout.FloatField("Sustain Time", _script.m_sustainTime);

//        EditorGUILayout.Space();
//        _script.m_autoReleaseTime = EditorGUILayout.FloatField("AutoReleaseTime", _script.m_waitingTime + _script.m_sustainTime);
//    }

//}
