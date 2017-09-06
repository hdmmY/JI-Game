using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Ubh random shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Random Shot")]
public class UbhRandomShot : UbhBaseShot
{
    // "Center angle of random range."
    [Range(0f, 360f)]
    public float _RandomCenterAngle = 180f;
    // "Set a angle size of random range. (0 to 360)"
    [Range(0f, 360f)]
    public float _RandomRangeSize = 360f;
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
    // "Evenly distribute of all bullet angle."
    public bool _EvenlyDistribute = true;

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
        if (_BulletNum <= 0 || _RandomSpeedMin <= 0f || _RandomSpeedMax <= 0) {
            Debug.LogWarning("Cannot shot because BulletNum or RandomSpeedMin or RandomSpeedMax is not set.");
            yield break;
        }
        if (_Shooting) {
            yield break;
        }
        _Shooting = true;

        List<int> numList = new List<int>();

        for (int i = 0; i < _BulletNum; i++) {
            numList.Add(i);
        }

        while (0 < numList.Count) {
            int index = Random.Range(0, numList.Count);
            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            float bulletSpeed = Random.Range(_RandomSpeedMin, _RandomSpeedMax);

            float minAngle = _RandomCenterAngle - (_RandomRangeSize / 2f);
            float maxAngle = _RandomCenterAngle + (_RandomRangeSize / 2f);
            float angle = 0f;

            if (_EvenlyDistribute) {
                float oneDirectionNum = Mathf.Floor((float) _BulletNum / 4f);
                float quarterIndex = Mathf.Floor((float) numList[index] / oneDirectionNum);
                float quarterAngle = Mathf.Abs(maxAngle - minAngle) / 4f;
                angle = Random.Range(minAngle + (quarterAngle * quarterIndex), minAngle + (quarterAngle * (quarterIndex + 1f)));

            } else {
                angle = Random.Range(minAngle, maxAngle);
            }

            ShotBullet(bullet, bulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);

            numList.RemoveAt(index);

            if (0 < numList.Count && 0f <= _RandomDelayMin && 0f < _RandomDelayMax) {
                float waitTime = Random.Range(_RandomDelayMin, _RandomDelayMax);
                yield return StartCoroutine(UbhUtil.WaitForSeconds(waitTime));
            }
        }

        FinishedShot();
    }
}