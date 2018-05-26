using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider2D))]
public class JIDestroyArea : UbhMonoBehaviour
{
    void OnTriggerEnter2D (Collider2D c)
    {
        HitCheck (c.transform);
    }

    private void OnTriggerStay2D (Collider2D c)
    {
        HitCheck (c.transform);
    }

    // Destroy all bullet. 
    static void HitCheck (Transform colTrans)
    {
        var bullet = colTrans.parent.GetComponent<JIBulletProperty> ();

        if (bullet == null) return;

        if (bullet.Type == JIBulletType.Normal)
        {
            BulletPool.Instance.ReleaseGameObject (bullet.gameObject);
        }
    }
}