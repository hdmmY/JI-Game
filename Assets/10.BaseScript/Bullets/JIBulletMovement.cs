using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

[RequireComponent (typeof (JIBulletProperty))]
public class JIBulletMovement : MonoBehaviour
{
    #region Public properties and methods

    /// <summary>
    /// Stop the movement
    /// </summary>
    [ShowInInspector]
    public bool Stop
    {
        get { return _stop; }
        set { _stop = value; }
    }

    [ShowInInspector]
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    [ShowInInspector]
    public float Accelerate
    {
        get { return _accel; }
        set { _accel = value; }
    }

    [ShowInInspector]
    public float AngleSpeed
    {
        get { return _angleSpeed; }
        set { _angleSpeed = value; }
    }

    [ShowInInspector]
    public float AngleAccelerate
    {
        get { return _angleAccel; }
        set { _angleAccel = value; }
    }

    #endregion

    #region Private variables

    private bool _stop = false;
    private float _speed = 0f;
    private float _accel = 0f;
    private float _angleSpeed = 0f;
    private float _angleAccel = 0f;

    #endregion

    #region Monobehavior

    private void LateUpdate ()
    {
        float deltTime = JITimer.Instance.DeltTime;

        if (Stop) return;

        _speed += Accelerate * deltTime;
        _angleSpeed += AngleAccelerate * deltTime;

        Vector3 rot = transform.rotation.eulerAngles;
        rot.z += AngleSpeed * deltTime;
        transform.rotation = Quaternion.Euler (rot);

        transform.position += transform.up * Speed * deltTime;
    }

    #endregion
}