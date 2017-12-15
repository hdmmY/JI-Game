using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JIBulletController : MonoBehaviour
{
    private bool _shooting;

    private void OnEnable()
    {
        _shooting = true;
    }
                       
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Bullet Shot
    /// </summary>
    public void Shot(float speed, float angle, float angleSpeed, float accelSpeed,
                      bool homing, Transform homingTarget, float homingAngleSpeed, float maxHomingAngle,
                      bool wave, float waveSpeed, float waveRangeSize,
                      bool pauseAndResume, float pauseTime, float resumeTime, bool useRealTime = false)
    {
        if (_shooting)
        {
            return;
        }
        _shooting = true;

        StartCoroutine(MoveCoroutine(speed, angle, angleSpeed, accelSpeed,
                                    homing, homingTarget, homingAngleSpeed, maxHomingAngle,
                                    wave, waveSpeed, waveRangeSize,
                                    pauseAndResume, pauseTime, resumeTime, useRealTime));
    }

    /// <summary>
    /// Bullet Shot
    /// </summary>
    /// <param name="bulletMoveRoutine"></param>
    public void Shot(IEnumerator bulletMoveRoutine)
    {
        if (_shooting)
        {
            return;
        }
        _shooting = true;

        StartCoroutine(bulletMoveRoutine);
    }




    IEnumerator MoveCoroutine(float speed, float angle, float angleSpeed, float accelSpeed,
                        bool homing, Transform homingTarget, float homingAngleSpeed, float maxHomingAngle,
                        bool wave, float waveSpeed, float waveRangeSize,
                        bool pauseAndResume, float pauseTime, float resumeTime, bool useRealTime = false)
    {
        transform.SetEulerAnglesZ(angle);

        float selfFrameCnt = 0f;
        float selfTimeCount = 0f;
        float homingAngle = 0f;

        while (true)
        {
            if (homing)
            {
                // homing target.
                if (homingTarget != null && 0f < homingAngleSpeed)
                {
                    float rotAngle = UbhUtil.GetAngleFromTwoPosition(transform, homingTarget);
                    float myAngle = transform.eulerAngles.z;
                    float toAngle = Mathf.MoveTowardsAngle(myAngle, rotAngle, JITimer.Instance.RealDeltTime * homingAngleSpeed);

                    homingAngle += Mathf.Abs(myAngle - toAngle) >= 30 ? 0 : Mathf.Abs(myAngle - toAngle);
                    if (homingAngle >= maxHomingAngle) homing = false;

                    transform.SetEulerAnglesZ(toAngle);
                }

            }
            else if (wave)
            {
                // acceleration turning.
                angle += (angleSpeed * JITimer.Instance.RealDeltTime);
                // wave.
                if (0f < waveSpeed && 0f < waveRangeSize)
                {
                    float waveAngle = angle + (waveRangeSize / 2f * Mathf.Sin(selfFrameCnt * waveSpeed / 100f));
                    transform.SetEulerAnglesZ(waveAngle);
                }
                selfFrameCnt++;

            }
            else
            {
                // turning.
                float addAngle = angleSpeed * JITimer.Instance.RealDeltTime;
                transform.AddEulerAnglesZ(addAngle);
            }

            // acceleration speed.
            speed += (accelSpeed * JITimer.Instance.RealDeltTime);

            // move.
            transform.position += transform.up * speed * JITimer.Instance.RealDeltTime;

            yield return 0;

            selfTimeCount += JITimer.Instance.RealDeltTime;

            // pause and resume.
            if (pauseAndResume && pauseTime >= 0f && resumeTime > pauseTime)
            {
                while (pauseTime <= selfTimeCount && selfTimeCount < resumeTime)
                {
                    yield return 0;
                    selfTimeCount += JITimer.Instance.RealDeltTime;
                }
            }
        }
    }


}
