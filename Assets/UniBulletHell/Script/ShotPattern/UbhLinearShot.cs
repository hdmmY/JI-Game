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
    public float _Angle = 180f;
    // "Set a delay time between bullet and next bullet. (sec)"
    public float _BetweenDelay = 0.1f;

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
            if (0 < i && 0f < _BetweenDelay) {
                yield return StartCoroutine(UbhUtil.WaitForSeconds(_BetweenDelay));
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            ShotBullet(bullet, m_bulletSpeed, _Angle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the shot direction
        Gizmos.color = Color.green;
        float angle = _Angle + 90;
        float length = m_autoReleaseTime * m_bulletSpeed;
        Gizmos.DrawLine(transform.position, transform.position +
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0) * length);
    }
}