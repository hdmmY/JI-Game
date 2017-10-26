using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiCirclePath : JiPath 
{
    // The circle centre position.
    public Vector2 m_CircleCentre = new Vector2(0f, 0f);

    public int m_startAngle;
    public int m_endAngle;
    public int m_deltAngle = 10;

    // The circle radius.
    public float m_Radius = 1f;
    

    public void GenerateCirclePath()
    {
        m_CtrolNodeCount = 0;
        m_CtrolNode.Clear();

        for (int angle = m_startAngle; angle < m_endAngle; angle += m_deltAngle)
        {
            // start from angle 90f.
            float rad = angle * Mathf.Deg2Rad;
            m_CtrolNode.Add(m_CircleCentre + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * m_Radius);
        }

        m_CtrolNodeCount = m_CtrolNode.Count;
    }


    void OnDrawGizmosSelected()
    {
        if(!m_alwaysVisable)
        {
            Gizmos.DrawCube(m_CircleCentre, Vector3.one * 0.1f);

            if(m_CtrolNode.Count >= 2)
                iTween.DrawPath(m_CtrolNode.ToArray());
        }
    }   

    void OnDrawGizmos()
    {
        if(m_alwaysVisable)
        {
            Gizmos.DrawCube(m_CircleCentre, Vector3.one * 0.1f);

            if(m_CtrolNode.Count >= 2)
                iTween.DrawPath(m_CtrolNode.ToArray());
        }
    }
}
