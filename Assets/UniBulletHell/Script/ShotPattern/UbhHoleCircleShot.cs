using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh hole circle shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Hole Circle Shot")]
public class UbhHoleCircleShot : UbhBaseShot
{
    // "Set a center angle of hole. (0 to 360)"
    [Range(0f, 360f)]
    public float _HoleCenterAngle = 180f;
    // "Set a size of hole. (0 to 360)"
    [Range(0f, 360f)]
    public float _HoleSize = 20f;

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

        _HoleCenterAngle = UbhUtil.Get360Angle(_HoleCenterAngle);
        float startAngle = _HoleCenterAngle - (_HoleSize / 2f);
        float endAngle = _HoleCenterAngle + (_HoleSize / 2f);

        float shiftAngle = (endAngle - startAngle) / (float) m_bulletNum;

        for (int i = 0; i < m_bulletNum; i++) {
            float angle = startAngle + shiftAngle * i;
            
            //if (startAngle <= angle && angle <= endAngle) {
            //    continue;
            //}

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            ShotBullet(bullet, m_bulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }
}