using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletReference))]
public class Bullet_Controller : MonoBehaviour
{
    public bool m_updateBulletProperty;

    private GameObject _BulletSprite;

    [HideInInspector]
    public float m_InitAngle;

    private BulletPool _BulletPool;
    private Bullet_Property _propertyScript;
    private BulletEventMaster _bulletEventMaster;

    private SpriteRenderer _bulletSpriteRender;

    private Transform _bulletPictureTrans;

    private Vector2 _velocity;
    private float _velocityAngle;

    private float _lifeTimer;
    private float _acceTimer;  // The acceleration timer

    private bool _updateVelocityOnce;  // the mark for init velocity 



    private void OnEnable()
    {
        SetInitReference();

        _lifeTimer = 0f;
        _acceTimer = 0f;
        _velocity = Vector2.zero;
        _velocityAngle = 0f;
        _updateVelocityOnce = true;

        _bulletEventMaster.BulletPropertyInitEvent += UpdateVelocityProperty;
    }

    private void OnDisable()
    {
        _bulletEventMaster.BulletPropertyInitEvent -= UpdateVelocityProperty;
    }


    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        _acceTimer += Time.deltaTime;

        if (_lifeTimer >= _propertyScript.m_LifeTime)
        {
            ReachLifeTime();
        }


        // init velocity
        //if (_updateVelocityOnce)
        //{
        //    UpdateVelocity();
        //    _updateVelocityOnce = false;
        //}

        //// update velocity each frame
        //else
        //{
        //    if(_propertyScript.m_useBulletAttrack || _propertyScript.m_useBulletReject)
        //        _velocity = _propertyScript.m_Velocity;
        //    else
        //    {
        //        UpdateVelocity();
        //    }
        //}

        UpdateVelocity();

        transform.localRotation = Quaternion.Euler(0, 0, _velocityAngle);
        transform.localPosition += (Vector3)_velocity * Time.deltaTime;
        _propertyScript.m_Velocity = _velocity;

        if (m_updateBulletProperty)
        {
            //if (_propertyScript.m_Accelerate != _propertyScript.m_Accelerate)
            //    _acceTimer = 0f;

            UpdateBulletPictureProperty();
        }

    }



    void UpdateVelocity()
    {
        float velocityDegree = Mathf.Deg2Rad * m_InitAngle;
        float accleDegree = Mathf.Deg2Rad * _propertyScript.m_AcceleratDir;

        Vector2 velocityDir = new Vector2(Mathf.Cos(velocityDegree), Mathf.Sin(velocityDegree));

        Vector2 accelDir = (_propertyScript.m_AcceleratDir == 0) ? Vector2.zero : new Vector2(
            Mathf.Cos(velocityDegree + accleDegree),
            Mathf.Sin(velocityDegree + accleDegree));

        _velocity = velocityDir * _propertyScript.m_BulletSpeed;
        _velocity += _acceTimer * _propertyScript.m_Accelerate * accelDir;

        _velocity.x *= _propertyScript.m_HorizontalVelocityFactor;
        _velocity.y *= _propertyScript.m_VerticalVelocityFactor;


        float angle = CountVelocityAngle();
        _velocityAngle = angle;

        return;
    }

    void UpdateBulletPictureProperty()
    {
        Color newColor = _propertyScript.m_BulletColor;
        newColor.a = _propertyScript.m_Alpha;
        _bulletSpriteRender.color = newColor;

        if (!_propertyScript.m_AlignWithVelocity)
        {
            _bulletPictureTrans.rotation = Quaternion.Euler(0, 0, _propertyScript.m_SpriteDirection);
        }
        else
        {
            _bulletPictureTrans.localRotation = Quaternion.identity;
        }
    }


    /// <summary>
    /// it is called when the bullet reach the life time
    /// </summary>
    void ReachLifeTime()
    {
        _BulletPool.delete(this.gameObject);
    }


    /// <summary>
    /// Count the velocity angle.
    /// </summary>
    /// <returns></returns>
    float CountVelocityAngle()
    {
        float degree = Mathf.Acos(_velocity.normalized.x) * Mathf.Rad2Deg;

        if (_velocity.x > 0)
        {
            if (_velocity.y > 0)      // x > 0 && y > 0
            {
                return degree;
            }
            else                      // x > 0 && y < 0
            {
                return -degree;
            }
        }
        else if (_velocity.x == 0)
        {
            if (_velocity.y > 0)
            {
                return -90;
            }

            return 90;
        }
        else
        {
            if (_velocity.y > 0)       // x < 0 && y > 0
            {
                return degree;
            }
            else                      // x < 0 && y < 0
            {
                return -degree;
            }
        }
    }


    void SetInitReference()
    {
        _propertyScript = GetComponent<Bullet_Property>();
        _bulletEventMaster = GetComponent<BulletEventMaster>();

        _BulletPool = GetComponent<BulletReference>().m_BulletPool;
        if (_BulletPool == null)
        {
            Debug.LogError("The m_BulletPool is not Init");
            return;
        }

        _BulletSprite = GetComponent<BulletReference>().m_BulletSprite;

        _bulletPictureTrans = _BulletSprite.GetComponent<Transform>();
        _bulletSpriteRender = _BulletSprite.GetComponent<SpriteRenderer>();
    }


    void UpdateVelocityProperty(Bullet_Property initBulletProperty)
    {
        _propertyScript.m_AcceleratDir = initBulletProperty.m_AcceleratDir;
        _propertyScript.m_Accelerate = initBulletProperty.m_Accelerate;

        _propertyScript.m_BulletSpeed = initBulletProperty.m_BulletSpeed;

        _propertyScript.m_HorizontalVelocityFactor = initBulletProperty.m_HorizontalVelocityFactor;
        _propertyScript.m_VerticalVelocityFactor = initBulletProperty.m_VerticalVelocityFactor;
    }


}
