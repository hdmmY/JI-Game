using UnityEngine;
using System.Collections;

public class UbhDestroyArea : UbhMonoBehaviour
{                        
    void OnTriggerEnter2D(Collider2D c)
    {   
        HitCheck(c.transform);
    }

    void OnTriggerEnter(Collider c)
    {         
        HitCheck(c.transform);
    }                            

    // Destroy all bullet. 
    // distinguished by tag
    void HitCheck(Transform colTrans)
    {
        string goTag = colTrans.tag;

        if (goTag == "EnemyBullet")
        {
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.parent.gameObject);
            return;
        }
        else if(goTag == "PlayerBullet")
        {
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.parent.gameObject);
            return;
        }
    }
}