using UnityEngine;
using System.Collections;

public class UbhExplosion : UbhMonoBehaviour
{
    void OnAnimationFinish ()
    {
        Destroy(gameObject);
    }
}
