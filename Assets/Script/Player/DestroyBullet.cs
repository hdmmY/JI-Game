using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag.ToLower();

        if(tag.Contains("enemy") && tag.Contains("bullet"))
        {
            UbhObjectPool.Instance.ReleaseGameObject(collision.transform.parent.gameObject);
        }
    }
}
