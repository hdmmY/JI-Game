using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class JiPathMoveCtrl : JiMoveCtrlBase
{
    
    [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = true, Expanded = false)]
    public List<JIPathInfo> m_Paths;

    [ReadOnly]
    [ShowInInspector]
    private float _timer;

    [ReadOnly, ShowInInspector]
    private int _curPathIndex;
    private bool _curPathInvoked;

    public override void Start()
    {
        base.Start();

        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        _timer = -0.5f;

        _curPathIndex = 0;
        _curPathInvoked = false;

        iTween.Init(m_targetGameObject);
    }


    private void Update()
    {
        if (m_Paths == null) return;
        if (m_targetGameObject == null) return;
        if (_curPathIndex > m_Paths.Count) return;

        _timer += JITimer.Instance.DeltTime;

        if (_curPathIndex == m_Paths.Count)
        {
            if (m_distroyWhenEndOfPaths)
            {
                Destroy(m_targetGameObject);
            }
            _curPathIndex++;
            return;
        }

        float startTime, endTime;
        startTime = m_Paths[_curPathIndex].m_delayTime;
        endTime = m_Paths[_curPathIndex].m_time + startTime;
        if (m_Paths[_curPathIndex].m_loopType != iTween.LoopType.none)
            endTime += m_Paths[_curPathIndex].m_time * (m_Paths[_curPathIndex].m_loopTimes - 1);

        if ((_timer >= startTime - 0.01f) && !_curPathInvoked)
        {
            StartMove(_curPathIndex);
            _curPathInvoked = true;
            return;
        }

        if ((_timer >= endTime - 0.01f))
        {
            _timer = 0f;
            _curPathIndex++;
            _curPathInvoked = false;
            iTween.Stop(m_targetGameObject);
            return;
        }
    }


    public override void StopMove()
    {
        if (m_targetGameObject != null)
        {
            iTween.Stop(m_targetGameObject, "moveto");
        }
    }

    // Lauch the itween variabeles
    public override Hashtable LauchArgs(int index)
    {
        if (m_Paths == null)
        {
            Debug.LogError("There is no path!");
            return null;
        }

        if (m_Paths.Count <= index)
        {
            Debug.LogError("The path index is not right!");
            return null;
        }

        var pathInfo = m_Paths[index];

        Hashtable args = new Hashtable();

        args.Add("axis", "z");   // restrict the rotation to z-axis only.
        args.Add("path", pathInfo.m_controlPoints.ToArray());
        args.Add("movetopath", pathInfo.m_moveTo);
        args.Add("easetype", pathInfo.m_easeType);
        args.Add("looptype", pathInfo.m_loopType);
        args.Add("time", pathInfo.m_time);

        return args;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var pathInfo in m_Paths)
        {
            if (pathInfo.m_controlPoints == null || pathInfo.m_controlPoints.Count < 2)
                continue;
            Gizmos.DrawIcon(pathInfo.m_controlPoints[0], "Point", true);
            Gizmos.DrawIcon(pathInfo.m_controlPoints[pathInfo.m_controlPoints.Count - 1], "Point", true);
            iTween.DrawPathGizmos(pathInfo.m_controlPoints.ToArray());
        }
    }
}

[System.Serializable]
public struct JIPathInfo
{
    /// <summary>
    /// Path nodes
    /// </summary>
    [ListDrawerSettings(NumberOfItemsPerPage = 4, Expanded = false)]
    public List<Vector3> m_controlPoints;

    /// <summary>
    /// Set a delay time to start move when this path is invoked
    /// </summary>
    [CustomValueDrawer("ClampToNoneNagativeFloat")]
    public float m_delayTime;

    /// <summary>
    /// Time in seconds the movement will take to complete. 
    /// </summary>
    [CustomValueDrawer("ClampToNoneNagativeFloat")]
    public float m_time;

    /// <summary>
    /// The ease type of the movement.
    /// </summary>
    public iTween.EaseType m_easeType;

    /// <summary>
    /// The loop type of the movement.
    /// </summary>
    public iTween.LoopType m_loopType;

    [HideIf("m_loopType", iTween.LoopType.none)]
    [CustomValueDrawer("ClampToNoneNegativeInt")]
    public int m_loopTimes;

    /// <summary>
    /// The gameobject will move to the start of the path
    /// </summary>
    public bool m_moveTo;

    private static float ClampToNoneNagativeFloat(float value, GUIContent label)
    {
        value = value < 0.01f ? 0.01f : value;
        return UnityEditor.EditorGUILayout.FloatField(label, value);
    }

    private static int ClampToNoneNegativeInt(int value, GUIContent label)
    {
        value = value < 1 ? 1 : value;
        return UnityEditor.EditorGUILayout.IntField(label, value);
    }
}