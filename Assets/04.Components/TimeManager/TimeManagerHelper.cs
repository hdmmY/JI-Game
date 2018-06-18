using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class TimeManagerHelper : EditorWindow
{
    private TimeManager _timeManager;

    private Transform _parent;

    [MenuItem ("Tools/Set Time Gameobject Batch")]
    private static void Init ()
    {
        var window = EditorWindow.GetWindow (typeof (TimeManagerHelper)) as TimeManagerHelper;
        window.Show ();
    }

    private void OnGUI ()
    {
        _timeManager = EditorGUILayout.ObjectField ("Time Manager",
            _timeManager, typeof (TimeManager), true) as TimeManager;

        _parent = EditorGUILayout.ObjectField ("Parent",
            _parent, typeof (Transform), true) as Transform;

        if (GUILayout.Button ("Import"))
        {
            if (_timeManager == null || _parent == null) return;

            _timeManager.m_timeGos = new List<TimeManager.TimeGameobject> ();
            for(int i = 0; i < _parent.childCount; i++)
            {
                _timeManager.m_timeGos.Add (new TimeManager.TimeGameobject
                {
                    Go = _parent.GetChild(i).gameObject
                });
            }
        }
    }
}