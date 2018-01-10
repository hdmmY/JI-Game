using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;


/// <summary>
/// Ubh spiral shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Spiral Shot")]
public class UbhSpiralShot : UbhBaseShot
{
    /// <summary>
    /// Set a starting angle of shot. (0 to 360)
    /// </summary>
    [Range(0f, 360f)]
    [BoxGroup("Sprial")]
    public float _StartAngle = 180f;

    /// <summary>
    /// Set a shift angle of spiral. (-360 to 360)
    /// </summary>
    [Range(-360f, 360f)]
    [BoxGroup("Sprial")]
    public float _ShiftAngle = 5f;

    /// <summary>
    /// Set a delay time between bullet and next bullet. (sec)
    /// </summary>
    [BoxGroup("Sprial")]
    [Range(0.02f, 3f)]
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

        FinishedShot(this);
    }

    [BoxGroup("Gizmos")]
    public bool m_showGizmos = true;

    [BoxGroup("Gizmos")]
    [ShowIf("m_showGizmos")]
    public float m_gizmosLength = 2;

    private void OnDrawGizmosSelected()
    {
        if (!m_showGizmos) return;

        float rad = _StartAngle * Mathf.Deg2Rad;
        Vector3 dest = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * 5f + transform.position;

        // Start
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, dest);

        // End
        Gizmos.color = Color.blue;
        rad = ((m_bulletNum - 1) * _ShiftAngle + _StartAngle) * Mathf.Deg2Rad;
        dest = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * 5f + transform.position;
        Gizmos.DrawLine(transform.position, dest);

        for(int i = 0; i < m_bulletNum; i++)
        {
            rad = (i * _ShiftAngle + _StartAngle) * Mathf.Deg2Rad;
            dest = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * m_gizmosLength + transform.position;
            Gizmos.DrawIcon(dest, "Point");
        }
    }
}