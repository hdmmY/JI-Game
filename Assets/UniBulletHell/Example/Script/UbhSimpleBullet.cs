using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class UbhSimpleBullet : UbhMonoBehaviour
{
    public int _Power = 1;
    [SerializeField]
    float _Speed = 10;

    void OnEnable ()
    {
        rigidbody2D.velocity = transform.up.normalized * _Speed;
    }
}