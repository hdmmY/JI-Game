using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialShot
{
    /************************  Note  ***********************************/
    /* This shot pattern don't support "m_bulletSpeed", "m_accelerationSpeed", "m_angleSpeed", "m_usePauseAndResume"*/
    /* "m_pauseTime", "m_resumeTime" parameter. */
    public class CircleMatrixBounceShot : UbhBaseShot
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
                for (int wayIndex = 1; wayIndex <= m_NWay; wayIndex++)
                {
                    float angle = m_ShiftAngle + dir * 90 + deltAngle * wayIndex;
                    Vector2 destination = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * length;
                    destination += (Vector2)transform.position;

                    var bullet = GetBullet(transform.position, transform.rotation);
                    if (bullet == null) break;

                    ShotBullet(bullet, BulletMove(bullet, angle, destination));
                    AutoReleaseBulletGameObject(bullet.gameObject);
                }  
            }

            _Shooting = false;
            FinishedShot();
        }


        IEnumerator BulletMove(JIBulletController bullet, float angle, Vector3 destination)
        {
            Transform bulletTrans = bullet.transform;
            if (bulletTrans == null)
            {
                Debug.LogWarning("The shooting bullet is not exist!");
                yield break;
            }
            bulletTrans.SetEulerAnglesZ(angle);

            float timeToReachRect = m_timeToReachRectEdge;
            float waitTime = m_waitingTime;
            Rect bounceBound = m_bounceBound;
                        

            // Bullet movement before waiting.
            float t = 0;
            Vector3 moveDir = destination - bulletTrans.position;
            Vector3 startPosition = bulletTrans.position;
            while (true)
            {
                if (m_bindTransform)
                {
                    startPosition = m_bindTransform.position;
                }

                t += JITimer.Instance.DeltTime / timeToReachRect;
                bulletTrans.position = moveDir * t + startPosition;

                if (t >= 0.995f) break;  

                yield return null;
            }

            // Wait for a while
            yield return UbhUtil.WaitForSeconds(waitTime);

            // Get bounce shot component
            var bounceShot = bulletTrans.GetComponentInChildren<LinearBounceShot>();
            bounceShot.m_bounceBound = new Rect(bounceBound);
            bounceShot.m_shotAngle = angle;
            bounceShot.Shot();
        }


        private void OnDrawGizmosSelected()
        {
            // Draw the bounce edge
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(m_bounceBound.center, m_bounceBound.size);

            float length = 1 / Mathf.Sqrt(2) * m_RectWidth;
            float deltAngle = 90f / (m_NWay + 1);
            Vector3 prevPosition = transform.position;


            // Four direction: forwarad, right, back, left
            for (int dir = 0; dir < 4; dir++)
            {
                for (int wayIndex = 1; wayIndex <= m_NWay; wayIndex++)
                {
                    float angle = m_ShiftAngle + dir * 90 + deltAngle * wayIndex;
                    Vector3 destination = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * length;
                    destination += transform.position;

                    Gizmos.DrawCube(destination, Vector3.one * 0.1f);
                    Gizmos.DrawLine(prevPosition, destination);
                    prevPosition = destination;
                }
            }
        }

    }
}

