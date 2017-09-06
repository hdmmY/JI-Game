using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh waving nway shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Waving nWay Shot")]
public class UbhWavingNwayShot : UbhBaseShot
{
    // "Set a number of shot way."
    public int _WayNum = 5;
    // "Set a center angle of wave range. (0 to 360)"
    [Range(0f, 360f)]
    public float _WaveCenterAngle = 180f;
    // "Set a size of wave range. (0 to 360)"
    [Range(0f, 360f)]
    public float _WaveRangeSize = 40f;
    // "Set a speed of wave. (0 to 10)"
    [Range(0f, 10f)]
    public float _WaveSpeed = 5f;
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

            float centerAngle = _WaveCenterAngle + (_WaveRangeSize / 2f * Mathf.Sin(UbhTimer.Instance.FrameCount * _WaveSpeed / 100f));

            float baseAngle = _WayNum % 2 == 0 ? centerAngle - (_BetweenAngle / 2f) : centerAngle;

            float angle = UbhUtil.GetShiftedAngle(wayIndex, baseAngle, _BetweenAngle);

            ShotBullet(bullet, _BulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);

            wayIndex++;
        }

        FinishedShot();
    }
}