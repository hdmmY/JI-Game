using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiPathMoveCtrl : JiMoveCtrlBase
{
    public List<JiPathInfo> m_Paths;

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

        float startTime = m_Paths[_curPathIndex].m_DelayTime;
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
        args.Add("name", pathInfo.m_PathData.m_pathName);
        args.Add("path", pathInfo.m_PathData.m_controlPoints.ToArray());
        args.Add("time", pathInfo.m_time);
        args.Add("movetopath", pathInfo.m_MoveTo);
        args.Add("easetype", pathInfo.m_easeType);
        args.Add("looptype", pathInfo.m_loopType);

        return args;
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < m_Paths.Count; i++)
        {
            if (m_Paths[i].m_PathData != null)

                iTween.DrawPath(m_Paths[i].m_PathData.m_controlPoints.ToArray());
        }
    }
}



[System.Serializable]
public class JiPathInfo
{
    // Select a path that you will move on.
    public JiPathData m_PathData;

    // Set a delay time to start move when this path is invoked
    public float m_DelayTime;

    // Time in seconds the movement will take to complete.
    public float m_time = 0f;

    // The ease type of the movement.
    public iTween.EaseType m_easeType = iTween.EaseType.linear;

    // The loop type of the movement.
    public iTween.LoopType m_loopType = iTween.LoopType.none;

    // The gameobject will move to the start of the path
    public bool m_MoveTo = false;
}
