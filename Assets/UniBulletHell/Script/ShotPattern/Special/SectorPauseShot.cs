using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialShot
{
    /************************  Note  ***********************************/
    /* This shot pattern don't support "m_bulletSpeed", "m_accelerationSpeed", "m_angleSpeed", "m_usePauseAndResume"*/
    /* "m_pauseTime", "m_resumeTime" parameter. */
    public class SectorPauseShot : UbhBaseShot
    {
        [Space]
        [Header("Special Properties")]

        [Range(-180, 180)]
        [Tooltip("The sector's centre angle")]
        public float m_centreAngle;

        [Range(0, 360)]
        [Tooltip("The sector's angle range")]
        public float m_sectroRange;

        [Range(0, 5)]
        [Tooltip("The sector's radius")]
        public float m_sectorRadius;

        [Range(1, 20)]
        [Tooltip("Number of bullet in a sector")]
        public int m_NWay = 10;

        [Range(0.2f, 2f)]
        [Tooltip("The time delay between bullet and next shoot bullet in the same spiral way")]
        public float m_EmitInterval = 0.2f;

        [Range(0.1f, 5f)]
        [Tooltip("Time that bullet will cost to reach the sector's edge")]
        public float m_timeToReachEdge;

        [Range(0, 3f)]
        [Tooltip("Time that bullet will waiting")]
        public float m_waitingTime;

        [Tooltip("Bullet speed after waiting")]
        public float m_speedAfterWait;

        [Tooltip("Bullet acceleration after waiting")]
        public float m_accelAfterWait;

        [Tooltip("Bullet angle speed after waiting")]
        public float m_angleSpeedAfterWait;



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


            float startAngle = m_centreAngle - m_sectroRange / 2; // Start angle of the sector
            for(int bulletNum = 0; bulletNum < m_bulletNum;)
            {
                for(int i = 0; i < m_NWay; i++)
                {
                    bulletNum++;
                    if (bulletNum >= m_bulletNum) break;

                    var bullet = GetBullet(transform.position, transform.rotation);
                    if (bullet == null)
                    {
                        break;
                    }

                    float angle = (m_sectroRange / (m_NWay + 1)) * (i + 1) + startAngle;
                    ShotBullet(bullet, BulletMove(bullet, angle));

                    AutoReleaseBulletGameObject(bullet.gameObject);
                }

                if (m_EmitInterval > 0f)
                    yield return StartCoroutine(UbhUtil.WaitForSeconds(m_EmitInterval));
            }

            _Shooting = false;
        }


        // The method for bullet movemnt
        IEnumerator BulletMove(UbhBullet bullet, float angle)
        {
            Transform bulletTrans = bullet.transform;

            float sectorRadius = m_sectorRadius;
            float timeToReachEdge = m_timeToReachEdge;
            float waitingTime = m_waitingTime;
            float speedAfterWait = m_speedAfterWait;
            float accelAfterWait = m_accelAfterWait;
            float angleSpeedAfterWait = m_angleSpeedAfterWait;

            if (bulletTrans == null)
            {
                Debug.LogWarning("The shooting bullet is not exist!");
                yield break;
            }

            bulletTrans.SetEulerAnglesZ(angle - 90f);

            Vector3 startPos = bulletTrans.position;
            Vector3 endPos = startPos + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0) * sectorRadius;
            float timer = 0f;
            // Bullet movement before waiting
            while (true)
            {
                float t = timer / timeToReachEdge;
                bulletTrans.position = Vector3.Lerp(startPos, endPos, t);
                timer += UbhTimer.Instance.DeltaTime;

                if (t >= 0.995f) break;

                yield return null;
            }

            // Wait for a while
            yield return UbhUtil.WaitForSeconds(waitingTime);

            // Bullet movemnt after waiting
            while(true)
            {
                // turning.
                float addAngle = angleSpeedAfterWait * UbhTimer.Instance.DeltaTime;
                bulletTrans.AddEulerAnglesZ(addAngle);

                // acceleration speed
                speedAfterWait += accelAfterWait * UbhTimer.Instance.DeltaTime;

                // move
                bulletTrans.position += bulletTrans.up * speedAfterWait * UbhTimer.Instance.DeltaTime;
                yield return 0;
            }
        }


        private void OnDrawGizmosSelected()
        {
            float startAngle = m_centreAngle - m_sectroRange / 2; // Start angle of the sector
            float endAngle = m_centreAngle + m_sectroRange / 2;  // End angle of the sector
            
            Vector3 startPoint = new Vector3(Mathf.Cos(Mathf.Deg2Rad * startAngle), Mathf.Sin(Mathf.Deg2Rad * startAngle), 0);
            startPoint *= m_sectorRadius;
            startPoint += transform.position;

            Vector3 endPoint = new Vector3(Mathf.Cos(Mathf.Deg2Rad * endAngle), Mathf.Sin(Mathf.Deg2Rad * endAngle), 0);
            endPoint *= m_sectorRadius;
            endPoint += transform.position;

            List<Vector3> points = new List<Vector3>(m_NWay + 2);
            points.Add(startPoint);

            // Draw the emit point
            Gizmos.color = Color.red;
            for (int i = 0; i < m_NWay; i++)
            {
                float angle = (m_sectroRange / (m_NWay + 1)) * (i + 1) + startAngle;

                Vector3 point = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
                point *= m_sectorRadius;
                point += transform.position;
                points.Add(point);

                Gizmos.DrawCube(point, Vector3.one * 0.05f);
            }

            points.Add(endPoint);

            // Draw the sector outline
            Gizmos.color = Color.green;
            for(int i = 0; i < m_NWay + 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
            Gizmos.DrawLine(transform.position, startPoint);
            Gizmos.DrawLine(transform.position, endPoint);
        }

    }
}

