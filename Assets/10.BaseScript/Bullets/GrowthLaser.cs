using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Sirenix.OdinInspector;

public class GrowthLaser : Laser
{
    public override LaserType LaserType => LaserType.Growth;

    public float GrowthSpeed;

    private List<Transform> _colliders = new List<Transform> ();

    private GrowthLaserCollisionDetecter _colDetecter;

    protected override void OnEnable ()
    {
        base.OnEnable ();

        if (_colDetecter != null) DestroyImmediate (_colDetecter);

        _colDetecter = (GetComponentInChildren<JIBulletCollider> ().gameObject)
            .AddComponent<GrowthLaserCollisionDetecter> ();
        _colDetecter.OnTriggerEnter += AddCollider;
        _colDetecter.OnTriggerExit += RemoveCollider;
    }

    private void OnDisable ()
    {
        _colliders.Clear ();

        if (_colDetecter != null)
        {
            _colDetecter.OnTriggerEnter -= AddCollider;
            _colDetecter.OnTriggerExit -= RemoveCollider;
            Destroy (_colDetecter);
        }
    }

    protected override void Update ()
    {
        base.Update ();

        if (_colliders.Count == 0)
        {
            LaserLength += JITimer.Instance.DeltTime * GrowthSpeed;
        }
        else
        {
            _colliders = new List<Transform> (_colliders.Where (
                x => x != null && x.gameObject.activeInHierarchy));

            foreach (var col in _colliders)
            {
                float length = col.position.y - transform.position.y;
                LaserLength = Mathf.Min (length, LaserLength);
            }

            if (LaserLength <= 0)
            {
                _colliders.Clear ();
                LaserLength = 0f;
            }
        }

        UpdateLaserAppear ();
    }

    private void AddCollider (Collider2D col)
    {
        if (col == null) return;
        if (col.CompareTag ("DestroyArea")) return;

        var colBullet = col.transform.GetComponentInParent<JIBulletProperty> ();
        if (colBullet != null)
        {
            return;
        }

        if (_bullet.IsPlayerBullet)
        {
            if (col.transform.parent?.GetComponent<PlayerProperty> () != null) return;

            _colliders.Add (col.transform);
            return;
        }
        else
        {
            if (col.GetComponent<EnemyProperty> () != null) return;

            _colliders.Add (col.transform);
            return;
        }
    }

    private void RemoveCollider (Collider2D col)
    {
        if (col == null) return;

        if (_colliders.Contains (col.transform)) _colliders.Remove (col.transform);
    }
}

internal class GrowthLaserCollisionDetecter : MonoBehaviour
{
    public Action<Collider2D> OnTriggerEnter;

    public Action<Collider2D> OnTriggerExit;

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (OnTriggerEnter != null) OnTriggerEnter (other);
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (OnTriggerExit != null) OnTriggerExit (other);
    }
}