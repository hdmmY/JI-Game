using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletReference))]
public class Bullet_Controller : MonoBehaviour
{
    public bool m_updateBulletProperty;

    public GameObject m_BulletPicture;
    
    [HideInInspector]
    public float m_InitAngle;

    private BulletPool _BulletPool;
    private Bullet_Property _propertyScript;
    private Material _bulletPictureMaterial;
    private Transform _bulletPictureTrans;

    private Vector2 _velocity;
    private float _velocityAngle;

    private float _lifeTimer;
    private float _acceTimer;  // The acceleration timer

    #region Bullet Property -- Private

    float _lifeTime_Property;
    Color _bulletColor_Property;
    float _Alpha_Property;
    int _spriteDir_Property;
    bool _alignWithVelocity_Property;
    float _bulletSpeed_Property;
    float _Accelerate_Property;
    int _AccelerateDir_Property;
    float _XVelocityFactor_Property;
    float _YVelocityFactor_Property;

    #endregion


    private void OnEnable()
    {
        SetInitReference();

        _lifeTimer = 0f;
        _acceTimer = 0f;
        _velocity = new Vector2();

        UpdateBulletProperty();

        //SetBulletPictureProperty();
    }


    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        _acceTimer += Time.deltaTime;

        if (_lifeTimer >= _lifeTime_Property)
        {
            ReachLifeTime();
        }

        UpdateVelocity();
        transform.position += (Vector3)_velocity * Time.deltaTime;

        if (m_updateBulletProperty)
        {
            if (_Accelerate_Property != _propertyScript.m_Accelerate)
                _acceTimer = 0f;

            UpdateBulletProperty();
            SetBulletPictureProperty();          
        }

    }

    void UpdateVelocity()
    {
        Vector2 velocityDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * m_InitAngle),
                                        Mathf.Sin(Mathf.Deg2Rad * m_InitAngle));
        Vector2 accelDir = new Vector2(
            Mathf.Cos(Mathf.Deg2Rad * (m_InitAngle + _AccelerateDir_Property)),
            Mathf.Sin(Mathf.Deg2Rad * m_InitAngle + _AccelerateDir_Property));

        _velocity = velocityDir * _bulletSpeed_Property;
        _velocity += _acceTimer * _Accelerate_Property * accelDir;

        _velocity.x *= _XVelocityFactor_Property;
        _velocity.y *= _YVelocityFactor_Property;

        float angle = CountVelocityAngle();
        if(Mathf.Abs(angle - _velocityAngle) >= 0.05)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        _velocityAngle = angle;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        return;
    }


    /// <summary>
    /// Update the bullet property from the _propertyScript;
    /// </summary>
    void UpdateBulletProperty()
    {
        _lifeTime_Property = _propertyScript.m_LifeTime;
        _bulletColor_Property = _propertyScript.m_BulletColor;
        _Alpha_Property = _propertyScript.m_Alpha;
        _spriteDir_Property = _propertyScript.m_SpriteDirection;
        _alignWithVelocity_Property = _propertyScript.m_AlignWithVelocity;
        _bulletSpeed_Property = _propertyScript.m_BulletSpeed;
        _AccelerateDir_Property = _propertyScript.m_AcceleratDir;
        _Accelerate_Property = _propertyScript.m_Accelerate;
        _XVelocityFactor_Property = _propertyScript.m_HorizontalVelocityFactor;
        _YVelocityFactor_Property = _propertyScript.m_VerticalVelocityFactor;
    }


    void SetBulletPictureProperty()
    {
        _bulletColor_Property.a = _Alpha_Property;
        _bulletPictureMaterial.color = _bulletColor_Property;

        if (!_alignWithVelocity_Property)
        {
            _bulletPictureTrans.rotation = Quaternion.Euler(0, 0, _spriteDir_Property);
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
        _propertyScript = GetComponent<BulletReference>().m_BulletProperty;

        if (m_BulletPicture == null)
        {
            Debug.LogError("The m_BulletPicture is not assigned!");
            return;
        }

        _BulletPool = GetComponent<BulletReference>().m_BulletPool;
        if (_BulletPool == null)
        {
            Debug.LogError("The m_BulletPool is not Init");
            return;
        }                   

        _bulletPictureMaterial = m_BulletPicture.GetComponent<MeshRenderer>().material;
        _bulletPictureTrans = m_BulletPicture.GetComponent<Transform>();
    }

}
