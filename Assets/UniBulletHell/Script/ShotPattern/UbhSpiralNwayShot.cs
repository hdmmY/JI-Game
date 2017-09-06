using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh spiral nway shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Spiral nWay Shot")]
public class UbhSpiralNwayShot : UbhBaseShot
{
    // "Set a number of shot way."
    public int _WayNum = 5;
    // "Set a starting angle of shot. (0 to 360)"
    [Range(0f, 360f)]
    public float _StartAngle = 180f;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range(-360f, 360f)]
    public float _ShiftAngle = 5f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f)]
    public float _BetweenAngle = 5f;
    // "Set a delay time between shot and next line shot. (sec)"
    public float _NextLineDelay = 0.1f;

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

        for (int i = 0; i < _BulletNum; i++) {
            if (_WayNum <= wayIndex) {
                wayIndex = 0;

                if (0f < _NextLineDelay) {
                    yield return StartCoroutine(UbhUtil.WaitForSeconds(_NextLineDelay));
                }
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            float centerAngle = _StartAngle + (_ShiftAngle * Mathf.Floor(i / _WayNum));

            float baseAngle = _WayNum % 2 == 0 ? centerAngle - (_BetweenAngle / 2f) : centerAngle;

            float angle = UbhUtil.GetShiftedAngle(wayIndex, baseAngle, _BetweenAngle);

            ShotBullet(bullet, _BulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);

            wayIndex++;
        }

        FinishedShot();
    }
}