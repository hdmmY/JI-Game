using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;


/// <summary>
/// Ubh nWay shot.
/// </summary>
public class UbhNwayShot : UbhBaseShot
{
    // note : In N way shot pattern, each way has m_bulletNum bullets.

    /// <summary>
    /// Set a number of shot way.
    /// </summary>
    [BoxGroup("N Way Shot")]
    public int _WayNum = 5;

    /// <summary>
    /// Set a center angle of shot. (0 to 360)
    /// </summary>
    [Range(0f, 360f)]
    [BoxGroup("N Way Shot")]
    public float _CenterAngle = 180f;

    /// <summary>
    /// Set a angle between bullet and next bullet. (0 to 360)
    /// </summary>
    [Range(0f, 360f)]
    [BoxGroup("N Way Shot")]
    public float _BetweenAngle = 10f;

    /// <summary>
    /// Set a delay time between shot and next line shot. (sec)
    /// </summary>
    [Range(0.02f, 3f)]
    [BoxGroup("N Way Shot")]
    public float _NextLineDelay = 0.1f;

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
        if (m_bulletNum <= 0 || _WayNum <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or WayNum is not set.");
            yield break;
        }
        if (_Shooting)
        {
            yield break;
        }
        _Shooting = true;

        for (int i = 0; i < m_bulletNum; i++)
        {
            for (int wayIndex = 0; wayIndex < _WayNum; wayIndex++)
            {
                var bullet = GetBullet(transform.position, transform.rotation);
                if (bullet == null) break;

                float baseAngle = _WayNum % 2 == 0 ? _CenterAngle - (_BetweenAngle / 2f) : _CenterAngle;
                float angle = UbhUtil.GetShiftedAngle(wayIndex, baseAngle, _BetweenAngle);

                ShotBullet(bullet, m_bulletSpeed, angle);
                AutoReleaseBulletGameObject(bullet.gameObject);
            }
            yield return StartCoroutine(UbhUtil.WaitForSeconds(_NextLineDelay));
        }              

        FinishedShot(this);
    }

    [BoxGroup("Gizmos")]
    public bool m_showGizmos = true;

    [BoxGroup("Gizmos")]
    [ShowIf("m_showGizmos")]
    public float m_gizmosLength = 1;

    private void OnDrawGizmosSelected()
    {
        if (!m_showGizmos) return;

        float rad;
        Vector3 centre = transform.position;
        Vector3 dest;

        Gizmos.color = Color.green;
        for (int i = 0; i < _WayNum; i++)
        {
            rad = (_CenterAngle + _BetweenAngle * i) * Mathf.Deg2Rad;
            dest = centre + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * m_gizmosLength;

            if (i == 0 || i == _WayNum - 1)
            {
                Gizmos.DrawLine(centre, dest);
            }

            Gizmos.DrawIcon(dest, "Point");
        }
    }
}