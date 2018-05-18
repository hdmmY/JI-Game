using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class JiPathCreator : MonoBehaviour
{
    public enum CreateMode
    {
        Simple,
        Circle,
        Point
    };

    [EnumToggleButtons, HideLabel, Title("Create Mode", horizontalLine: false, bold: false)]
    [OnValueChanged("OnChangeCreateMode")]
    public CreateMode m_currentCreateMode;

    [ShowIf("m_currentCreateMode", CreateMode.Point)]
    [TitleGroup("Point Mode Property")]
    public List<Vector3> m_points;

    #region circle relative variable
    [ShowIf("m_currentCreateMode", CreateMode.Circle)]
    [BoxGroup("Circle Mode Property")]
    [OnValueChanged("OnChangeCircleProperty")]
    public Vector2 m_circleCentre;

    [ShowIf("m_currentCreateMode", CreateMode.Circle)]
    [BoxGroup("Circle Mode Property")]
    [OnValueChanged("OnChangeCircleProperty")]
    [Range(0.1f, 10)]
    public float m_radius;

    [ShowIf("m_currentCreateMode", CreateMode.Circle)]
    [BoxGroup("Circle Mode Property")]
    [OnValueChanged("OnChangeCircleProperty")]
    [OnValueChanged("NoMoreThan")]
    public float m_startAngle;

    [ShowIf("m_currentCreateMode", CreateMode.Circle)]
    [BoxGroup("Circle Mode Property")]
    [OnValueChanged("OnChangeCircleProperty")]
    public float m_endAngle;

    [ShowIf("m_currentCreateMode", CreateMode.Circle)]
    [BoxGroup("Circle Mode Property")]
    [OnValueChanged("OnChangeCircleProperty")]
    [Range(5, 60)]
    public int m_deltAngle;

    [ShowIf("m_currentCreateMode", CreateMode.Circle)]
    [BoxGroup("Circle Mode Property")]
    [OnValueChanged("OnChangeCircleProperty")]
    public bool m_reverse;
    #endregion

    /// <summary>
    /// Control nodes of the path, determine the path shape.
    /// </summary>
    [Space]
    [EnableIf("m_currentCreateMode", CreateMode.Simple)]
    [ListDrawerSettings(NumberOfItemsPerPage = 10)]
    public List<Vector3> m_controlNode;

    public Color m_pathColor = new Color(0, 0.743f, 1, 1);

    [Button("Add To Path", ButtonSizes.Medium)]
    public void SavePathData()
    {
        JiPathMoveCtrl pathMoveCtrl = GetComponent<JiPathMoveCtrl>();

        if (m_currentCreateMode == CreateMode.Simple || m_currentCreateMode == CreateMode.Circle)
        {
            JIPathInfo pathInfo = new JIPathInfo();
            pathInfo.m_controlPoints = new List<Vector3>(m_controlNode);
            pathMoveCtrl.m_Paths.Add(pathInfo);
        }
        else if (m_currentCreateMode == CreateMode.Point)
        {
            for (int i = 1; i < m_points.Count; i++)
            {
                JIPathInfo pathInfo = new JIPathInfo();
                pathInfo.m_controlPoints = new List<Vector3>(2);
                pathInfo.m_controlPoints.Add(m_points[i - 1]);
                pathInfo.m_controlPoints.Add(m_points[i]);
                pathMoveCtrl.m_Paths.Add(pathInfo);
            }
        }
        DestroyImmediate(this);
    }

    public void OnChangeCreateMode()
    {
        m_controlNode.Clear();

        if (m_currentCreateMode == CreateMode.Simple)
        {
            m_controlNode.Add(Vector3.zero);
            m_controlNode.Add(Vector3.one);
        }
        else if (m_currentCreateMode == CreateMode.Circle)
        {
            OnChangeCircleProperty();
        }
        else if (m_currentCreateMode == CreateMode.Point)
        {
            if (m_points == null || m_points.Count < 2)
            {
                m_points = new List<Vector3>();
                m_points.Add(Vector3.zero);
                m_points.Add(Vector3.one);
            }
        }
    }

    public void OnChangeCircleProperty()
    {
        if (m_controlNode == null) m_controlNode = new List<Vector3>();
        m_controlNode.Clear();

        float rad;
        int times = ((int)(m_endAngle - m_startAngle)) / m_deltAngle;
        float startAngle, endAngle, deltAngle;

        startAngle = m_reverse ? m_endAngle : m_startAngle;
        endAngle = m_reverse ? m_startAngle : m_endAngle;
        deltAngle = m_reverse ? -m_deltAngle : m_deltAngle;

        for (float angle = startAngle, i = 0; i <= times; angle += deltAngle, i++)
        {
            rad = angle * Mathf.Deg2Rad;
            m_controlNode.Add(m_circleCentre + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * m_radius);
        }
        rad = endAngle * Mathf.Deg2Rad;
        m_controlNode.Add(m_circleCentre + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * m_radius);
    }

    public void NoMoreThan()
    {
        if (m_startAngle > m_endAngle) m_startAngle = m_endAngle;
    }

    private void SimpleModeDrawer()
    {
        Gizmos.DrawIcon(m_controlNode[0], "Start", true);
        Gizmos.DrawIcon(m_controlNode[m_controlNode.Count - 1], "End", true);
        for (int i = 1; i < m_controlNode.Count - 1; i++)
        {
            Gizmos.DrawIcon(m_controlNode[i], "Point", true);
        }
        iTween.DrawPathGizmos(m_controlNode.ToArray(), m_pathColor);
    }

    private void CircleModeDrawer()
    {
        Gizmos.DrawIcon(m_controlNode[0], "Start", true);
        Gizmos.DrawIcon(m_controlNode[m_controlNode.Count - 1], "End", true);
        iTween.DrawPathGizmos(m_controlNode.ToArray(), m_pathColor);
    }

    private void PointModeDrawer()
    {
        Gizmos.DrawIcon(m_points[0], "Start", true);
        Gizmos.DrawIcon(m_points[m_points.Count - 1], "End", true);

        Gizmos.color = m_pathColor;
        Gizmos.DrawLine(m_points[0], m_points[1]);
        for (int i = 1; i < m_points.Count - 1; i++)
        {
            Gizmos.DrawIcon(m_points[i], "Point");
            Gizmos.DrawLine(m_points[i], m_points[i + 1]);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (m_controlNode != null && m_controlNode.Count >= 2)
        {
            if (m_currentCreateMode == CreateMode.Simple)
                SimpleModeDrawer();
            else if (m_currentCreateMode == CreateMode.Circle)
                CircleModeDrawer();
        }

        if (m_points != null && m_points.Count >= 2)
        {
            if (m_currentCreateMode == CreateMode.Point)
                PointModeDrawer();
        }
    }
}
