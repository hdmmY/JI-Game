using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class JiCirclePath : JiPath 
{
    // The circle centre position.
    public Vector2 m_CircleCentre = new Vector2(0f, 0f);

    public int m_CircleTimes = 2;

    // The circle radius.
    public float m_Radius = 1f;

    // The segment is clockwise?
    public bool m_ClockWise = true;

    // The segment to divide the circle to points.
    public int m_segment = 10;

    // Run in editor mode
    private void Update()
    {
        m_CtrolNode.Clear();
        m_CtrolNode = new List<Vector3>(m_segment + 2);

        // m_CtrolNode.Add(m_StartNode);
        // m_CtrolNode.Add(new Vector2(
        //     (m_CircleCentre.x + m_StartNode.x) / 2f, m_StartNode.y));


        // Segment angle.
        float deltAngle = 360f / m_segment;

        // Whethe the segment is clockwise or not.
        deltAngle = m_ClockWise ? deltAngle : -deltAngle;

        for(int t = 0; t < m_CircleTimes; t++)
        {
            for(int i = 0; i < m_segment; i++)
            {
                // start from angle 90f.
                float rad = (90f - deltAngle * i) * Mathf.Deg2Rad;
                m_CtrolNode.Add(m_CircleCentre + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * m_Radius);
            }
        }
        float theRad = 89f * Mathf.Deg2Rad;
        m_CtrolNode.Add(m_CircleCentre + new Vector2(Mathf.Cos(theRad), Mathf.Sin(theRad)) * m_Radius);

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
