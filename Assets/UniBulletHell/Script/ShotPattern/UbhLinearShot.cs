using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh linear shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Linear Shot")]
public class UbhLinearShot : UbhBaseShot
{
    // "Set a angle of shot. (0 to 360)"
    [Range(0f, 360f)]
    public float m_shotAngle = 180f;
    // "Set a delay time between bullet and next bullet. (sec)"
    public float m_timeBetweenDelay = 0.1f;

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
        if (m_bulletNum <= 0) {
            Debug.LogWarning("Cannot shot because BulletNum is not set.");
            yield break;
        }
        if (_Shooting) {
            yield break;
        }
        _Shooting = true;

        for (int i = 0; i < m_bulletNum; i++) {
            if (0 < i && 0f < m_timeBetweenDelay) {
                yield return StartCoroutine(UbhUtil.WaitForSeconds(m_timeBetweenDelay));
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            ShotBullet(bullet, m_bulletSpeed, m_shotAngle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float shotAngle = m_shotAngle + 90f;
        float shotDistance = m_bulletSpeed * m_autoReleaseTime;
        Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * shotAngle), Mathf.Sin(Mathf.Deg2Rad * shotAngle), 0);
        Gizmos.DrawLine(transform.position, transform.position + direction * shotDistance);   
    }
}