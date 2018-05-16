using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (JIBulletProperty))]
public class JIBulletMovement : MonoBehaviour
{
    #region Public member variable

    /// <summary>
    /// Stop the movement vm
    /// </summary>
    public bool Stop;

    /// <summary>
    /// Timescale of the movement vm. Default timescale is 1.
    /// </summary>
    public float TimeScale = 1;

    public float Speed;

    public float Accelerate;

    public float AngleSpeed;

    public float AngleAccelerate;

    #endregion

    #region Private methods and variables

    private float _lastframeTime;

    private void OnEnable ()
    {
        _lastframeTime = Time.realtimeSinceStartup;
    }

    private void Update ()
    {
        float deltTime = (Time.realtimeSinceStartup - _lastframeTime) * TimeScale;
        _lastframeTime = Time.realtimeSinceStartup;

        if (Stop) return;

        Speed += Accelerate * deltTime;
        AngleSpeed += AngleAccelerate * deltTime;

        Vector3 rot = transform.rotation.eulerAngles;
        rot.z += AngleSpeed * deltTime;
        transform.rotation = Quaternion.Euler (rot);

        transform.position += transform.up * Speed * deltTime;
    }

    #endregion
}