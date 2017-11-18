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
        public Transform m_background;


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


        IEnumerator BulletMove(UbhBullet bullet, float angle)
        {
            Transform bulletTrans = bullet.transform;

            float speed = m_bulletSpeed;
            float accel = m_accelerationSpeed;
            float angleSpeed = m_angleSpeed;
            float maxBounceTimes = m_bounceTimes;

            Rect edgeRect = new Rect(m_background.position, m_background.localScale);
            edgeRect.Set(edgeRect.x - edgeRect.width / 2, edgeRect.y - edgeRect.height / 2, edgeRect.width, edgeRect.height);

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
                angle += angleSpeed * UbhTimer.Instance.DeltaTime;
                bulletTrans.SetEulerAnglesZ(angle - 90);

                // acceleration speed
                speed += accel * UbhTimer.Instance.DeltaTime;

                // move
                Vector3 newPosition = bulletTrans.position + bulletTrans.up * speed * UbhTimer.Instance.DeltaTime;
                if (edgeRect.Contains(newPosition) || bounceTime >= maxBounceTimes)
                {
                    bulletTrans.position = newPosition;
                }
                else // cross with edge 
                {
                    bulletTrans.position = GetIntersectWithRect(bulletTrans.position, newPosition, ref edgeRect, ref angle);
                    bulletTrans.SetEulerAnglesZ(angle - 90);
                    bounceTime++;
                }

                yield return 0;
            }
        }

        private Vector2 GetIntersectWithRect(Vector2 origin, Vector2 end, ref Rect rect, ref float afterAngle)
        {
            float up = rect.center.y + rect.height / 2;
            float right = rect.center.x + rect.width / 2;
            float left = rect.center.x - rect.width / 2;
            float down = rect.center.y - rect.height / 2;

            Vector2 crossPoint;
            Vector2 originDir = end - origin;
            Vector2 newDir;
            float t;

            afterAngle = UbhUtil.Get360Angle(afterAngle);

            if (origin.y < up && end.y > up)      // Cross against with up segment of rect
            {
                t = (up - origin.y) / (end.y - origin.y);
                crossPoint = originDir * t + origin;
                newDir = new Vector2(originDir.x, -originDir.y);
                afterAngle = 360 - afterAngle;
                return crossPoint + newDir;
            }

            if (origin.x < right && end.x > right)    // Cross against with right segment of rect
            {
                t = (right - origin.x) / (end.x - origin.x);
                crossPoint = originDir * t + origin;
                newDir = new Vector2(-originDir.x, originDir.y);
                afterAngle = originDir.y > 0 ? 180 - afterAngle : 540 - afterAngle;
                return crossPoint + newDir;
            }

            if (origin.y > down && end.y < down)      // Cross against with down segment of rect
            {
                t = (down - origin.y) / (end.y - origin.y);
                crossPoint = originDir * t + origin;
                newDir = new Vector2(originDir.x, -originDir.y);
                afterAngle = 360 - afterAngle;
                return crossPoint + newDir;
            }

            // Cross against with left segment of rect
            t = (left - origin.x) / (end.x - origin.x);
            crossPoint = originDir * t + origin;
            newDir = new Vector2(-originDir.x, originDir.y);
            afterAngle = originDir.y > 0 ? 180 - afterAngle : 540 - afterAngle;
            return crossPoint + newDir;
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

