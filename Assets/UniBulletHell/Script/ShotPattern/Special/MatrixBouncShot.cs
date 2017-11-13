using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialShot
{
    /************************  Note  ***********************************/
    /* This shot pattern don't support "m_bulletSpeed", "m_accelerationSpeed", "m_angleSpeed", "m_usePauseAndResume"*/
    /* "m_pauseTime", "m_resumeTime" parameter. */
    public class MatrixBouncShot : UbhBaseShot
    {
        [Space]
        [Header("Special Properties")]

        [Range(0.2f, 3f)]
        [Tooltip("Width of the rectangle")]
        public float m_RectWidth;

        [Range(1, 8)]
        [Tooltip("Number of the bullet per 90 degree")]
        public int m_NWay = 3;

        [Range(0f, 90f)]
        [Tooltip("Shift angle of spiral")]
        public float m_ShiftAngle = 30f;

        [Range(0.2f, 2f)]
        [Tooltip("The time delay between bullet and next shoot bullet in the same spiral way")]
        public float m_BulletEmitInterval = 0.2f;

        [Range(0.1f, 5f)]
        [Tooltip("Time that bullet will cost to reach the rect edge")]
        public float m_timeToReachRectEdge;

        [Range(0, 3f)]
        [Tooltip("Time that bullet will waiting")]
        public float m_waitingTime;

        [Tooltip("Bullet speed after waiting")]
        public float m_speedAfterWait;

        [Range(0, 10)]
        [Tooltip("The maximum times that bullet can bounce against the edge")]
        public int m_bounceTimes;

        [Tooltip("Use to determine the bouce rect")]
        public Transform m_background;

        public override void Shot()
        {
            StartCoroutine(ShotCoroutine());
        }


        // Control bullet shot
        IEnumerator ShotCoroutine()
        {
            if (m_bulletNum <= 0 || m_NWay <= 0)
            {
                Debug.LogWarning("Cannot shot because BulletNum or NWay is not set.");
                yield break;
            }

            if (_Shooting)
            {
                yield break;
            }
            _Shooting = true;


            for (int bulletNum = 0; bulletNum < m_bulletNum;)
            {
                // Four direction: forwarad, right, back, left
                for (int dir = 0; dir < 4; dir++)
                {
                    // Start angle of each direction
                    float startAngle = m_ShiftAngle + dir * 90f;                                            

                    float endAngle = startAngle + 90f;

                    // Delt angle of each bullet in the direction
                    float deltAngle = 90f / (m_NWay + 1);

                    float length = 1 / Mathf.Sqrt(2) * m_RectWidth;
                    Vector3 startPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * startAngle), Mathf.Sin(Mathf.Deg2Rad * startAngle), 0);
                    startPos *= length;
                    startPos += transform.position;
                    Vector3 endPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * endAngle), Mathf.Sin(Mathf.Deg2Rad * endAngle), 0);
                    endPos *= length;
                    endPos += transform.position;

                    // Each direction show NWay number of the bullet
                    for (int wayIndex = 0; wayIndex < m_NWay; wayIndex++)
                    {
                        bulletNum++;
                                                                                                  
                        var bullet = GetBullet(transform.position, transform.rotation);
                        if (bullet == null)
                        {
                            break;
                        }

                        float angle = (90f / (m_NWay + 1)) * (wayIndex + 1) + startAngle;
                        Vector3 destination = ((endPos - startPos) / (m_NWay + 1)) * (wayIndex + 1) + startPos;
                              
                        ShotBullet(bullet, BulletMove(bullet, angle, startAngle + 45, destination));
                        AutoReleaseBulletGameObject(bullet.gameObject);
                    }
                }

                if (m_BulletEmitInterval > 0f)
                    yield return StartCoroutine(UbhUtil.WaitForSeconds(m_BulletEmitInterval));
            }

            _Shooting = false;
        }


        /// <summary>
        /// The method for bullet move
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="beforeAngle"> bullet move angle before waiting</param>
        /// <param name="afterAngle"> bullet move angle after waiting </param>
        /// <param name="destination"> bullet waiting destination </param>
        /// <returns></returns>
        IEnumerator BulletMove(UbhBullet bullet, float beforeAngle, float afterAngle, Vector3 destination)
        {
            Transform bulletTrans = bullet.transform;

            float timeToReachRect = m_timeToReachRectEdge;
            float waitTime = m_waitingTime;
            float speedAfterWait = m_speedAfterWait;
            float maxBounceTimes = m_bounceTimes;

            Rect edgeRect = new Rect(m_background.position, m_background.localScale);
            edgeRect.Set(edgeRect.x - edgeRect.width / 2, edgeRect.y - edgeRect.height / 2, edgeRect.width, edgeRect.height);

            if (bulletTrans == null)
            {
                Debug.LogWarning("The shooting bullet is not exist!");
                yield break;
            }

            bulletTrans.SetEulerAnglesZ(beforeAngle - 90f);

            Vector3 startPosition = bulletTrans.position;
            float timer = 0f;
            // Bullet movement before waiting.
            while (true)
            {
                float t = timer / timeToReachRect;
                bulletTrans.position = Vector3.Lerp(startPosition, destination, t);
                timer += UbhTimer.Instance.DeltaTime;

                if(Mathf.Abs(t - 1) < 0.01f)
                {
                    break;
                }

                yield return null;
            }

            // Wait for a while
            yield return UbhUtil.WaitForSeconds(waitTime);

            bulletTrans.SetEulerAnglesZ(afterAngle - 90);

            // Bullet movement after waiting, it will bouncing against the edge
            int bounceTime = 0;
            while (true)
            {                                           
                Vector3 newPosition = bulletTrans.position + bulletTrans.up * speedAfterWait * UbhTimer.Instance.DeltaTime;

                if (edgeRect.Contains(newPosition) || bounceTime >= maxBounceTimes)
                {
                    bulletTrans.position = newPosition;
                }
                else // cross with edge 
                {
                    bulletTrans.position = GetIntersectWithRect(bulletTrans.position, newPosition, ref edgeRect, ref afterAngle);
                    bulletTrans.SetEulerAnglesZ(afterAngle - 90);
                    bounceTime++;
                }

                yield return null;
            }
        }


        private Vector2 GetIntersectWithRect(Vector2 origin, Vector2 end, ref Rect rect, ref float afterAngle)
        {
            bool cross;

            Vector2 upRight = rect.center + new Vector2(rect.width / 2, rect.height / 2);
            Vector2 upLeft = rect.center + new Vector2(-rect.width / 2, rect.height / 2);
            Vector2 downRight = rect.center + new Vector2(rect.width / 2, -rect.height / 2);
            Vector2 downLeft = rect.center + new Vector2(-rect.width / 2, -rect.height / 2);

            Vector2 closestPoint;
            Vector2 originDir = end - origin;
            Vector2 newDir;

            afterAngle = UbhUtil.Get360Angle(afterAngle);

            closestPoint = GetClosePoint(ref origin, ref end, ref upRight, ref downRight, out cross);
            if (cross)
            {
                newDir = new Vector2(-originDir.x, originDir.y);
                afterAngle = originDir.y > 0 ? 180 - afterAngle : 540 - afterAngle;
                return closestPoint + newDir;
            }

            closestPoint = GetClosePoint(ref origin, ref end, ref downRight, ref downLeft, out cross);
            if (cross)
            {
                newDir = new Vector2(originDir.x, -originDir.y);
                afterAngle = 360 - afterAngle; 
                return closestPoint + newDir;
            }

            closestPoint = GetClosePoint(ref origin, ref end, ref downLeft, ref upLeft, out cross);
            if (cross)
            {
                newDir = new Vector2(-originDir.x, originDir.y);
                afterAngle = originDir.y > 0 ? 180 - afterAngle : 540 - afterAngle;
                return closestPoint + newDir;
            }

            closestPoint = GetClosePoint(ref origin, ref end, ref upLeft, ref upRight, out cross);
            newDir = new Vector2(originDir.x, -originDir.y);
            afterAngle = 360 - afterAngle;
            return closestPoint + newDir;
        }


        /// <summary>
        /// Get the cross point
        /// </summary>
        /// <param name="start"> start of the line </param>
        /// <param name="end"> end of the line </param>
        /// <param name="rect"> edge rect </param>
        /// <returns></returns>
        private Vector2 GetClosePoint(ref Vector2 segment0_origin, ref Vector2 segment0_end, ref Vector2 segment1_origin, ref Vector2 segment1_end, out bool cross)
        {
            Vector2 segment0_dir = segment0_end - segment0_origin;
            Vector2 segment1_dir = segment1_end - segment1_origin;

            Vector2 w0 = segment0_origin - segment1_origin;
            float a = segment0_dir.x * segment0_dir.x + segment0_dir.y * segment0_dir.y;
            float b = segment0_dir.x * segment1_dir.x + segment0_dir.y * segment1_dir.y;
            float c = segment1_dir.x * segment1_dir.x + segment1_dir.y * segment1_dir.y;
            float d = segment0_dir.x * w0.x + segment0_dir.y * w0.y;
            float e = segment1_dir.x * w0.x + segment1_dir.y * w0.y;

            float denom = a * c - b * b;
            float s_c, t_c;
            float sn, sd, tn, td;  // parameters to compute s_c, t_c

            if (Mathf.Approximately(denom, 0))
            {
                // clamp s_c to 0
                sd = td = c;
                sn = 0.0f;
                tn = e;
            }
            else
            {
                // clamp s_c within [0,1]
                sd = td = denom;
                sn = b * e - c * d;
                tn = a * e - b * d;

                // clamp s_c to 0
                if (sn < 0.0f)
                {
                    sn = 0.0f;
                    tn = e;
                    td = c;
                }
                // clamp s_c to 1
                else if (sn > sd)
                {
                    sn = sd;
                    tn = e + b;
                    td = c;
                }
            }

            // clamp t_c within [0,1]
            // clamp t_c to 0
            if (tn < 0.0f)
            {
                t_c = 0.0f;
                // clamp s_c to 0
                if (-d < 0.0f)
                {
                    s_c = 0.0f;
                }
                // clamp s_c to 1
                else if (-d > a)
                {
                    s_c = 1.0f;
                }
                else
                {
                    s_c = -d / a;
                }
            }
            // clamp t_c to 1
            else if (tn > td)
            {
                t_c = 1.0f;
                // clamp s_c to 0
                if ((-d + b) < 0.0f)
                {
                    s_c = 0.0f;
                }
                // clamp s_c to 1
                else if ((-d + b) > a)
                {
                    s_c = 1.0f;
                }
                else
                {
                    s_c = (-d + b) / a;
                }
            }
            else
            {
                t_c = tn / td;
                s_c = sn / sd;
            }

            Vector2 wc = w0 + s_c * segment0_dir - t_c * segment1_dir;
            Vector2 result = s_c * segment0_dir + segment0_origin;
            float wc2 = wc.x * wc.x + wc.y * wc.y;

            if (wc2 > -0.05f && wc2 < 0.05f)
            {
                cross = true;
                return result;
            }
            else
            {
                cross = false;
                return Vector2.zero;
            }                      
        }




        private void OnDrawGizmosSelected()
        {
            // Draw the bounce edge
            if (m_background != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(m_background.position, m_background.localScale);
            }


            // Four direction: forwarad, right, back, left
            for (int dir = 0; dir < 4; dir++)
            {
                // Start angle of each direction
                float startAngle = m_ShiftAngle + dir * 90f;

                float endAngle = m_ShiftAngle + (dir + 1) * 90f;

                float length = 1 / Mathf.Sqrt(2) * m_RectWidth;
                Vector3 startPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * startAngle), Mathf.Sin(Mathf.Deg2Rad * startAngle), 0);
                startPos *= length;
                startPos += transform.position;
                Vector3 endPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * endAngle), Mathf.Sin(Mathf.Deg2Rad * endAngle), 0);
                endPos *= length;
                endPos += transform.position;

                Gizmos.color = Color.green;
                Gizmos.DrawLine(startPos, endPos);

                // Each direction show NWay number of the bullet
                Gizmos.color = Color.red;
                for (int wayIndex = 0; wayIndex < m_NWay; wayIndex++)
                {
                    Vector3 destination = ((endPos - startPos) / (m_NWay + 1)) * (wayIndex + 1) + startPos;

                    Gizmos.DrawCube(destination, Vector3.one * 0.05f);
                }
            }
        }

    }
}

