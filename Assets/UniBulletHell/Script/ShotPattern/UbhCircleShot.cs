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
        if (_BulletNum <= 0 || _BulletSpeed <= 0f) {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed is not set.");
            return;
        }

        float shiftAngle = 360f / (float) _BulletNum;

        for (int i = 0; i < _BulletNum; i++) {
            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            float angle = shiftAngle * i;

            ShotBullet(bullet, _BulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }
}