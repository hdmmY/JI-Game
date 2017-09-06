using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiBullet : MonoBehaviour
{

    /// <summary>
    /// If the bullet has been shot, this property is false; else true.
    /// </summary>
    public bool Shooting
    {
        private set
        {
            _Shooting = value;
        }

        get
        {
            return _Shooting;
        }
    }

    private bool _Shooting;


    /// <summary>
    /// Shoot the bullet
    /// </summary>
    public void Shot(float speed, float angle, float accelSpeed, float accelTurn,
                     bool homing, Transform homingTarget, float homingAngleSpeed,
                     bool wave, float waveSpeed, float waveAngleSize,
                     bool pauseAndResume, float pauseTime, float resumeTime)
    {
        // the bullet has been shot
        if (Shooting)
        {
            return;
        }

        Shooting = true;

        StartCoroutine(MoveBullet(speed, angle, accelSpeed, accelTurn,
                    homing, homingTarget, homingAngleSpeed,
                    wave, waveSpeed, waveAngleSize,
                    pauseAndResume, pauseTime, resumeTime));
    }


    IEnumerator MoveBullet(float speed, float angle, float accelSpeed, float accelTurn,
                     bool homing, Transform homingTarget, float homingAngleSpeed,
                     bool wave, float waveSpeed, float waveAngleSize,
                     bool pauseAndResume, float pauseTime, float resumeTime)
    {
        transform.SetEulerAnglesZ(angle);

        
        while(true)
        {
            if(homing)
            {
                float rotateAngle = UbhUtil.GetAngleFromTwoPosition(transform, homingTarget, UbhUtil.AXIS.X_AND_Y);
                angle += Mathf.MoveTowardsAngle(angle, rotateAngle, UbhTimer.Instance.DeltaTime * homingAngleSpeed);

                transform.SetEulerAnglesZ(angle);
            }
            else if(wave)
            {

            }
        }




    }



}
