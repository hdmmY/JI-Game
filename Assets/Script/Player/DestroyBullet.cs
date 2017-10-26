using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string name = collision.name.ToLower();

        if(name.Contains("bullet"))
        {
            UbhObjectPool.Instance.ReleaseGameObject(collision.transform.parent.gameObject);
        }
    }
}
