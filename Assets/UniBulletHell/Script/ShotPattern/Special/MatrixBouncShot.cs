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

        [Range(0.1f, 5f)]
        [Tooltip("Time that bullet will cost to reach the rect edge")]
        public float m_timeToReachRectEdge;

        [Range(0, 3f)]
        [Tooltip("Time that bullet will waiting")]
        public float m_waitingTime;

        [Tooltip("Use to determine the bouce rect")]
        public Rect m_bounceBound;

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

            if (m_bulletNum != m_NWay * 4)
            {
                Debug.LogWarning("Cannot shot because BulletNum != NWay * 4!");
                yield break;
            }

            if (_Shooting)
            {
                yield break;
            }
            _Shooting = true;


            float length = 1 / Mathf.Sqrt(2) * m_RectWidth;
            float deltAngle = 90f / (m_NWay + 1);

            // Four direction: forwarad, right, back, left
            for (int dir = 0; dir < 4; dir++)
            {
                for(int wayIndex = 1; wayIndex <= m_NWay; wayIndex++)
                {
                    float angle = m_ShiftAngle + dir * 90 + deltAngle * wayIndex;
                    Vector2 destination = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * length;
                    destination += (Vector2)transform.position;

                    var bullet = GetBullet(transform.position, transform.rotation);
                    if (bullet == null) break;
                    bullet.transform.SetEulerAnglesZ(angle);

                    ShotBullet(bullet, BulletMove(bullet, angle, destination));
                    AutoReleaseBulletGameObject(bullet.gameObject);
                }



                //// Start angle of each direction
                //float startAngle = m_ShiftAngle + dir * 90f;
                //// End angle of each direction
                //float endAngle = startAngle + 90f;

                //Vector3 startPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * startAngle), Mathf.Sin(Mathf.Deg2Rad * startAngle), 0);
                //startPos *= length;
                //startPos += transform.position;
                //Vector3 endPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * endAngle), Mathf.Sin(Mathf.Deg2Rad * endAngle), 0);
                //endPos *= length;
                //endPos += transform.position;

                //// Each direction show NWay number of the bullet
                //for (int wayIndex = 0; wayIndex < m_NWay; wayIndex++)
                //{
                //    var bullet = GetBullet(transform.position, transform.rotation);
                //    if (bullet == null)
                //    {
                //        break;
                //    }

                //    float angle = (90f / (m_NWay + 1)) * (wayIndex + 1) + startAngle;
                //    Vector3 destination = ((endPos - startPos) / (m_NWay + 1)) * (wayIndex + 1) + startPos;

                //    ShotBullet(bullet, BulletMove(bullet, angle, startAngle + 45, destination));
                //    AutoReleaseBulletGameObject(bullet.gameObject);
                //}
            }

            _Shooting = false;
            FinishedShot();
        }


        /// <summary>
        /// The method for bullet move
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="afterAngle"> bullet move angle after waiting </param>
        /// <param name="destination"> bullet waiting destination </param>
        /// <returns></returns>
        IEnumerator BulletMove(JIBulletController bullet, float afterAngle, Vector3 destination)
        {
            Transform bulletTrans = bullet.transform;

            float timeToReachRect = m_timeToReachRectEdge;
            float waitTime = m_waitingTime;
            Rect bounceBound = m_bounceBound;

            if (bulletTrans == null)
            {
                Debug.LogWarning("The shooting bullet is not exist!");
                yield break;
            }

            Vector3 moveDir = destination - bulletTrans.position;
            Vector3 startPosition = bulletTrans.position;

            float timer = 0f;
            // Bullet movement before waiting.
            while (true)
            {
                if(m_bindTransform)
                {
                    startPosition = m_bindTransform.position;
                }                         

                float t = Mathf.Clamp01(timer / timeToReachRect);
                bulletTrans.position = moveDir * t + startPosition;
                timer += JITimer.Instance.DeltTime;

                if (t >= 0.995f)
                {
                    break;
                }

                yield return null;
            }
            bulletTrans.SetEulerAnglesZ(afterAngle - 90);

            // Wait for a while
            yield return UbhUtil.WaitForSeconds(waitTime);

            // Get bounce shot component
            var bounceShot = bulletTrans.GetChild(0).GetComponent<LinearBounceShot>();
            bounceShot.m_bounceBound = bounceBound;
            bounceShot.m_shotAngle = afterAngle;
            bounceShot.Shot();
        }


        private void OnDrawGizmosSelected()
        {
            // Draw the bounce edge
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(m_bounceBound.center, m_bounceBound.size);


            //// Four direction: forwarad, right, back, left
            //for (int dir = 0; dir < 4; dir++)
            //{
            //    // Start angle of each direction
            //    float startAngle = m_ShiftAngle + dir * 90f;

            //    float endAngle = m_ShiftAngle + (dir + 1) * 90f;

            //    float length = 1 / Mathf.Sqrt(2) * m_RectWidth;
            //    Vector3 startPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * startAngle), Mathf.Sin(Mathf.Deg2Rad * startAngle), 0);
            //    startPos *= length;
            //    startPos += transform.position;
            //    Vector3 endPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * endAngle), Mathf.Sin(Mathf.Deg2Rad * endAngle), 0);
            //    endPos *= length;
            //    endPos += transform.position;

            //    Gizmos.color = Color.green;
            //    Gizmos.DrawLine(startPos, endPos);

            //    // Each direction show NWay number of the bullet
            //    Gizmos.color = Color.red;
            //    for (int wayIndex = 0; wayIndex < m_NWay; wayIndex++)
            //    {
            //        Vector3 destination = ((endPos - startPos) / (m_NWay + 1)) * (wayIndex + 1) + startPos;

            //        Gizmos.DrawCube(destination, Vector3.one * 0.05f);
            //    }
            //}
        }

    }
}

