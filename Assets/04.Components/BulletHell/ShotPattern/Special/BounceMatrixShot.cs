using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SpecialShot
{
    public class BounceMatrixShot : MatrixShot
    {                                           
        [Range(0, 30)]
        [Tooltip("The maximum times that bullet can bounce against the edge")]
        public int m_bounceTimes;

        [Tooltip("Use to determine the bouce rect")]
        public Rect m_bounceBound;


        public override void Shot()
        {
            base.Shot();
        }

        protected override void ShotMatrixBullet(JIBulletController bullet, float angle)
        {
            bullet.StartCoroutine(BulletMove(bullet, angle));
            bullet.OnBulletDestroy += StopAllCoroutOnBullet;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            // Draw the Rect
            Gizmos.DrawWireCube(m_bounceBound.center, m_bounceBound.size);
        }

        private void StopAllCoroutOnBullet(JIBulletController bullet)
        {
            bullet.StopAllCoroutines();
        }


        // Note : Copy from LinearBounceShot script.
        IEnumerator BulletMove(JIBulletController bullet, float angle)
        {
            Transform bulletTrans = bullet.transform;
            float speed = m_bulletSpeed;
            float accel = m_accelerationSpeed;
            float angleSpeed = m_angleSpeed;
            float maxBounceTimes = m_bounceTimes;
            Rect bounceBound = m_bounceBound;

            if (bulletTrans == null)
            {
                Debug.LogWarning("The shooting bullet is not exist!");
                yield break;
            }

            bulletTrans.SetEulerAnglesZ(angle - 90);

            int bounceTime = 0;
            while (true)
            {
                // turning.
                angle += angleSpeed * JITimer.Instance.DeltTime;
                bulletTrans.SetEulerAnglesZ(angle - 90);

                // acceleration speed
                speed += accel * JITimer.Instance.DeltTime;

                // move
                Vector3 newPosition = bulletTrans.position + bulletTrans.up * speed * JITimer.Instance.DeltTime;
                if (bounceBound.Contains(newPosition) || bounceTime >= maxBounceTimes)
                {
                    bulletTrans.position = newPosition;
                }
                else // cross with edge 
                {
                    bulletTrans.position = LinearBounceShot.GetIntersectWithRect(bulletTrans.position, newPosition, ref bounceBound, ref angle);
                    bulletTrans.SetEulerAnglesZ(angle - 90);
                    bounceTime++;
                }

                yield return 0;
            }
        }

    }
}


