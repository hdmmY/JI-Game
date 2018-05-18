using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class JiPathMoveCtrl : JiMoveCtrlBase
{
    public bool m_alwaysShowPath = false;

    [ListDrawerSettings (ShowIndexLabels = true, DraggableItems = true, Expanded = false)]
    public List<JIPathInfo> m_Paths;

    [ReadOnly, ShowInInspector]
    private float _timer;

    [ReadOnly, ShowInInspector]
    private int _curPathIndex;
    private bool _curPathInvoked;

    public override void Start ()
    {
        base.Start ();

        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        _timer = -0.5f;

        _curPathIndex = 0;
        _curPathInvoked = false;
    }

    protected void Update ()
    {
        if (m_Paths == null) return;
        if (m_targetGameObject == null) return;
        if (_curPathIndex > m_Paths.Count) return;

        _timer += JITimer.Instance.DeltTime;

        if (_curPathIndex == m_Paths.Count)
        {
            if (m_distroyWhenEndOfPaths)
            {
                if (m_targetGameObject == this.gameObject)
                {
                    DestroyImmediate (m_targetGameObject);
                }
                else
                {
                    DestroyImmediate (this.gameObject);
                    DestroyImmediate (m_targetGameObject);
                }
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
            StartMove (_curPathIndex);
            _curPathInvoked = true;
            return;
        }

        if ((_timer >= endTime - 0.01f))
        {
            _timer = 0f;
            _curPathIndex++;
            _curPathInvoked = false;
            iTween.Stop (m_targetGameObject);
            return;
        }
    }

    public override void StopMove ()
    {
        if (m_targetGameObject != null)
        {
            iTween.Stop (m_targetGameObject, "moveto");
        }
    }

    // Lauch the itween variabeles
    public override Hashtable LauchArgs (int index)
    {
        if (m_Paths == null)
        {
            Debug.LogError ("There is no path!");
            return null;
        }

        if (m_Paths.Count <= index)
        {
            Debug.LogError ("The path index is not right!");
            return null;
        }

        var pathInfo = m_Paths[index];

        Hashtable args = new Hashtable ();

        args.Add ("axis", "z"); // restrict the rotation to z-axis only.
        args.Add ("path", pathInfo.m_controlPoints.ToArray ());
        args.Add ("movetopath", pathInfo.m_moveTo);
        args.Add ("easetype", pathInfo.m_easeType);
        args.Add ("looptype", pathInfo.m_loopType);
        args.Add ("time", pathInfo.m_time);

        return args;
    }

    private void OnDrawGizmosSelected ()
    {
        foreach (var pathInfo in m_Paths)
        {
            if (pathInfo.m_controlPoints == null || pathInfo.m_controlPoints.Count < 2)
                continue;
            Gizmos.DrawIcon (pathInfo.m_controlPoints[0], "Point", true);
            Gizmos.DrawIcon (pathInfo.m_controlPoints[pathInfo.m_controlPoints.Count - 1], "Point", true);
            iTween.DrawPathGizmos (pathInfo.m_controlPoints.ToArray ());
        }
    }

    private void OnDrawGizmos ()
    {
        if (!m_alwaysShowPath) return;

        OnDrawGizmosSelected ();
    }

#if UNITY_EDITOR
    [Button ("Preview", ButtonSizes.Medium)]
    [HideInPlayMode]
    public void Preview ()
    {
        var prevGo = Instantiate (this.gameObject) as GameObject;
        var prevTargetGo = gameObject == m_targetGameObject ?
            prevGo : Instantiate (m_targetGameObject) as GameObject;

        prevGo.hideFlags = HideFlags.HideAndDontSave;
        prevTargetGo.hideFlags = HideFlags.HideAndDontSave;

        prevGo.SetActive (false);
        prevTargetGo.SetActive (false);

        var originMove = prevGo.GetComponent<JiPathMoveCtrl> ();
        var newMove = prevGo.AddComponent<JiPathMovePreview> ();

        // Copy origin component's data to new preview commponent
        newMove.m_alwaysShowPath = originMove.m_alwaysShowPath;
        newMove.m_distroyWhenEndOfPaths = true;
        newMove.m_Paths = new List<JIPathInfo> ();
        foreach (var path in originMove.m_Paths)
        {
            newMove.m_Paths.Add (new JIPathInfo
            {
                m_controlPoints = new List<Vector3> (path.m_controlPoints),
                    m_delayTime = path.m_delayTime,
                    m_easeType = path.m_easeType,
                    m_loopTimes = path.m_loopTimes,
                    m_loopType = path.m_loopType,
                    m_moveTo = path.m_moveTo,
                    m_time = path.m_time
            });
        }
        newMove.m_targetGameObject = prevTargetGo;

        DestroyImmediate (prevGo.GetComponent<JiPathMoveCtrl> ());

        prevTargetGo.SetActive (true);
        prevGo.SetActive (true);
    }

    /// <summary>
    /// Only use for preview path move
    /// </summary>
    [ExecuteInEditMode]
    protected class JiPathMovePreview : JiPathMoveCtrl { }
#endif
}

[System.Serializable]
public struct JIPathInfo
{
    /// <summary>
    /// Path nodes
    /// </summary>
    [ListDrawerSettings (NumberOfItemsPerPage = 4, Expanded = false)]
    public List<Vector3> m_controlPoints;

    /// <summary>
    /// Set a delay time to start move when this path is invoked
    /// </summary>
    [CustomValueDrawer ("ClampToNoneNagativeFloat")]
    [Tooltip ("The delay time to start move when this path is invoked")]
    public float m_delayTime;

    /// <summary>
    /// Time in seconds the movement will take to complete 
    /// </summary>
    [CustomValueDrawer ("ClampToNoneNagativeFloat")]
    [Tooltip ("Time in seconds the movement will take to complete")]
    public float m_time;

    /// <summary>
    /// The ease type of the movement
    /// </summary>
    [Tooltip ("The ease type of the movement")]
    public iTween.EaseType m_easeType;

    /// <summary>
    /// The loop type of the movement.
    /// </summary>
    [Tooltip ("The loop type of the movement")]
    public iTween.LoopType m_loopType;

    [HideIf ("m_loopType", iTween.LoopType.none)]
    [CustomValueDrawer ("ClampToNoneNegativeInt")]
    public int m_loopTimes;

    /// <summary>
    /// The gameobject will move to the start of the path
    /// </summary>  
    [Tooltip ("The gameobject will move to the start of the path")]
    public bool m_moveTo;

#if UNITY_EDITOR
    private static float ClampToNoneNagativeFloat (float value, GUIContent label)
    {
        value = value < 0.001f ? 0.001f : value;
        return UnityEditor.EditorGUILayout.FloatField (label, value);
    }

    private static int ClampToNoneNegativeInt (int value, GUIContent label)
    {
        value = value < 1 ? 1 : value;
        return UnityEditor.EditorGUILayout.IntField (label, value);
    }
#endif

}