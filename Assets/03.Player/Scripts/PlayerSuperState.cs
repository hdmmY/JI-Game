using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperState : MonoBehaviour
{
    /// </summary>
    public float m_bulletSpeed = 9;
    public int m_bulletDamage = 300;
    public float m_shotInterval = 0.12f;

    /// <summary>
    /// Super state last time
    /// </summary>
    public float m_lastTime = 10;

    private PlayerProperty _playerProperty;
    private PlayerShoot _playerShot;
    private PlayerStateManager _playerStateManager;

    private void Start ()
    {
        _playerProperty = GetComponent<PlayerProperty> ();
        _playerShot = GetComponent<PlayerShoot> ();
        _playerStateManager = GetComponent<PlayerStateManager> ();
    }

    private void Update ()
    {
        if (InputManager.Instance.InputCtrl.MaxBlanceButtonDown)
        {
            if (_playerProperty.m_playerBlackPoint >= _playerProperty.m_maxPlayerPoint &&
                _playerProperty.m_playerWhitePoint >= _playerProperty.m_maxPlayerPoint)
            {
                StartCoroutine (TurnOnSuperState ());
            }
        }
    }

    private IEnumerator TurnOnSuperState ()
    {
        yield break;
        // if (_playerProperty.m_superState)
        //     yield break;

        // float prevBulletSpeed = _playerProperty.m_bulletSpeed;
        // int prevBulletDamage = _playerProperty.m_bulletDamage;
        // float prevShotInterval = _playerProperty.m_shootInterval;

        // _playerProperty.m_superState = true;
        // _playerProperty.m_bulletSpeed = m_bulletSpeed;
        // _playerProperty.m_bulletDamage = m_bulletDamage;
        // _playerProperty.m_shootInterval = m_shotInterval;

        // _playerShot.m_homing = true;

        // _playerStateManager.ChangeState (_playerProperty.m_playerState);

        // // JIGlobalRef.MainCamera.GetComponent<ShockWaveEffect> ().enabled = true;
        // // JIGlobalRef.MainCamera.GetComponent<ShockWaveEffect> ().StartShockWave (transform.position, 0.5f, 1);

        // float timer = 0;
        // while (timer < m_lastTime)
        // {
        //     _playerProperty.m_playerBlackPoint = (int) ((1f - timer / m_lastTime) * _playerProperty.m_maxPlayerPoint);
        //     _playerProperty.m_playerWhitePoint = _playerProperty.m_playerBlackPoint;

        //     timer += JITimer.Instance.DeltTime;
        //     yield return null;
        // }
        // _playerProperty.m_playerBlackPoint = 0;
        // _playerProperty.m_playerWhitePoint = 0;

        // _playerProperty.m_superState = false;
        // _playerProperty.m_bulletSpeed = prevBulletSpeed;
        // _playerProperty.m_bulletDamage = prevBulletDamage;
        // _playerProperty.m_shootInterval = prevShotInterval;

        // _playerShot.m_homing = false;

        // // JIGlobalRef.MainCamera.GetComponent<ShockWaveEffect> ().enabled = false;
    }

}