using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialShot
{
    public class MatrixBouncShot : UbhBaseShot
    {
        // Width of the rectangle
        [Range(0.2f, 10f)]
        public float m_RectWidth;

        // Set the number of the bullet per 90 degree
        [Range(1, 10)]
        public int m_NWay = 3;  

        // Set a shift angle of spiral
        [Range(-360f, 360f)]
        public float m_ShiftAngle = 30f;

        // The time delay between bullet and next shoot bullet in the same spiral way
        public float m_BulletEmitInterval = 0.2f;

        public override void Shot()
        {
            StartCoroutine(ShotCoroutine());
        }


        IEnumerator ShotCoroutine()
        {
            if (m_bulletNum <= 0 || m_NWay <= 0)
            {
                Debug.LogWarning("Cannot shot because BulletNum or NWay is not set.");
                yield break;
            }

            if(_Shooting)
            {
                yield break;
            }
            _Shooting = true;
                                  

            for(int i = 0; i < m_bulletNum; i++)
            {
                // Four direction: forwarad, right, back, left
                for(int dir = 0; dir < 4; dir++)
                {
                    // Start angle of each direction
                    float startAngle = m_ShiftAngle + dir * 90f;

                    // Delt angle of each bullet in the direction
                    float deltAngle = 90f / (m_NWay + 1);

                    // Each direction show NWay number of the bullet
                    for (int wayIndex = 0; wayIndex < m_NWay; wayIndex++)
                    {
                        var bullet = GetBullet(transform.position, transform.rotation);
                        if (bullet == null)
                        {
                            break;
                        }

                        float angle = startAngle + deltAngle * wayIndex;

                        ShotBullet(bullet, BulletMove(bullet, angle));
                        AutoReleaseBulletGameObject(bullet.gameObject);
                    }
                }

                if (m_BulletEmitInterval > 0f)
                    yield return StartCoroutine(UbhUtil.WaitForSeconds(m_BulletEmitInterval));
            }

            _Shooting = false;
        }


        // The method for bullet move
        IEnumerator BulletMove(UbhBullet bullet, float angle)
        {
            yield break;
            
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Vector3 centre = transform.position;
            
        }

    }
}

