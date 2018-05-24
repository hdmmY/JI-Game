using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JIBulletController : MonoBehaviour
{
    private bool _shooting;

    /// <summary>
    /// Call on bullet destroy
    /// </summary>
    public System.Action<JIBulletController> OnBulletDestroy;

    private void OnEnable ()
    {
        _shooting = false;

        // Clear all delegates in OnBulletDestroy 
        if (OnBulletDestroy != null)
        {
            var del = OnBulletDestroy.GetInvocationList ();

            for (int i = 0; i < del.Length; i++)
            {
                OnBulletDestroy -= del[i] as System.Action<JIBulletController>;
            }
        }
    }

    private void OnDisable ()
    {
        StopAllCoroutines ();

        if (OnBulletDestroy != null) OnBulletDestroy (this);
    }

    /// <summary>
    /// General bullet shot
    /// </summary>
    public void Shot (float speed, float angle, float angleSpeed, float accelSpeed,
        bool homing, Transform homingTarget, float homingAngleSpeed, float maxHomingAngle,
        bool wave, float waveSpeed, float waveRangeSize,
        bool pauseAndResume, float pauseTime, float resumeTime, bool useRealTime = false)
    {
        if (_shooting)
        {
            return;
        }
        _shooting = true;

        StartCoroutine (MoveCoroutine (speed, angle, angleSpeed, accelSpeed,
            homing, homingTarget, homingAngleSpeed, maxHomingAngle,
            wave, waveSpeed, waveRangeSize,
            pauseAndResume, pauseTime, resumeTime, useRealTime));
    }

    /// <summary>
    /// User defined bullet shot
    /// </summary>
    /// <param name="bulletMoveRoutine"></param>
    public void Shot (IEnumerator bulletMoveRoutine)
    {
        if (_shooting)
        {
            return;
        }
        _shooting = true;

        StartCoroutine (bulletMoveRoutine);
    }

    IEnumerator MoveCoroutine (float speed, float angle, float angleSpeed, float accelSpeed,
        bool homing, Transform homingTarget, float homingAngleSpeed, float maxHomingAngle,
        bool wave, float waveSpeed, float waveRangeSize,
        bool pauseAndResume, float pauseTime, float resumeTime, bool useRealTime = false)
    {
        angle = angle - 90;

        float selfFrameCnt = 0f;
        float selfTimeCount = 0f;
        float homingAngle = 0f;

        transform.SetEulerAnglesZ (angle);
        while (true)
        {
            if (homing)
            {
                // homing target.
                if (homingTarget != null && 0f < homingAngleSpeed)
                {
                    float rotateAngle = UbhUtil.GetAngleFromTwoPosition (transform, homingTarget) - 90;
                    float myAngle = transform.eulerAngles.z;
                    float toAngle = Mathf.MoveTowardsAngle (myAngle, rotateAngle, JITimer.Instance.DeltTime * homingAngleSpeed);

                    homingAngle += Mathf.Abs (toAngle - myAngle);
                    if (homingAngle <= maxHomingAngle)
                    {
                        transform.SetEulerAnglesZ (toAngle);
                    }
                }
            }
            else if (wave)
            {
                // acceleration turning.
                angle += (angleSpeed * GetTime (useRealTime));
                // wave.
                if (0f < waveSpeed && 0f < waveRangeSize)
                {
                    float waveAngle = angle + (waveRangeSize / 2f * Mathf.Sin (selfFrameCnt * waveSpeed / 100f));
                    transform.SetEulerAnglesZ (waveAngle);
                }
                selfFrameCnt++;

            }
            else
            {
                // turning.
                float addAngle = angleSpeed * GetTime (useRealTime);
                transform.AddEulerAnglesZ (addAngle);
            }

            // acceleration speed.
            speed += (accelSpeed * GetTime (useRealTime));

            // move.
            transform.position += transform.up * speed * GetTime (useRealTime);

            yield return 0;

            selfTimeCount += GetTime (useRealTime);

            // pause and resume.
            if (pauseAndResume && pauseTime >= 0f && resumeTime > pauseTime)
            {
                while (pauseTime <= selfTimeCount && selfTimeCount < resumeTime)
                {
                    yield return 0;
                    selfTimeCount += GetTime (useRealTime);
                }
            }
        }
    }

    float GetTime (bool useRealTime)
    {
        return useRealTime ? JITimer.Instance.RealDeltTime : JITimer.Instance.DeltTime;
    }
}