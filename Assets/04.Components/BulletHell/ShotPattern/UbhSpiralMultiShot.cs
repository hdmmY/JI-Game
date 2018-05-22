using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh spiral multi shot.
/// </summary>
public class UbhSpiralMultiShot : UbhBaseShot
{
    // "Set a number of shot spiral way."
    public int _SpiralWayNum = 4;
    // "Set a starting angle of shot. (0 to 360)"
    [Range (0f, 360f)]
    public float _StartAngle = 180f;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range (-360f, 360f)]
    public float _ShiftAngle = 5f;
    // "Set a delay time between bullet and next bullet. (sec)"
    public float _BetweenDelay = 0.2f;

    public override void Shot ()
    {
        StartCoroutine (ShotCoroutine ());
    }

    IEnumerator ShotCoroutine ()
    {
        if (m_bulletNum <= 0 || _SpiralWayNum <= 0)
        {
            Debug.LogWarning ("Cannot shot because BulletNum or SpiralWayNum is not set.");
            yield break;
        }
        if (_Shooting)
        {
            yield break;
        }
        _Shooting = true;

        float spiralWayShiftAngle = 360f / _SpiralWayNum;

        int spiralWayIndex = 0;

        for (int i = 0; i < m_bulletNum; i++)
        {
            if (_SpiralWayNum <= spiralWayIndex)
            {
                spiralWayIndex = 0;
                if (0f < _BetweenDelay)
                {
                    yield return StartCoroutine (UbhUtil.WaitForSeconds (_BetweenDelay));
                }
            }

            var bullet = GetBullet (transform.position, transform.rotation);
            var bulletMove = bullet.gameObject.AddComponent<GeneralBulletMoveCtrl> ();

            float angle = _StartAngle + (spiralWayShiftAngle * spiralWayIndex) +
                (_ShiftAngle * Mathf.Floor (i / _SpiralWayNum));

            bulletMove.Angle = angle;
            bulletMove.Speed = m_bulletSpeed;
            bulletMove.Init ();

            AutoReleaseBulletGameObject (bullet.gameObject);

            spiralWayIndex++;
        }

        FinishedShot (this);
    }
}