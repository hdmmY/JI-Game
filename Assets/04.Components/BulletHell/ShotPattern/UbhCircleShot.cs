    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Ubh circle shot.
    /// </summary>
    public class UbhCircleShot : UbhBaseShot
    {
        public override void Shot ()
        {
            if (m_bulletNum <= 0)
            {
                Debug.LogWarning ("Cannot shot because BulletNum is not set.");
                return;
            }

            float shiftAngle = 360f / (float) m_bulletNum;

            for (int i = 0; i < m_bulletNum; i++)
            {
                var bullet = GetBullet (transform.position, transform.rotation);
                if (bullet == null)
                {
                    break;
                }

                float angle = shiftAngle * i;

                var moveCtrl = bullet.gameObject.AddComponent<GeneralBulletMoveCtrl> ();
                moveCtrl.Angle = angle;
                moveCtrl.Speed = m_bulletSpeed;
                moveCtrl.Init ();

                AutoReleaseBulletGameObject (bullet.gameObject);
            }

            FinishedShot (this);
        }
    }