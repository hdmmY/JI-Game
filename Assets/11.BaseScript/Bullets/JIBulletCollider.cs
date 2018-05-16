using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletColliderType
{
    Normal,
    Laser
}

[RequireComponent (typeof (Collider2D), typeof (Rigidbody2D))]
public class JIBulletCollider : MonoBehaviour
{
    public BulletColliderType ColliderType;

    #region Monobehaviors

    private void OnEnable ()
    {
        _bullet = transform.parent.GetComponent<JIBulletProperty> ();

        _laser = transform.parent.GetComponent<Laser> ();
        if (_laser != null)
        {
            if (_laser.LaserType == LaserType.Constant)
                _constLaser = _laser as ConstLaser;
            else
                _growthLaser = _laser as GrowthLaser;
        }

    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        switch (ColliderType)
        {
            case BulletColliderType.Normal:
                OnEnter_Normal (other);
                break;

            case BulletColliderType.Laser:
                if (_laser.LaserType == LaserType.Growth)
                {
                    OnEnter_GrowthLaser (other);
                }
                break;
        }
    }

    private void OnTriggerStay2D (Collider2D other)
    {
        switch (ColliderType)
        {
            case BulletColliderType.Laser:
                OnStay_Laser (other);
                break;
        }
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        switch (ColliderType)
        {
            case BulletColliderType.Laser:
                if (_laser.LaserType == LaserType.Growth)
                {
                    OnExit_GrowthLaser (other);
                }
                break;
        }
    }

    #endregion

    #region Private variables

    private JIBulletProperty _bullet;

    private Laser _laser;

    private GrowthLaser _growthLaser;

    private ConstLaser _constLaser;

    #endregion

    #region Private methods

    private void OnEnter_Normal (Collider2D other)
    {
        if (other.CompareTag ("DestroyArea")) return;

        EventManager.Instance.Raise (new BulletAttackEvent (_bullet, other.transform));
    }

    private void OnEnter_GrowthLaser (Collider2D other)
    {
        if (other.CompareTag ("DestroyArea")) return;

        _growthLaser.Colliders.Add (other.transform);
    }

    private void OnExit_GrowthLaser (Collider2D other)
    {
        if (other.CompareTag ("DestroyArea")) return;

        _growthLaser.Colliders.Remove (other.transform);
    }

    private void OnStay_Laser (Collider2D other)
    {
        if (other.CompareTag ("DestroyArea")) return;

        EventManager.Instance.Raise (new BulletAttackEvent (_bullet, other.transform));
    }

    #endregion
}