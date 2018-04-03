using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialShot
{
    public class LinearBounceShot : UbhBaseShot
    {
        [Space]
        [Header("Special Properties")]

        [Range(0, 360)]
        public float m_shotAngle;

        [Range(0, 30)]
        [Tooltip("The maximum times that bullet can bounce against the edge")]
        public int m_bounceTimes;

        [Range(0.05f, 5)]
        public float m_emitInterval;

        [Tooltip("Use to determine the bouce rect")]
        public Rect m_bounceBound;


        public override void Shot()
        {
            StartCoroutine(ShotCoroutine());
        }

        // Control bullet shot
        IEnumerator ShotCoroutine()
        {
            if (m_bulletNum <= 0)
            {
                Debug.LogWarning("Cannot shot because BulletNum is not set.");
                yield break;
            }

            if (_Shooting)
            {
                yield break;
            }
            _Shooting = true;


            for (int bulletNum = 0; bulletNum < m_bulletNum; bulletNum++)
            {
                var bullet = GetBullet(transform.position, transform.rotation);
                if (bullet == null)
                {
                    break;
                }

                ShotBullet(bullet, BulletMove(bullet, m_shotAngle));
                AutoReleaseBulletGameObject(bullet.gameObject);

                yield return StartCoroutine(UbhUtil.WaitForSeconds(m_emitInterval));
            }

            _Shooting = false;
        }


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
                    bulletTrans.position = GetIntersectWithRect(bulletTrans.position, newPosition, ref bounceBound, ref angle);
                    bulletTrans.SetEulerAnglesZ(angle - 90);
                    bounceTime++;
                }

                yield return 0;
            }
        }

        /// <summary>
        /// Change the bouce bullet when it intersection with the rect
        /// </summary>
        /// <param name="origin"> origin bullet postiion </param>
        /// <param name="end"> origin bullet destination </param>
        /// <param name="rect"> rect that bullet will intersect with </param>
        /// <param name="afterAngle"> bullet angle after it intersect with rect </param>
        /// <returns> new bullet position </returns>
        public static Vector2 GetIntersectWithRect(Vector2 origin, Vector2 end, ref Rect rect, ref float angle)
        {
            // Origin point outside the rect, give it up
            if(!rect.Contains(origin))
            {
                return end;
            }

            Vector2 inDirection = end - origin;
            Vector2 inNormal;

            if (origin.y < rect.yMax && end.y > rect.yMax)
                inNormal = Vector2.down;
            else if (origin.y > rect.yMin && end.y < rect.yMin)
                inNormal = Vector2.up;
            else if (origin.x > rect.xMin && end.x < rect.xMin)
                inNormal = Vector2.right;
            else 
                inNormal = Vector2.left;

            Vector2 outDirction = Vector2.Reflect(inDirection, inNormal);

            if(inNormal == Vector2.up)
            {
                inNormal = Vector2.up;
            }

            angle = Vector2.SignedAngle(Vector2.right, outDirction);
            return outDirction + end;
        }



        // Draw the shot direction
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position +
                new Vector3(Mathf.Cos(Mathf.Deg2Rad * m_shotAngle), Mathf.Sin(Mathf.Deg2Rad * m_shotAngle), 0) * 3);
        }
    }
}