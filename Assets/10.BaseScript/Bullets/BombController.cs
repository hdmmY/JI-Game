using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BombController : MonoBehaviour
{
    [Range (0.05f, 4f)]
    public float AttractFieldRadius;

    public float AttractForceFactor = 5f;

    public float SmoothFactor = 0.5f;

    [Range (0.05f, 1f)]
    public float DestroyFieldRaius;

    [Space]

    public GameObject AttractFieldRenderer;
    public GameObject DestroyFieldRenderer;
    public GameObject UnexplodeBombRenderer;

    #region MonoBehaviour

    private void OnEnable ()
    {
        _attractFieldDetecter = InstantiateDetecter ("AttractField Detecter", AttractFieldRadius);
        _attractFieldDetecter.OnTriggerStayEvent += AttractBullet;

        _destroyFieldDetecter = InstantiateDetecter ("DestroyField Detecter", DestroyFieldRaius);
        _destroyFieldDetecter.OnTriggerStayEvent += DestroyBulletAndEnemy;
    }

    private void OnDisable ()
    {

        if (_attractFieldDetecter?.gameObject != null)
        {
            _attractFieldDetecter.OnTriggerStayEvent -= AttractBullet;
            Destroy (_attractFieldDetecter);
        }

        if (_destroyFieldDetecter?.gameObject != null)
        {
            _destroyFieldDetecter.OnTriggerStayEvent -= DestroyBulletAndEnemy;
            Destroy (_destroyFieldDetecter);
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (transform.position, AttractFieldRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, DestroyFieldRaius);
    }

    #endregion

    #region Private Variable and Method

    private BombCollisionDetecter _attractFieldDetecter;
    private BombCollisionDetecter _destroyFieldDetecter;

    private BombCollisionDetecter InstantiateDetecter (string name, float detectRadius)
    {
        var detecter = new GameObject (name);
        detecter.transform.parent = transform.GetChild (0);
        detecter.transform.position = transform.position;
        detecter.layer = transform.gameObject.layer;

        var detecterCol = detecter.AddComponent<CircleCollider2D> ();
        detecterCol.radius = detectRadius;
        detecterCol.isTrigger = true;

        return detecter.AddComponent<BombCollisionDetecter> ();
    }

    private void AttractBullet (Collider2D col)
    {
        var bullet = col.transform.parent?.GetComponent<JIBulletProperty> ();
        if (bullet == null || bullet.IsPlayerBullet) return;

        var bulletMove = bullet.GetComponent<JIBulletMovement> ();
        if (bulletMove == null) return;

        var attractMoveCtrl = bulletMove.GetComponent<AttractBulletMoveCtrl> ();
        if (attractMoveCtrl == null)
        {
            BulletUtils.ClearAllMoveCtrl (bullet.gameObject);
            attractMoveCtrl = bulletMove.gameObject.AddComponent<AttractBulletMoveCtrl> ();
            attractMoveCtrl.AttractFactor = AttractForceFactor;
            attractMoveCtrl.SmoothFactor = SmoothFactor;
            attractMoveCtrl.GravityCenter = transform;
        }
    }

    private void DestroyBulletAndEnemy (Collider2D col)
    {
        var bullet = col.transform.parent?.GetComponent<JIBulletProperty> ();

        var enemy = col.GetComponent<EnemyProperty> ();

        if (bullet != null && !bullet.IsPlayerBullet)
        {
            BulletPool.Instance.ReleaseGameObject (bullet.gameObject);
            return;
        }

        if (enemy != null)
        {
            if (!enemy.m_elite)
            {
                Destroy (enemy.gameObject);
            }
            return;
        }
    }

    #endregion
}

internal class BombCollisionDetecter : MonoBehaviour
{
    public Action<Collider2D> OnTriggerEnterEvent;

    public Action<Collider2D> OnTriggerStayEvent;

    public Action<Collider2D> OnTriggerExitEvent;

    private void OnEnable ()
    {
        OnTriggerEnterEvent = null;
        OnTriggerStayEvent = null;
        OnTriggerExitEvent = null;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (OnTriggerEnterEvent != null) OnTriggerEnterEvent (other);
    }

    private void OnTriggerStay2D (Collider2D other)
    {
        if (OnTriggerStayEvent != null) OnTriggerStayEvent (other);
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (OnTriggerExitEvent != null) OnTriggerExitEvent (other);
    }

}