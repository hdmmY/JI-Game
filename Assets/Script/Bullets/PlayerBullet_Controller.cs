using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet_Controller : MonoBehaviour
{
    private Transform _bulletTransform;

    private float _bulletVelocity;

    static float _defaultBulletVelocity = 5f;


    void OnEnable()
    {
        SetInitReference();
    }


    private void Update()
    {
        _bulletTransform.position += _bulletVelocity * Vector3.up * Time.deltaTime;
    }



    /// <summary>
    /// set the bullet velocity
    /// </summary>
    /// <param name="bulletVelocity"> target velocity </param>
    /// <returns> real bullet velocity </returns>
    public float SetBulletVelocity(float bulletVelocity)
    {
        if (bulletVelocity < 0)
        {
            _bulletVelocity = _defaultBulletVelocity;
        }
        else
        {
            _bulletVelocity = bulletVelocity;
        }

        return _bulletVelocity;
    }



    void SetInitReference()
    {
        _bulletTransform = GetComponent<Transform>();
    }
}
