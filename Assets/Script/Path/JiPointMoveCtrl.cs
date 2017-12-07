using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiPointMoveCtrl : JiMoveCtrlBase
{
    public Vector3 m_startPoint;

    public List<JiPathPointInfo> m_Paths;

    private float _timer;

    private int _curPathIndex;
    private bool _curPathInvoked;             

    public override void Start()
    {
        base.Start();

        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        _timer = -0.5f;

        _curPathIndex = 0;
        _curPathInvoked = false;

        m_targetGameObject.transform.position = m_startPoint;

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

        float startTime = m_Paths[_curPathIndex].m_delayTime;
        float endTime = m_Paths[_curPathIndex].m_time + startTime;

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
        args.Add("position", pathInfo.m_destination);
        args.Add("time", pathInfo.m_time);
        args.Add("movetopath", true);
        args.Add("easetype", pathInfo.m_easeType);
        args.Add("looptype", pathInfo.m_loopType);

        return args;
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 pointSize = Vector3.one * 0.2f;

        Gizmos.color = Color.red;

        Gizmos.DrawCube(m_startPoint, pointSize);
        foreach(var pointInfo in m_Paths)
        {
            Gizmos.DrawCube(pointInfo.m_destination, pointSize);
        }
    }

}


[System.Serializable]
public class JiPathPointInfo
{
    // The destination you want to move to
    public Vector3 m_destination;

    // Set a delay time to start move when this path is invoked
    public float m_delayTime;

    // Time in seconds the movement will take to complete.
    public float m_time = 0f;

    // The ease type of the movement.
    public iTween.EaseType m_easeType = iTween.EaseType.linear;

    // The loop type of the movement.
    public iTween.LoopType m_loopType = iTween.LoopType.none;
}