using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletReference))]
public class BulletDestoryOutBound : MonoBehaviour {

    private BulletPool _bulletPool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Edge"))
        {
            _bulletPool.delete(this.gameObject);
        }
    }


    private void OnEnable()
    {
        _bulletPool = GetComponent<BulletReference>().m_BulletPool;

        if (_bulletPool == null)
            Debug.LogError("The bulletPool is not init!");

    }

}
