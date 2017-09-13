using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Stage1Shot
{
    public class ChangeDirecShot : UbhBaseShot
    {
        // A number of shot way.
        public int m_WayNum = 2;

        // The start angle of the shot.
        [Range(-180f, 180f)]
        public float m_StartAngle = 90f;

        // The shift angle of each way.
        [Range(0f, 360f)]
        public float m_ShiftAngle = 30f;

        // The time delay between bullet and next shoot bullet in the same spiral way
        public float m_BulletEmitInterval = 0.2f;

        // Time for first bullet change it's speed.
        public float m_StartChangeSpeedTime = 0.6f;

        public float m_ChangeSpeedInterval = 0.2f;

        // After change the speed, bullet will pause for a while, and then move.
        public float m_PauseWhenChangeSpeed = 0.1f;

        // The bullet will add some angle to change it's direction.
        [Range(-180f, 180f)]
        public float m_AddAngleAfterChangeDirection = -60f;

        // The bullet speed after it change it's direction.
        public float m_BulletSpeedAfterChangeDir = 2f;

        public override void Shot()
        {
            StartCoroutine(ShotCoroutine());
        }

        IEnumerator ShotCoroutine()
        {
            if (_BulletNum <= 0 || _BulletSpeed <= 0f || m_WayNum <= 0)
            {
                Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed or SpiralWayNum is not set.");
                yield break;
            }

            if (_Shooting)
            {
                yield break;
            }
            _Shooting = true;


            // Shot _BulletNum bullets in each way
            for(int i = 0; i < _BulletNum; i++)
            {
                for(int j = 0; j < m_WayNum; j++)
                {
                    float angle = m_StartAngle + j * m_ShiftAngle;

                    var bullet = GetBullet(transform.position, Quaternion.identity);
                    if(bullet == null)  continue;

                    ShotBullet(bullet, BulletMove(bullet, angle, i));
                    AutoReleaseBulletGameObject(bullet.gameObject);
                }

                if(m_BulletEmitInterval > 0f)
                    yield return StartCoroutine(UbhUtil.WaitForSeconds(m_BulletEmitInterval));
            }
            _Shooting = false;
        }


        // The method for bullet move
        // angle: bullet move angle
        // index: bullet index in a bullet way, to determine the change speed time.
        IEnumerator BulletMove(UbhBullet bullet, float angle, int index)
        {
            Transform bulletTrans = bullet.transform;
            float speed = _BulletSpeed;
            float accelSpeed = _AccelerationSpeed;
            float selfTimeCount = 0f;

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

                // Change direction.
                if(selfTimeCount >= (m_StartChangeSpeedTime + m_ChangeSpeedInterval * index))
                {
                    yield return UbhUtil.WaitForSeconds(m_PauseWhenChangeSpeed);

                    angle += m_AddAngleAfterChangeDirection;
                    var childBullet = GetBullet(bulletTrans.position, Quaternion.identity);
                    childBullet.Shot(m_BulletSpeedAfterChangeDir, angle, 
                            0, 0, 
                            false, null, 0, 0, 
                            false, 0, 0, 
                            false, 0, 0,
                            UbhUtil.AXIS.X_AND_Y);

                    AutoReleaseBulletGameObject(bulletTrans.gameObject);
                    UbhObjectPool.Instance.ReleaseGameObject(bulletTrans.gameObject);
                    FinishedShot();

                    yield break;
                }
            }
        }

    }    


}