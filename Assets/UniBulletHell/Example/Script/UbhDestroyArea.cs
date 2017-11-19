using UnityEngine;
using System.Collections;

public class UbhDestroyArea : UbhMonoBehaviour
{                                
    // Destroy all bullet. 
    // distinguished by tag
    void OnTriggerEnter2D(Collider2D other)
    {
        string goTag = other.tag;

        if (goTag.Contains("Bullet"))
        {
            UbhObjectPool.Instance.ReleaseGameObject(other.transform.parent.gameObject);
            return;
        }
    }
}