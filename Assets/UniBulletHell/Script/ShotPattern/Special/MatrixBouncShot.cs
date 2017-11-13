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


            for (int i = 0; i < m_bulletNum; i++)
            {
                // Four direction: forwarad, right, back, left
                for (int dir = 0; dir < 4; dir++)
                {
                    // Start angle of each direction
                    float startAngle = m_ShiftAngle + dir * 90f;

                    float endAngle = m_ShiftAngle + (dir + 1) * 90f;

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
                        var bullet = GetBullet(transform.position, transform.rotation);
                        if (bullet == null)
                        {
                            break;
                        }

                        float angle = startAngle + deltAngle * wayIndex;
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

            if (bulletTrans == null)
            {
                Debug.LogWarning("The shooting bullet is not exist!");
                yield break;
            }

            bulletTrans.SetEulerAnglesZ(afterAngle);

            // Bullet move speed before waiting.
            float beforeSpeed = (bulletTrans.position - destination).magnitude / timeToReachRect;
            // Bullet move direction before waiting
            Vector3 beforeDirection = transform.up;
            // Bullet movement before waiting.
            while(true)
            {
                bulletTrans.position += beforeDirection * beforeSpeed * UbhTimer.Instance.DeltaTime;

                // Use vector dot to determine if bullet reach the destination
                Vector3 line = destination - bulletTrans.position;
                if (Vector3.Dot(line, beforeDirection) < 0) break;
            }


            // Wait for a while
            yield return UbhUtil.WaitForSeconds(waitTime);


            // Bullet movement after waiting, it will bouncing against the edge
            int bounceTime;
            while(true)
            {
                Vector3 direction = transform.forward;

            }
        }


        private void OnDrawGizmosSelected()
        {
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

