using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh spread nway shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Spread nWay Shot")]
public class UbhSpreadNwayShot : UbhBaseShot
{
    // "Set a number of shot way."
    public int _WayNum = 8;
    // "Set a center angle of shot. (0 to 360)"
    [Range(0f, 360f)]
    public float _CenterAngle = 180f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f)]
    public float _BetweenAngle = 10f;
    // "Set a difference speed between shot and next line shot."
    public float _DiffSpeed = 0.5f;

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
        if (_BulletNum <= 0 || _BulletSpeed <= 0f || _WayNum <= 0) {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed or WayNum is not set.");
            yield break;
        }
        if (_Shooting) {
            yield break;
        }
        _Shooting = true;

        int wayIndex = 0;

        float bulletSpeed = _BulletSpeed;

        for (int i = 0; i < _BulletNum; i++) {
            if (_WayNum <= wayIndex) {
                wayIndex = 0;

                bulletSpeed -= _DiffSpeed;
                while (bulletSpeed <= 0) {
                    bulletSpeed += Mathf.Abs(_DiffSpeed);
                }
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            float baseAngle = _WayNum % 2 == 0 ? _CenterAngle - (_BetweenAngle / 2f) : _CenterAngle;

            float angle = UbhUtil.GetShiftedAngle(wayIndex, baseAngle, _BetweenAngle);

            ShotBullet(bullet, bulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);

            wayIndex++;
        }

        FinishedShot();
    }
}