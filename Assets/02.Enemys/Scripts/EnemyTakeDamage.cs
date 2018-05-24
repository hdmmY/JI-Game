using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (EnemyProperty))]
public class EnemyTakeDamage : MonoBehaviour
{
    private void Awake ()
    {
#if UNITY_EDITOR
        DestroyImmediate (this);
#else 
        Destroy (this);
#endif
    }
}