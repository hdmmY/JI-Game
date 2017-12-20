using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialShot
{
    public class MatrixShot : UbhBaseShot
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

        [Range(0.05f, 5)]
        public float m_emitInterval;


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

            if (m_bulletNum % (4 * m_NWay) != 0)
            {
                Debug.LogWarning("Cannot shot because bulletNum % (4 * m_NWay) !=  0!");
                yield break;
            }

            if (_Shooting)
            {
                yield break;
            }
            _Shooting = true;


            int shotTimes = m_bulletNum / (4 * m_NWay);
            float deltAngle = 90f / (m_NWay + 1);

            for (int i = 0; i < shotTimes; i++)
            {
                // Four direction: forwarad, right, back, left
                for (int dir = 0; dir < 4; dir++)
                {
                    for (int wayIndex = 1; wayIndex <= m_NWay; wayIndex++)
                    {
                        float angle = m_ShiftAngle + dir * 90 + deltAngle * wayIndex;

                        var bulletController = GetBullet(transform.position, transform.rotation);
                        if (bulletController == null) break;

                        ShotMatrixBullet(bulletController, angle);
                        AutoReleaseBulletGameObject(bulletController.gameObject);
                    }
                }

                yield return UbhUtil.WaitForSeconds(m_emitInterval);
            }         

            _Shooting = false;
            FinishedShot(this);
        }


        protected virtual void ShotMatrixBullet(JIBulletController bullet, float angle)
        {
            ShotBullet(bullet, m_bulletSpeed, angle);
        }


        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

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

