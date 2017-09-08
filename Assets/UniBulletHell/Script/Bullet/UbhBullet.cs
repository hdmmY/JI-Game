using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh bullet.
/// </summary>
public class UbhBullet : UbhMonoBehaviour
{
    public bool _Shooting
    {
        get;
        private set;
    }

    void OnDisable ()
    {
        StopAllCoroutines();
        transform.ResetPosition();
        transform.ResetRotation();
        _Shooting = false;
    }

    /// <summary>
    /// Bullet Shot
    /// </summary>
    public void Shot (float speed, float angle, float accelSpeed, float accelTurn,
                      bool homing, Transform homingTarget, float homingAngleSpeed, float maxHomingAngle,
                      bool wave, float waveSpeed, float waveRangeSize,
                      bool pauseAndResume, float pauseTime, float resumeTime, UbhUtil.AXIS axisMove)
    {
        if (_Shooting) {
            return;
        }
        _Shooting = true;

        StartCoroutine(MoveCoroutine(speed, angle, accelSpeed, accelTurn,
                                     homing, homingTarget, homingAngleSpeed, maxHomingAngle,
                                     wave, waveSpeed, waveRangeSize,
                                     pauseAndResume, pauseTime, resumeTime, axisMove));
        
        //UbhBullet testBullet = new UbhBullet();
        //testBullet._Shooting = false;
    }


    // 
    public void Shot(IEnumerator bulletMoveRoutine)
    {
        if(_Shooting)
        {
            return;
        }
        _Shooting = true;

        StartCoroutine(bulletMoveRoutine);
    }



    IEnumerator MoveCoroutine (float speed, float angle, float accelSpeed, float accelTurn,
                               bool homing, Transform homingTarget, float homingAngleSpeed, float maxHomingAngle,
                               bool wave, float waveSpeed, float waveRangeSize,
                               bool pauseAndResume, float pauseTime, float resumeTime, UbhUtil.AXIS axisMove)
    {
        if (axisMove == UbhUtil.AXIS.X_AND_Z) {
            // X and Z axis
            transform.SetEulerAnglesY(-angle);
        } else {
            // X and Y axis
            transform.SetEulerAnglesZ(angle);
        }

        float selfFrameCnt = 0f;
        float selfTimeCount = 0f;
        float homingAngle = 0f;

        while (true) {
            if (homing) {
                // homing target.
                if (homingTarget != null && 0f < homingAngleSpeed) {
                    float rotAngle = UbhUtil.GetAngleFromTwoPosition(transform, homingTarget, axisMove);
                    float myAngle = 0f;
                    if (axisMove == UbhUtil.AXIS.X_AND_Z) {
                        // X and Z axis
                        myAngle = -transform.eulerAngles.y;
                    } else {
                        // X and Y axis
                        myAngle = transform.eulerAngles.z;
                    }

                    float toAngle = Mathf.MoveTowardsAngle(myAngle, rotAngle, UbhTimer.Instance.DeltaTime * homingAngleSpeed);

                    homingAngle += Mathf.Abs(myAngle - toAngle) >= 30 ? 0 : Mathf.Abs(myAngle - toAngle);
                    if(homingAngle >= maxHomingAngle)  homing = false;                   

                    if (axisMove == UbhUtil.AXIS.X_AND_Z) {
                        // X and Z axis
                        transform.SetEulerAnglesY(-toAngle);
                    } else {
                        // X and Y axis
                        transform.SetEulerAnglesZ(toAngle);
                    }
                }

            } else if (wave) {
                // acceleration turning.
                angle += (accelTurn * UbhTimer.Instance.DeltaTime);
                // wave.
                if (0f < waveSpeed && 0f < waveRangeSize) {
                    float waveAngle = angle + (waveRangeSize / 2f * Mathf.Sin(selfFrameCnt * waveSpeed / 100f));
                    if (axisMove == UbhUtil.AXIS.X_AND_Z) {
                        // X and Z axis
                        transform.SetEulerAnglesY(-waveAngle);
                    } else {
                        // X and Y axis
                        transform.SetEulerAnglesZ(waveAngle);
                    }
                }
                selfFrameCnt++;

            } else {
                // acceleration turning.
                float addAngle = accelTurn * UbhTimer.Instance.DeltaTime;
                if (axisMove == UbhUtil.AXIS.X_AND_Z) {
                    // X and Z axis
                    transform.AddEulerAnglesY(-addAngle);
                } else {
                    // X and Y axis
                    transform.AddEulerAnglesZ(addAngle);
                }
            }

            // acceleration speed.
            speed += (accelSpeed * UbhTimer.Instance.DeltaTime);

            // move.
            if (axisMove == UbhUtil.AXIS.X_AND_Z) {
                // X and Z axis
                transform.position += transform.forward.normalized * speed * UbhTimer.Instance.DeltaTime;
            } else {
                // X and Y axis
                transform.position += transform.up.normalized * speed * UbhTimer.Instance.DeltaTime;
            }

            yield return 0;

            selfTimeCount += UbhTimer.Instance.DeltaTime;

            // pause and resume.
            if (pauseAndResume && pauseTime >= 0f && resumeTime > pauseTime) {
                while (pauseTime <= selfTimeCount && selfTimeCount < resumeTime) {
                    yield return 0;
                    selfTimeCount += UbhTimer.Instance.DeltaTime;
                }
            }
        }
    }
}