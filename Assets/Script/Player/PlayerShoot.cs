using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerProperty))]
[RequireComponent(typeof(PlayerReference))]
public class PlayerShoot : MonoBehaviour
{
    private PlayerReference _playerRefer;
    private PlayerProperty _playerProperty;

    private InputManager _InputManager;
    private BulletPool _BulletPool;

    private Bullet_Property _TempleBlackBulletProperty;
    private Bullet_Property _TempleWhiteBulletProperty;
    private Bullet_Property _curTempleBulletProperty;


    private Transform _leftShootTras;
    private Transform _rightShootTras;

    private float _shootInterval;
    private float _bulletSpeed;
    private int _bulletDamage;

    private float _freTimer;    // timer for count frequency


    private void OnEnable()
    {
        SetInitReference();

        _freTimer = 0f;

        _shootInterval = _playerProperty.m_shootInterval;
        _bulletSpeed = _curTempleBulletProperty.m_BulletSpeed;


        // event 
        PlayerProperty.OnPlayerStateChangeEvent += ChangeBulletProperty;
    }


    private void OnDisable()
    {
        PlayerProperty.OnPlayerStateChangeEvent -= ChangeBulletProperty;
    }



    private void Update()
    {
        UpdateShootProperty();

        if (_InputManager.m_Shoot)
        {
            if (_freTimer >= _shootInterval)
            {
                Shoot();
            }
        }

        _freTimer += Time.deltaTime;
    }


    private void UpdateShootProperty()
    {
        _shootInterval = _playerProperty.m_shootInterval;
        _bulletSpeed = _curTempleBulletProperty.m_BulletSpeed;
        _bulletDamage = _curTempleBulletProperty.m_BulletDamage;
    }



    private void Shoot()
    {
        _freTimer = 0f;

        //Vector3 leftPoint = _leftShootPos.TransformPoint(_leftShootPos.localPosition);
        //Vector3 rightPoint = _rightShootPos.TransformPoint(_rightShootPos.localPosition); 

        GameObject leftBullet = _BulletPool.create(_leftShootTras.position);
        GameObject rightBullet = _BulletPool.create(_rightShootTras.position);

        leftBullet.GetComponent<BulletReference>().m_BulletPool = _BulletPool;
        rightBullet.GetComponent<BulletReference>().m_BulletPool = _BulletPool;

        leftBullet.GetComponent<Bullet_Controller>().m_InitAngle = 90f;
        rightBullet.GetComponent<Bullet_Controller>().m_InitAngle = 90f;

        Bullet_Property leftProperty = leftBullet.GetComponent<Bullet_Property>();
        Bullet_Property rightProperty = rightBullet.GetComponent<Bullet_Property>();

        leftProperty.CopyProperty(_curTempleBulletProperty);
        rightProperty.CopyProperty(_curTempleBulletProperty);

        rightProperty.m_CurTime = leftProperty.m_CurTime = 0f;
        rightProperty.m_BulletSpeed = leftProperty.m_BulletSpeed = _bulletSpeed;
        rightProperty.m_BulletDamage = leftProperty.m_BulletDamage = _bulletDamage;

        PlayerEventMaster.CallPlayerShootEvent(leftBullet);
        PlayerEventMaster.CallPlayerShootEvent(rightBullet);

        return;
    }


    void SetInitReference()
    {
        _playerRefer = GetComponent<PlayerReference>();
        _playerProperty = GetComponent<PlayerProperty>();

        _InputManager = _playerRefer.m_InputManager;
        _BulletPool = _playerRefer.m_BulletPool;

        _leftShootTras = FindShootPoint(_playerProperty.m_leftShootPoint);
        _rightShootTras = FindShootPoint(_playerProperty.m_rightShootPoint);

        _TempleBlackBulletProperty = _playerRefer.m_BlackBulletProperty;
        _TempleWhiteBulletProperty = _playerRefer.m_WhiteBulletProperty;
        switch (_playerProperty.m_playerState)
        {
            case PlayerProperty.PlayerStateType.Black:
                _curTempleBulletProperty = _TempleBlackBulletProperty;
                break;
            case PlayerProperty.PlayerStateType.White:
                _curTempleBulletProperty = _TempleWhiteBulletProperty;
                break;
        }
    }


    void ChangeBulletProperty(PlayerProperty.PlayerStateType prevState)
    {
        switch (prevState)
        {
            case PlayerProperty.PlayerStateType.Black:
                _curTempleBulletProperty = _TempleWhiteBulletProperty;
                break;
            case PlayerProperty.PlayerStateType.White:
                _curTempleBulletProperty = _TempleBlackBulletProperty;
                break;
        }
    }


    Transform FindShootPoint(string shootPointName)
    {
        foreach (ShootPoint shootPoint in _playerRefer.m_ShootTransform)
        {
            if (shootPointName == shootPoint.name)
            {
                return shootPoint.transform;
            }
        }

        return null;
    }




}
