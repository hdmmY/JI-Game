using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour 
{
    // The bullet shot key in keyboard.
	public KeyCode m_ShotKey = KeyCode.Z;

    // The position of the bullet emit point.
    public List<Transform> m_shootList = new List<Transform>();

    public GameObject m_bulletPrefab;

    // Use this property to control damage and shot interval.
    private PlayerProperty _playerProperty;

    // Timer for count whether it is enough time after last shot.
    private float _timer;


    void OnEnable()
    {
        _playerProperty = GetComponent<PlayerProperty>();
    }


    void Update()
    {
        if(Input.GetKey(m_ShotKey))
        {
            Shot();
        }
        else
        {
            _timer = 0f;
        }
    }


    // Player wants to shot.
    void Shot()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        // it is not enough time after the last shot, shot cancel 
        if(_timer < _playerProperty.m_shootInterval)
            return;

        if(m_shootList == null || m_shootList.Count <= 0)
        {
            Debug.LogWarning("Cannot shot because ShotList is not set.");
            return;
        } 

        for(int i = 0; i < m_shootList.Count; i++)
        {
            UbhBullet bullet = GetBullet(m_shootList[i].position, Quaternion.identity);
            if(bullet == null)  break;
            bullet.Shot(_playerProperty.m_bulletSpeed, m_shootList[i].rotation.z, 
                    0, 0, 
                    false, null, 0, 0, 
                    false, 0, 0, 
                    false, 0, 0, 
                    UbhUtil.AXIS.X_AND_Y);
        }

        // finish a shot, reset timer
        _timer = 0f;
    }


    // Get a template bullet in the object pool.
    // position: bullet worldspace position.
    // rotation: bullet worldspace rotation.
    // forceInstantiate: force to instantiate a bullet in object pool and get it.
    UbhBullet GetBullet(Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        if(m_bulletPrefab == null)
        {
            return null;
        }

        // Get bullet gameobject from object pool
        var goBullet = UbhObjectPool.Instance.GetGameObject
                    (m_bulletPrefab, position, rotation, forceInstantiate);
        if(goBullet == null)
        {
            Debug.LogWarning("Fail to get the bullet from object pool!");
            return null;
        }    

        // Get or add UbhBullet component
        var bullet = goBullet.GetComponent<UbhBullet>();
        if(bullet == null)
        {
            bullet = goBullet.AddComponent<UbhBullet>();
        }

        return bullet;
    }

}
