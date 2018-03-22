using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // The position of the bullet emit point.
    public List<Transform> m_shootList = new List<Transform> ();

    // Prefab of red status bullet
    public GameObject m_whiteBulletPrefab;

    // Prefab of black status bullet
    public GameObject m_blackBulletPrefab;

    public GameObject m_superWhiteBulletPrefab;
    public GameObject m_superBlackBulletPrefab;

    [Space]
    [Header ("Homing")]
    public bool m_homing;

    public float m_homingSpeed;
    public LayerMask m_homingLayer;

    // Use this property to control damage and shot interval.
    private PlayerProperty _playerProperty;

    private PlayerEventMaster _playerEventMaster;

    // Timer for count whether it is enough time after last shot.
    private float _timer;

    void OnEnable ()
    {
        _playerProperty = GetComponent<PlayerProperty> ();
        _playerEventMaster = _playerProperty.m_eventMaster;
    }

    void Update ()
    {
        if (Input.GetButton ("Shot"))
        {
            Shot ();
        }
        else
        {
            _timer = 0f;
        }
    }

    // Player wants to shot.
    void Shot ()
    {
        _timer += JITimer.Instance.DeltTime;

        // it is not enough time after the last shot, shot cancel 
        if (_timer < _playerProperty.m_shootInterval)
            return;

        if (m_shootList == null || m_shootList.Count <= 0)
        {
            Debug.LogWarning ("Cannot shot because ShotList is not set.");
            return;
        }

        for (int i = 0; i < m_shootList.Count; i++)
        {
            JIBulletController bulletController = GetBullet (m_shootList[i].position, Quaternion.identity);
            JIBulletProperty bulletProperty = bulletController.GetComponent<JIBulletProperty> ();

            if (bulletController == null || bulletProperty == null) break;

            bulletProperty.m_damage = _playerProperty.m_bulletDamage;
            ShotBullet (bulletController, _playerProperty.m_bulletSpeed, m_shootList[i].rotation.z + 90f, m_homing);
            _playerEventMaster.CallOnShot (bulletController);
        }

        // finish a shot, reset timer
        _timer = 0f;
    }

    // Get a template bullet in the object pool.
    // position: bullet worldspace position.
    // rotation: bullet worldspace rotation.
    // forceInstantiate: force to instantiate a bullet in object pool and get it.
    JIBulletController GetBullet (Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        GameObject bulletPrefab = null;
        if (_playerProperty.m_superState && _playerProperty.m_playerState == JIState.Black)
            bulletPrefab = m_superBlackBulletPrefab;
        else if (_playerProperty.m_superState && _playerProperty.m_playerState == JIState.White)
            bulletPrefab = m_superWhiteBulletPrefab;
        else if (!_playerProperty.m_superState && _playerProperty.m_playerState == JIState.Black)
            bulletPrefab = m_blackBulletPrefab;
        else
            bulletPrefab = m_whiteBulletPrefab;

        if (bulletPrefab == null)
        {
            return null;
        }

        // Get bullet gameobject from object pool
        var goBullet = UbhObjectPool.Instance.GetGameObject (bulletPrefab, position, rotation, forceInstantiate);
        if (goBullet == null)
        {
            Debug.LogWarning ("Fail to get the bullet from object pool!");
            return null;
        }

        // Get or add JIBulletController component
        var bulletController = goBullet.GetComponent<JIBulletController> ();
        if (bulletController == null)
        {
            bulletController = goBullet.AddComponent<JIBulletController> ();
        }

        // Get or add JIBulletProperty component
        var bulletProperty = goBullet.GetComponent<JIBulletProperty> ();
        if (bulletProperty == null)
        {
            bulletProperty = goBullet.AddComponent<JIBulletProperty> ();
        }

        return bulletController;
    }

    private void ShotBullet (JIBulletController bullet, float speed, float angle, bool homing)
    {
        if (homing)
        {
            var enemy = Physics2D.CircleCast (transform.position, 20, Vector2.one, 0, m_homingLayer).transform;

            if (enemy != null)
            {
                bullet.Shot (speed, angle, 0, 0,
                    true, enemy, m_homingSpeed, 10000,
                    false, 0, 0,
                    false, 0, 0);
                return;
            }
        }

        bullet.Shot (speed, angle, 0, 0,
            false, null, 0, 0,
            false, 0, 0,
            false, 0, 0);

    }

}