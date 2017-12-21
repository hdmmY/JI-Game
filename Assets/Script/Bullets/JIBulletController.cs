using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JIBulletController : MonoBehaviour
{
    private bool _shooting;

    #region Events
    // Call on bullet destroy
    public System.Action<JIBulletController> OnBulletDestroy;

    private void CallOnBulletDestroy()
    {
        if (OnBulletDestroy != null)
        {
            OnBulletDestroy(this);
        }
    }

    // Clear all delegates in OnBulletDestroy 
    private void ClearOnBulletDestroyList()
    {
        if (OnBulletDestroy != null)
        {
            var del = OnBulletDestroy.GetInvocationList();

            for (int i = 0; i < del.Length; i++)
            {
                OnBulletDestroy -= del[i] as System.Action<JIBulletController>;
            }
        }
    }
    #endregion


    private void OnEnable()
    {
        _shooting = false;

        ClearOnBulletDestroyList();
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        CallOnBulletDestroy();
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
            Debug.Log("Here!");
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
                    float toAngle = Mathf.MoveTowardsAngle(myAngle, rotAngle, GetTime(useRealTime) * homingAngleSpeed);

                    homingAngle += Mathf.Abs(myAngle - toAngle) >= 30 ? 0 : Mathf.Abs(myAngle - toAngle);
                    if (homingAngle >= maxHomingAngle) homing = false;

                    transform.SetEulerAnglesZ(toAngle);
                }

            }
            else if (wave)
            {
                // acceleration turning.
                angle += (angleSpeed * GetTime(useRealTime));
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
                float addAngle = angleSpeed * GetTime(useRealTime);
                transform.AddEulerAnglesZ(addAngle);
            }

            // acceleration speed.
            speed += (accelSpeed * GetTime(useRealTime));

            // move.
            transform.position += transform.up * speed * GetTime(useRealTime);

            yield return 0;

            selfTimeCount += GetTime(useRealTime);

            // pause and resume.
            if (pauseAndResume && pauseTime >= 0f && resumeTime > pauseTime)
            {
                while (pauseTime <= selfTimeCount && selfTimeCount < resumeTime)
                {
                    yield return 0;
                    selfTimeCount += GetTime(useRealTime);
                }
            }
        }
    }


    float GetTime(bool useRealTime)
    {
        return useRealTime ? JITimer.Instance.RealDeltTime : JITimer.Instance.DeltTime;
    }

}
