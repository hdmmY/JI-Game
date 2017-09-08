using System.Collections;
using UnityEngine;



namespace Stage1Shot
{
    public class GlowwarmShot : UbhBaseShot
    {
        // Set number of shot spiral way
        public int m_SpiralWayNum = 4;

        // Set the starting angle of the shot
        [Range(0f, 360f)]
        public float m_StartAngle = 180f;

        // Set a shift angle of spiral
        [Range(-360f, 360f)]
        public float m_ShiftAngle = 30f;

        // The time delay between bullet and next shoot bullet in the same spiral way
        public float m_BulletEmitInterval = 0.2f;

        // The bullet will pause for a while before change the direction
        public float m_PauseBeforeChangeDirection = 0.5f;

        // The bullet shifted angle after it change direction.
        public float m_AddAngleAfterChangeDirection = 30f;

        // The bullet speed after it change direction.
        public float m_BulletSpeedAfterChangeDir = 2f;


        public override void Shot()
        {
            StartCoroutine(ShotCoroutine());
        }


        IEnumerator ShotCoroutine()
        {
            if (_BulletNum <= 0 || _BulletSpeed <= 0f || m_SpiralWayNum <= 0)
            {
                Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed or SpiralWayNum is not set.");
                yield break;
            }

            if (_Shooting)
            {
                yield break;
            }
            _Shooting = true;

            float spiralWayShiftAngle = 360f / m_SpiralWayNum;
            int spiralWayIndex = 0;

            for (int i = 0; i < _BulletNum; i++)
            {
                for(spiralWayIndex = 0; spiralWayIndex < m_SpiralWayNum; spiralWayIndex++)
                {
                    var bullet = GetBullet(transform.position, transform.rotation);
                    if (bullet == null)
                    {
                        break;
                    }

                    float angle = m_StartAngle + (spiralWayShiftAngle * spiralWayIndex) + m_ShiftAngle * i;

                    ShotBullet(bullet, BulletMove(bullet, angle));
                    AutoReleaseBulletGameObject(bullet.gameObject);

                    if(spiralWayIndex == m_SpiralWayNum - 1 && m_BulletEmitInterval > 0f)
                    {
                        yield return StartCoroutine(UbhUtil.WaitForSeconds(m_BulletEmitInterval));
                    }                       
                }  
            }

            _Shooting = false;
        }



        // The method for bullet move
        IEnumerator BulletMove(UbhBullet bullet, float angle)
        {
            Transform bulletTrans = bullet.transform;
            float speed = _BulletSpeed;
            float accelSpeed = _AccelerationSpeed;
            float selfTimeCount = 0;

            if(bulletTrans == null)
            {
                Debug.LogWarning("The shooting bullet is not exist!");
                yield break;
            }

            UbhUtil.AXIS axisMove = ShotCtrl != null ? ShotCtrl._AxisMove : UbhUtil.AXIS.X_AND_Y;
            if (axisMove == UbhUtil.AXIS.X_AND_Z) // X and Z axis
            {
                bulletTrans.SetEulerAnglesY(-angle);
            } 
            else  // X and Y axis 
            {
                bulletTrans.SetEulerAnglesZ(angle);
            }


            while(true)
            {
                float addAngle = _AccelerationTurn * UbhTimer.Instance.DeltaTime;
                if (axisMove == UbhUtil.AXIS.X_AND_Z) // X and Z axis
                {
                   bulletTrans.AddEulerAnglesY(-addAngle);
                } 
                else // X and Y axis
                {
                     bulletTrans.AddEulerAnglesZ(addAngle);
                }

                speed += accelSpeed * UbhTimer.Instance.DeltaTime;
                if (axisMove == UbhUtil.AXIS.X_AND_Z) // X and Z axis
                {
                    bulletTrans.position += bulletTrans.forward.normalized * speed * UbhTimer.Instance.DeltaTime;
                } 
                else // X and Y axis
                {
                    bulletTrans.position += bulletTrans.up.normalized * speed * UbhTimer.Instance.DeltaTime;
                }

                yield return 0;

                selfTimeCount += UbhTimer.Instance.DeltaTime;

                // Shoot two other bullet
                if(selfTimeCount > Mathf.Abs(_BulletSpeed / accelSpeed))
                {
                    yield return UbhUtil.WaitForSeconds(m_PauseBeforeChangeDirection);  
                    
                    speed = m_BulletSpeedAfterChangeDir;
                    angle = UbhUtil.GetAngleFromTwoPosition(bulletTrans, transform, axisMove) - 90; 

                    var bulletUpper = GetBullet(bulletTrans.position, bulletTrans.rotation);
                    var bulletUnder = GetBullet(bulletTrans.position, bulletTrans.rotation);
                    if(bulletUpper == null || bulletUnder == null)
                    {
                        break;
                    }
                    ShotChildBullet(bulletUpper, speed, angle + m_AddAngleAfterChangeDirection, axisMove);
                    ShotChildBullet(bulletUnder, speed, angle - m_AddAngleAfterChangeDirection, axisMove);
                    AutoReleaseBulletGameObject(bulletUpper.gameObject);
                    AutoReleaseBulletGameObject(bulletUnder.gameObject);

                    UbhObjectPool.Instance.ReleaseGameObject(bulletTrans.gameObject);
                    FinishedShot();

                    yield break;
                }

                // pause and resume.
                if (_UsePauseAndResume && _PauseTime >= 0f && _ResumeTime > _PauseTime) {
                    while (_PauseTime <= selfTimeCount && selfTimeCount < _ResumeTime) {
                        yield return 0;
                        selfTimeCount += UbhTimer.Instance.DeltaTime;
                    }
                }
            }
        }


        // Different from ShotBullet Method(in UbhBaseShot Class), it just care about bullet angle and speed
        private void ShotChildBullet(UbhBullet bullet, float speed, float angle, 
                UbhUtil.AXIS axisMove = UbhUtil.AXIS.X_AND_Y)
        {
            if(bullet == null)  return;
            
            bullet.Shot(speed, angle, 
                    0, 0, false, null, 0, 0, false, 0, 0, false, 0,  0, axisMove);
        }
    }
}


