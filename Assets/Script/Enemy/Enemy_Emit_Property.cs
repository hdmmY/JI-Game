using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Enemy_Emit_Property : MonoBehaviour
{
    #region Properties

    public bool m_useBound;

    /// <summary>
    /// The centre of the emit circle (in local space)
    /// </summary>
    public Vector3 m_LocalOffset;


    /// <summary>
    /// The target transform that the emit origin followed
    /// </summary>
    public Transform m_FollowedTransform;


    /// <summary>
    /// The radius of the emit circle
    /// </summary>
    public float m_EmitRadius = 0;

    /// <summary>
    /// The number of the emit path
    /// </summary>
    [Range(1, 20)]
    public int m_EmitLineNumber = 1;

    /// <summary>
    /// The angle offset of the emit point
    /// </summary>
    public int m_EmitPointAngleOffset = 0;

    /// <summary>
    /// The interval between each emit
    /// </summary>
    [Range(0.02f, 5)]
    public float m_EmitInterval = 0.1f;

    /// <summary>
    /// The angle offset of the emit Dir
    /// </summary>
    public int m_EmitDirAngleOffset = 0;


    /// <summary>
    /// The angle range of the emit sector
    /// </summary>
    [Range(0, 360)]
    public int m_EmitAngleRange = 360;

    public BulletPool m_bulletPool;

    #endregion


    /// <summary>
    /// Get position of the points that is the emit origin
    /// </summary>
    /// <returns></returns>
    public Vector3[] GetEmitPoints()
    {
        Vector3 originPos;

        if (m_useBound)
            originPos = m_FollowedTransform.position + m_LocalOffset;
        else
            originPos = m_LocalOffset;

        float sectorDeltAngle = 1.0f * m_EmitAngleRange / (m_EmitLineNumber + 1);
        float sectorStartAngle = 1.0f * m_EmitAngleRange / 2;

        if (m_EmitAngleRange == 360)
        {
            sectorDeltAngle = 1.0f * m_EmitAngleRange / m_EmitLineNumber;
        }

        int emitLineNumber = m_EmitLineNumber;

        float[] emitPointAngle = new float[emitLineNumber];
        Vector3[] emitPoint = new Vector3[emitLineNumber];

        for (int i = 0; i < emitLineNumber; i++)
        {
            float originEmitAngle = sectorStartAngle - sectorDeltAngle * (i + 1);

            emitPointAngle[i] = originEmitAngle - m_EmitPointAngleOffset;
            emitPoint[i] = originPos + new Vector3(
                            Mathf.Cos(Mathf.Deg2Rad * emitPointAngle[i]),
                            Mathf.Sin(Mathf.Deg2Rad * emitPointAngle[i]),
                            0f) * m_EmitRadius;
        }

        return emitPoint;
    }



    /// <summary>
    /// Get each emit line dir
    /// </summary>
    /// <returns></returns>
    public float[] GetEmitDirs()
    {
        float sectorDeltAngle = 1.0f * m_EmitAngleRange / (m_EmitLineNumber + 1);
        float sectorStartAngle = 1.0f * m_EmitAngleRange / 2;

        if (m_EmitAngleRange == 360)
        {
            sectorDeltAngle = 1.0f * m_EmitAngleRange / m_EmitLineNumber;
        }

        float[] emitDirAngle = new float[m_EmitLineNumber];

        for (int i = 0; i < m_EmitLineNumber; i++)
        {
            float originEmitAngle = sectorStartAngle - sectorDeltAngle * (i + 1);

            emitDirAngle[i] = originEmitAngle - m_EmitDirAngleOffset;
        }

        return emitDirAngle;
    }



    #region ChangeValueFunction

    public void ChangeEmitRadius(float target)
    {
        this.m_EmitRadius = target;
    }


    public void ChangeEmitLineNumber(int target)
    {
        this.m_EmitLineNumber = target;
    }


    public void ChangePointAngleOffset(int target)
    {
        this.m_EmitPointAngleOffset = target;
    }

    public void ChangeEmitInterval(float target)
    {
        this.m_EmitInterval = target;
    }


    public void ChangeEmitDirAngleOffset(int target)
    {
        this.m_EmitDirAngleOffset = target;
    }


    public void ChangeEmitAngleRange(int target)
    {
        this.m_EmitAngleRange = target;
    }


    #endregion


    #region GetValueFunction

    public float GetEmitRadius()
    {
        return this.m_EmitRadius;
    }


    public int GetEmitLineNumber()
    {
        return this.m_EmitLineNumber;
    }


    public int GetEmitPointAngleOffset()
    {
        return this.m_EmitPointAngleOffset;
    }


    public float GetEmitInterval()
    {
        return this.m_EmitInterval;
    }

    public int GetEmitLineAngleOffset()
    {
        return this.m_EmitDirAngleOffset;
    }


    public int GetEmitAngleRange()
    {
        return this.m_EmitAngleRange;
    }

    #endregion

}
