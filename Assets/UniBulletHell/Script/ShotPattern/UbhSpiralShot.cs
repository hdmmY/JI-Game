using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh spiral shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Spiral Shot")]
public class UbhSpiralShot : UbhBaseShot
{
    // "Set a starting angle of shot. (0 to 360)"
    [Range(0f, 360f)]
    public float _StartAngle = 180f;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range(-360f, 360f)]
    public float _ShiftAngle = 5f;
    // "Set a delay time between bullet and next bullet. (sec)"
    public float _BetweenDelay = 0.2f;

    protected override void Awake ()
    {
        base.Awake();
    }

    public override void Shot ()
    {
        StartCoroutine(ShotCoroutine());
    }

    IEnumerator ShotCoroutine ()
    {
        if (m_bulletNum<= 0) {
            Debug.LogWarning("Cannot shot because BulletNum is not set.");
            yield break;
        }
        if (_Shooting) {
            yield break;
        }
        _Shooting = true;

        for (int i = 0; i < m_bulletNum; i++) {
            if (0 < i && 0f < _BetweenDelay) {
                yield return StartCoroutine(UbhUtil.WaitForSeconds(_BetweenDelay));
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            float angle = _StartAngle + (_ShiftAngle * i);

            ShotBullet(bullet, m_bulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }
}