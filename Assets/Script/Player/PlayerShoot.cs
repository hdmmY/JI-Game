using UnityEngine;


[RequireComponent(typeof(PlayerProperty))]
[RequireComponent(typeof(PlayerReference))]
public class PlayerShoot : MonoBehaviour
{
    public KeyCode m_ShotKey;

    private PlayerReference _playerRefer;
    private PlayerProperty _playerProperty;
    private PlayerEventMaster _playerEventMaster;

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
        _playerEventMaster.OnPlayerStateChangeEvent += ChangeBulletProperty;
    }


    private void OnDisable()
    {
        _playerEventMaster.OnPlayerStateChangeEvent -= ChangeBulletProperty;
    }



    private void Update()
    {
        UpdateShootProperty();

        if (Input.GetKey(m_ShotKey))
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

        GameObject leftBullet = _BulletPool.create(_leftShootTras.position);        // create bullet
        GameObject rightBullet = _BulletPool.create(_rightShootTras.position);

        Bullet_Property leftProperty = leftBullet.GetComponent<Bullet_Property>();   // set the bullet property
        Bullet_Property rightProperty = rightBullet.GetComponent<Bullet_Property>();
        leftProperty.CopyProperty(_curTempleBulletProperty);
        rightProperty.CopyProperty(_curTempleBulletProperty);
        rightProperty.m_CurTime = leftProperty.m_CurTime = 0f;
        rightProperty.m_BulletSpeed = leftProperty.m_BulletSpeed = _bulletSpeed;
        rightProperty.m_BulletDamage = leftProperty.m_BulletDamage = _bulletDamage;

        leftBullet.GetComponent<BulletReference>().m_BulletPool = _BulletPool;      // set the bullet pool reference
        rightBullet.GetComponent<BulletReference>().m_BulletPool = _BulletPool;

        leftBullet.GetComponent<Bullet_Controller>().m_InitAngle = 90f;             // init bullet controller
        rightBullet.GetComponent<Bullet_Controller>().m_InitAngle = 90f;

        rightBullet.layer = leftBullet.layer = LayerMask.NameToLayer(_playerProperty.m_PlayerBulletLayer); // set the layer of the bullet

        rightBullet.SetActive(true);
        leftBullet.SetActive(false);

        _playerEventMaster.CallPlayerShootEvent(leftBullet);                         // call bullet shoot event
        _playerEventMaster.CallPlayerShootEvent(rightBullet);

        return;
    }


    void SetInitReference()
    {
        _playerRefer = GetComponent<PlayerReference>();
        _playerProperty = GetComponent<PlayerProperty>();
        _playerEventMaster = GetComponent<PlayerEventMaster>();

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
