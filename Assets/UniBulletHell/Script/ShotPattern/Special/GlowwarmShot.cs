using System.Collections;
using UnityEngine;



namespace SpecialShot
{
    public class GlowwarmShot : UbhBaseShot
    {
        [Space]
        [Header("Special Properties")]

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

        // The bullet will be destroy by player bullet when its velocity is zero
        public bool m_destroyWhenVelocityZero;

        [HideInInspector]
        public int m_destroyBulletHealth;

        public override void Shot()
        {
            StartCoroutine(ShotCoroutine());
        }


        IEnumerator ShotCoroutine()
        {
            if (m_bulletNum <= 0 || m_SpiralWayNum <= 0)
            {
                Debug.LogWarning("Cannot shot because BulletNum or SpiralWayNum is not set.");
                yield break;
            }

            if (_Shooting)
            {
                yield break;
            }
            _Shooting = true;

            float spiralWayShiftAngle = 360f / m_SpiralWayNum;

            for (int i = 0; i < m_bulletNum; i++)
            {
                for (int spiralWayIndex = 0; spiralWayIndex < m_SpiralWayNum; spiralWayIndex++)
                {
                    var bullet = GetBullet(transform.position, transform.rotation);
                    if (bullet == null)
                    {
                        break;
                    }

                    float angle = m_StartAngle + (spiralWayShiftAngle * spiralWayIndex) + m_ShiftAngle * i;

                    if(m_destroyWhenVelocityZero)
                    {
                        var bulletChild = bullet.transform.GetChild(0).gameObject;

                        var destoryableScript = bulletChild.AddComponent<DestroyableBullet>();
                        destoryableScript.m_bulletTag = "PlayerBullet";
                        destoryableScript.m_destroyVelocity = 0;
                        destoryableScript.CurrentBulletVelocity += () => { return 0; };
                        destoryableScript.DestoryBullet += () =>
                        {
                            UbhObjectPool.Instance.ReleaseGameObject(bullet.gameObject);
                        };
                    }

                    ShotBullet(bullet, BulletMove(bullet, angle));
                    AutoReleaseBulletGameObject(bullet.gameObject);
                }

                if (m_BulletEmitInterval > 0f)
                    yield return StartCoroutine(UbhUtil.WaitForSeconds(m_BulletEmitInterval));
            }

            _Shooting = false;
        }



        // The method for bullet move
        IEnumerator BulletMove(UbhBullet bullet, float angle)
        {
            Transform bulletTrans = bullet.transform;

            float bulletSpeed = m_bulletSpeed;
            float accelerationSpeed = m_accelerationSpeed;
            float angleSpeed = m_angleSpeed;
            float addAngleAfterChangeDirection = m_AddAngleAfterChangeDirection;

            float selfTimeCount = 0;

            float pauseBeforeChangeDirection = m_PauseBeforeChangeDirection;
            float bulletSpeedAfterChangeDir = m_BulletSpeedAfterChangeDir;

            bool usePauseAndResume = m_usePauseAndResume;
            float pauseTime = m_pauseTime;
            float resumeTime = m_resumeTime;

            if (bulletTrans == null)
            {
                Debug.LogWarning("The shooting bullet is not exist!");
                yield break;
            }

            bulletTrans.SetEulerAnglesZ(angle);

            while (true)
            {
                float addAngle = angleSpeed * UbhTimer.Instance.DeltaTime;
                bulletTrans.AddEulerAnglesZ(addAngle);

                bulletSpeed += accelerationSpeed * UbhTimer.Instance.DeltaTime;
                bulletTrans.position += bulletTrans.up * bulletSpeed * UbhTimer.Instance.DeltaTime;

                yield return 0;

                selfTimeCount += UbhTimer.Instance.DeltaTime;

                // When the speed == 0, shoot two other bullet
                if (selfTimeCount > Mathf.Abs(bulletSpeed / accelerationSpeed))
                {
                    yield return UbhUtil.WaitForSeconds(pauseBeforeChangeDirection);

                    bulletSpeed = bulletSpeedAfterChangeDir;
                    //angle = UbhUtil.GetAngleFromTwoPosition(bulletTrans, transform, axisMove) - 90;

                    var bulletUpper = GetBullet(bulletTrans.position, bulletTrans.rotation);
                    var bulletUnder = GetBullet(bulletTrans.position, bulletTrans.rotation);
                    if (bulletUpper == null || bulletUnder == null)
                    {
                        break;
                    }

                    ShotChildBullet(bulletUpper, bulletSpeed, angle + addAngleAfterChangeDirection);
                    ShotChildBullet(bulletUnder, bulletSpeed, angle - addAngleAfterChangeDirection);
                    AutoReleaseBulletGameObject(bulletUpper.gameObject);
                    AutoReleaseBulletGameObject(bulletUnder.gameObject);

                    UbhObjectPool.Instance.ReleaseGameObject(bulletTrans.gameObject);
                    FinishedShot();

                    yield break;
                }


                // pause and resume.
                if (usePauseAndResume && pauseTime >= 0f && resumeTime > pauseTime)
                {
                    while (pauseTime <= selfTimeCount && selfTimeCount < resumeTime)
                    {
                        yield return 0;
                        selfTimeCount += UbhTimer.Instance.DeltaTime;
                    }
                }
            }
        }


        // Different from ShotBullet Method(in UbhBaseShot Class), it just care about bullet angle and speed
        private void ShotChildBullet(UbhBullet bullet, float speed, float angle)
        {
            if (bullet == null) return;

            bullet.Shot(speed, angle,
                    0, 0, false, null, 0, 0, false, 0, 0, false, 0, 0);
        }
    }
}


