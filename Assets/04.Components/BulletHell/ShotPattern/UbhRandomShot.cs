using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;


/// <summary>
/// Ubh random shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Random Shot")]
public class UbhRandomShot : UbhBaseShot
{
    /// <summary>
    /// Center angle of random range.
    /// </summary>
    [Range(0f, 360f)]
    [BoxGroup("Random")]
    public float _RandomCenterAngle = 180f;


    /// <summary>
    /// Set a angle size of random range. (0 to 360)
    /// </summary>
    [Range(0f, 360f)]
    [BoxGroup("Random")]
    public float _RandomRangeSize = 360f;

    /// <summary>
    /// Set a minimum bullet speed of shot. BulletSpeed is ignored.
    /// </summary>
    [Range(0, 20)]
    [ValidateInput("ValidRandomSpeedMin", "RandomSpeedMin must smaller than RandomSpeedMax")]
    [BoxGroup("Random")]
    public float _RandomSpeedMin = 1f;

    /// <summary>
    /// Set a maximum bullet speed of shot. BulletSpeed is ignored.
    /// </summary>
    [Range(0, 20)]
    [ValidateInput("ValidRandomSpeedMax", "RadomSpeedMax must bigger than RadomSpeedMin")]
    [BoxGroup("Random")]
    public float _RandomSpeedMax = 3f;

    /// <summary>
    /// Set a minimum delay time between bullet and next bullet. (sec)
    /// </summary>
    [Range(0.01f, 3f)]
    [ValidateInput("ValidRandomDelayMin", "RandomDelayMin must smaller than RandomDelayMax")]
    [BoxGroup("Random")]
    public float _RandomDelayMin = 0.01f;

    /// <summary>
    /// Set a maximum delay time between bullet and next bullet. (sec)
    /// </summary>
    [Range(0.01f, 3f)]
    [ValidateInput("ValidRandomDelayMax", "RandomDelayMax must bigger than RandomDelayMin")]
    [BoxGroup("Random")]
    public float _RandomDelayMax = 0.1f;         

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {
        StartCoroutine(ShotCoroutine());
    }

    IEnumerator ShotCoroutine()
    {
        if (m_bulletNum <= 0 || _RandomSpeedMin <= 0f || _RandomSpeedMax <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or RandomSpeedMin or RandomSpeedMax is not set.");
            yield break;
        }
        if (_Shooting)
        {
            yield break;
        }
        _Shooting = true;

        List<int> numList = new List<int>();

        for (int i = 0; i < m_bulletNum; i++)
        {
            numList.Add(i);
        }

        while (0 < numList.Count)
        {
            int index = Random.Range(0, numList.Count);
            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null)
            {
                break;
            }

            float bulletSpeed = Random.Range(_RandomSpeedMin, _RandomSpeedMax);

            float minAngle = _RandomCenterAngle - (_RandomRangeSize / 2f);
            float maxAngle = _RandomCenterAngle + (_RandomRangeSize / 2f);
            float angle = Random.Range(minAngle, maxAngle);

            ShotBullet(bullet, bulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);

            numList.RemoveAt(index);

            if (0 < numList.Count && 0f <= _RandomDelayMin && 0f < _RandomDelayMax)
            {
                float waitTime = Random.Range(_RandomDelayMin, _RandomDelayMax);
                yield return StartCoroutine(UbhUtil.WaitForSeconds(waitTime));
            }
        }

        FinishedShot(this);
    }


    private void OnDrawGizmosSelected()
    {
        float minAngle = _RandomCenterAngle - _RandomRangeSize / 2;
        float maxAngle = _RandomCenterAngle + _RandomRangeSize / 2;

        float length = 2f;

        Vector3 centre = transform.position;
        Vector3 minStart = new Vector3(Mathf.Cos(minAngle * Mathf.Deg2Rad), Mathf.Sin(minAngle * Mathf.Deg2Rad), 0) * length ;
        Vector3 maxStart = new Vector3(Mathf.Cos(maxAngle * Mathf.Deg2Rad), Mathf.Sin(maxAngle * Mathf.Deg2Rad), 0) * length;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(centre, minStart + centre);
        Gizmos.DrawLine(centre, maxStart + centre);
        for (float i = minAngle; i < maxAngle; i += 8f)
        {
            maxStart = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad), 0) * length;
            Gizmos.DrawLine(minStart + centre, maxStart + centre);
            minStart = maxStart;
        }
        maxStart = new Vector3(Mathf.Cos(maxAngle * Mathf.Deg2Rad), Mathf.Sin(maxAngle * Mathf.Deg2Rad), 0) * length;
        Gizmos.DrawLine(minStart + centre, maxStart + centre);


    }


    #region Inspector Function
    private bool ValidRandomSpeedMax(float radomSpeedMax)
    {
        return radomSpeedMax >= _RandomSpeedMin;
    }

    private bool ValidRandomSpeedMin(float radomSpeedMin)
    {
        return radomSpeedMin <= _RandomSpeedMax;
    }

    private bool ValidRandomDelayMax(float radomDelayMax)
    {
        return radomDelayMax >= _RandomDelayMin;
    }

    private bool ValidRandomDelayMin(float radomDelayMin)
    {
        return radomDelayMin <= _RandomDelayMax;
    }
    #endregion
}