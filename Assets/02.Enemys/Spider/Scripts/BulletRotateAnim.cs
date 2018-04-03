using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (UbhLinearShot))]
public class BulletRotateAnim : MonoBehaviour
{
    public float m_rotateSpeed;

    public float m_minAngle;

    public float m_maxAngle;

    private UbhLinearShot _targetShot;

    private void Start ()
    {
        _targetShot = GetComponent<UbhLinearShot> ();

        if (_targetShot == null)
        {
            Debug.LogErrorFormat ("The {0} doesn't have a UbhBaseShot component!", this.gameObject.name);
        }
    }

    public void Update ()
    {
        float targetAngle = m_rotateSpeed * JITimer.Instance.TimeScale + _targetShot.m_shotAngle;

        if (targetAngle < m_minAngle)
        {
            m_rotateSpeed = -m_rotateSpeed;
            targetAngle = 2 * m_minAngle - targetAngle;
        }
        else if (targetAngle > m_maxAngle)
        {
            m_rotateSpeed = -m_rotateSpeed;
            targetAngle = 2 * m_maxAngle - targetAngle;
        }

        _targetShot.m_shotAngle = targetAngle;
    }
}