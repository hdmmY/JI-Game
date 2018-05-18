using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialShot
{
    /************************  Note  ***********************************/
    /* This shot pattern don't support "m_bulletSpeed", "m_accelerationSpeed", "m_angleSpeed", "m_usePauseAndResume"*/
    /* "m_pauseTime", "m_resumeTime" parameter. */
    public class SubBounceMatrixShot : MatrixShot
    {  
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
            base.Shot();
        }                       

        protected override void ShotMatrixBullet(JIBulletController bullet, float angle)
        {
            float length = 1 / Mathf.Sqrt(2) * m_RectWidth;
            Vector3 destination = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * length;
            destination += transform.position;                            

            bullet.StartCoroutine(BulletMove(bullet, angle, destination));
            bullet.OnBulletDestroy += StopAllCoroutOnBullet;
        }

        private void StopAllCoroutOnBullet(JIBulletController bullet)
        {
            bullet.StopAllCoroutines();
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


        protected override void  OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
        }

    }
}

