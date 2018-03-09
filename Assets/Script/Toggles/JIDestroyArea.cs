using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider2D))]
public class JIDestroyArea : UbhMonoBehaviour
{
    public JIState m_destroyBulletType;

    void OnTriggerEnter2D(Collider2D c)
    {
        HitCheck(c.transform);
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        HitCheck(c.transform);
    }

    // Destroy all bullet. Distinguished by tag
    void HitCheck(Transform colTrans)
    {
        string goTag = colTrans.tag.ToLower();

        if(goTag.Contains("bullet"))
        {
            if(CheckBulletType(colTrans.parent.name))
            {
                UbhObjectPool.Instance.ReleaseGameObject(colTrans.parent.gameObject);
            }
        } 
    }          

    // Check whether a bullet can be destroy by this destroy area.
    private bool CheckBulletType(string bulletName)
    {
        bulletName = bulletName.ToLower();

        if ((m_destroyBulletType & JIState.Black) == JIState.Black)
        {
            if (bulletName.Contains("black")) return true;
        }

        if ((m_destroyBulletType & JIState.White) == JIState.White)
        {
            if (bulletName.Contains("white")) return true;
        }
        if (m_destroyBulletType == JIState.All)
            return true;

        return false;
    }
}