using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh random spiral multi shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Random Spiral Multi Shot")]
public class UbhRandomSpiralMultiShot : UbhBaseShot
{
    // "Set a number of shot spiral way."
    public int _SpiralWayNum = 4;
    // "Set a starting angle of shot. (0 to 360)"
    [Range(0f, 360f)]
    public float _StartAngle = 180f;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range(-360f, 360f)]
    public float _ShiftAngle = 5f;
    // "Set a angle size of random range. (0 to 360)"
    [Range(0f, 360f)]
    public float _RandomRangeSize = 30f;
    // "Set a minimum bullet speed of shot."
    // "BulletSpeed is ignored."
    public float _RandomSpeedMin = 1f;
    // "Set a maximum bullet speed of shot."
    // "BulletSpeed is ignored."
    public float _RandomSpeedMax = 3f;
    // "Set a minimum delay time between bullet and next bullet. (sec)"
    public float _RandomDelayMin = 0.01f;
    // "Set a maximum delay time between bullet and next bullet. (sec)"
    public float _RandomDelayMax = 0.1f;

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
        if (_BulletNum <= 0 || _RandomSpeedMin <= 0f || _RandomSpeedMax <= 0 || _SpiralWayNum <= 0) {
            Debug.LogWarning("Cannot shot because BulletNum or RandomSpeedMin or RandomSpeedMax or SpiralWayNum is not set.");
            yield break;
        }
        if (_Shooting) {
            yield break;
        }
        _Shooting = true;

        float wayAngle = 360f / _SpiralWayNum;

        int wayIndex = 0;

        for (int i = 0; i < _BulletNum; i++) {
            if (_SpiralWayNum <= wayIndex) {
                wayIndex = 0;

                if (0f <= _RandomDelayMin && 0f < _RandomDelayMax) {
                    float waitTime = Random.Range(_RandomDelayMin, _RandomDelayMax);
                    yield return StartCoroutine(UbhUtil.WaitForSeconds(waitTime));
                }
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            float bulletSpeed = Random.Range(_RandomSpeedMin, _RandomSpeedMax);

            float centerAngle = _StartAngle + (wayAngle * wayIndex) + (_ShiftAngle * Mathf.Floor(i / _SpiralWayNum));
            float minAngle = centerAngle - (_RandomRangeSize / 2f);
            float maxAngle = centerAngle + (_RandomRangeSize / 2f);
            float angle = Random.Range(minAngle, maxAngle);

            ShotBullet(bullet, bulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);

            wayIndex++;
        }

        FinishedShot();
    }
}