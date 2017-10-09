using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiPathMoveCtrl : MonoBehaviour
{
    // GameObject that move on the path
    public GameObject m_targetGameObject;

    // Destroy the m_targetGameObject when at the end of the last m_Paths
    public bool m_distroyWhenEndOfPaths = true;

    public List<JiPathInfo> m_Paths;

    private float _timer;
    private int _invokedPathNumber;
    private List<float> _invokeTimes;
    private List<bool> _isInvoked;


    private void Start()
    {
        if (m_targetGameObject == null)
            return;

        _timer = 0f;
        _invokedPathNumber = 0;
        _invokeTimes = new List<float>(m_Paths.Count + 1);
        _isInvoked = new List<bool>(m_Paths.Count);

        if (m_Paths.Count != 0)
        {
            _invokeTimes.Add(m_Paths[0].m_DelayTime + 0.1f);
            for (int i = 1; i < m_Paths.Count; i++)
            {
                _invokeTimes.Add(_invokeTimes[i - 1] + m_Paths[i - 1].m_time + m_Paths[i].m_DelayTime);
            }
            _invokeTimes.Add(_invokeTimes[m_Paths.Count - 1] + m_Paths[m_Paths.Count - 1].m_time);

            for (int i = 0; i < m_Paths.Count; i++) _isInvoked.Add(false);
        }

        iTween.Init(m_targetGameObject);
    }


    private void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        if ((_timer >= _invokeTimes[_invokedPathNumber]) &&   // can be invoked
            (_invokedPathNumber < m_Paths.Count) &&           // not the last time
            (!_isInvoked[_invokedPathNumber]))          // not been invoked
        {
            iTween.MoveTo(m_targetGameObject, Lauch(m_Paths[_invokedPathNumber]));
            _isInvoked[_invokedPathNumber] = true;
            _invokedPathNumber++;
        }

        if (_timer >= _invokeTimes[_invokeTimes.Count - 1])
        {
            if (m_distroyWhenEndOfPaths)
            {
                Destroy(m_targetGameObject, 0.1f);
            }
        }
    }


    private void OnDisable()
    {
        iTween.Stop(m_targetGameObject, "moveto");
    }

    private void OnDestroy()
    {
        iTween.Stop(m_targetGameObject, "moveto");
    }

    // Lauch the itween variabeles
    private Hashtable Lauch(JiPathInfo pathInfo)
    {
        Hashtable args = new Hashtable();

        args.Add("axis", "z");   // restrict the rotation to z-axis only.

        if (pathInfo.m_PathData == null)
        {
            Debug.LogError("There is no path!");
        }
        args.Add("name", pathInfo.m_PathData.m_pathName);
        args.Add("path", pathInfo.m_PathData.m_controlPoints.ToArray());
        args.Add("time", pathInfo.m_time);

        args.Add("movetopath", false);
        args.Add("easetype", pathInfo.m_easeType);
        args.Add("looptype", pathInfo.m_loopType);

        return args;
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < m_Paths.Count; i++)
        {
            if(m_Paths[i].m_PathData != null)

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
}
