using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletUtils
{
    /// <summary>
    /// Get bullet from object pool.
    /// </summary>                        
    /// <param name="forceInstantiate"> force the pool to return an instantiate bullet. </param>
    /// <returns></returns>
    public static JIBulletController GetBullet (GameObject bullet, Transform parent)
    {
        Vector3 defaultPos = parent == null ? bullet.transform.position : parent.position;

        return GetBullet (bullet, parent, defaultPos, Quaternion.identity, false);
    }

    /// <summary>
    /// Get bullet from object pool.
    /// </summary>                        
    /// <param name="forceInstantiate"> force the pool to return an instantiate bullet. </param>
    /// <returns></returns>
    public static JIBulletController GetBullet (GameObject bullet, Transform parent,
        Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        if (bullet == null) return null;

        var reBullet = BulletPool.Instance.GetGameObject (bullet, position, rotation, forceInstantiate);
        if (reBullet == null) return null;

        /* Legency code. Will remove later */
        var controller = reBullet.GetComponent<JIBulletController> ();
        if (controller == null) controller = reBullet.AddComponent<JIBulletController> ();
        /* ******************************* */

        var property = reBullet.GetComponent<JIBulletProperty> ();
        if (property == null) property = reBullet.AddComponent<JIBulletProperty> ();

        var bulletMove = reBullet.GetComponent<JIBulletMovement> ();
        if (bulletMove == null) bulletMove = reBullet.AddComponent<JIBulletMovement> ();

        if (parent != null) reBullet.transform.parent = parent;

        return controller;
    }
}