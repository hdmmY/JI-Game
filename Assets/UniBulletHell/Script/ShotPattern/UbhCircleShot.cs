using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh circle shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Circle Shot")]
public class UbhCircleShot : UbhBaseShot
{
    protected override void Awake ()
    {
        base.Awake();
    }

    public override void Shot ()
    {
        if (m_bulletNum <= 0) {
            Debug.LogWarning("Cannot shot because BulletNum is not set.");
            return;
        }

        float shiftAngle = 360f / (float) m_bulletNum;

        for (int i = 0; i < m_bulletNum; i++) {
            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            float angle = shiftAngle * i;

            ShotBullet(bullet, m_bulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }
}