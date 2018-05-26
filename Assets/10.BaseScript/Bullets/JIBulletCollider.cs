using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider2D), typeof (Rigidbody2D))]
public class JIBulletCollider : MonoBehaviour
{
    #region Monobehaviors

    private void OnEnable ()
    {
        _bullet = transform.parent.GetComponent<JIBulletProperty> ();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        RaiseCollisionEvent (other);
    }

    private void OnTriggerStay2D (Collider2D other)
    {
        if (_bullet.Type == JIBulletType.Laser)
            RaiseCollisionEvent (other);
    }

    #endregion

    #region Private variables

    private JIBulletProperty _bullet;

    #endregion

    #region Private methods

    private void RaiseCollisionEvent (Collider2D other)
    {
        EnemyProperty enemy = other.transform.GetComponent<EnemyProperty> ();
        if (enemy != null)
        {
            EventManager.Instance.Raise (new BulletAttackEvent (
                _bullet, new JIColliderUnit (enemy)));
            return;
        }

        PlayerProperty player = other.transform.parent?.GetComponent<PlayerProperty> ();
        if (player != null)
        {
            EventManager.Instance.Raise (new BulletAttackEvent (
                _bullet, new JIColliderUnit (player)));
            return;
        }

        JIBulletProperty bullet = other.transform.parent?.GetComponent<JIBulletProperty> ();
        if (bullet != null)
        {
            if (bullet.IsPlayerBullet)
            {
                EventManager.Instance.Raise (new BulletAttackEvent (
                    _bullet, new JIColliderUnit (bullet, JIColliderUnitType.PlayerBullet)));
            }
            else
            {
                EventManager.Instance.Raise (new BulletAttackEvent (
                    _bullet, new JIColliderUnit (bullet, JIColliderUnitType.EnemyBullet)));
            }
            return;
        }
    }

    #endregion
}