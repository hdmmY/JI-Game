using UnityEngine;
using System.Collections;

/// <summary>
/// Unity4.3 "2d kinematic and trigger bug" counterplan.
/// http://answers.unity3d.com/questions/575438/how-to-make-ontriggerenter2d-work.html
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class UbhNonKinematic2D : UbhMonoBehaviour
{
#if UNITY_4_3
    Vector3 _FixedPosition = Vector3.zero;
    float _OrgGravityScale;
#endif

    void Awake ()
    {
#if !UNITY_4_3
        rigidbody2D.isKinematic = true;
        enabled = false;
        return;
#else
        _FixedPosition = transform.localPosition;
#endif
    }

    void Update ()
    {
#if !UNITY_4_3
        enabled = false;
        return;
#else
        if (rigidbody2D == null) {
            return;
        }
        rigidbody2D.velocity = Vector3.zero;
        rigidbody2D.angularVelocity = 0;
        transform.localPosition = _FixedPosition;
#endif
    }

    void OnEnable ()
    {
#if !UNITY_4_3
        enabled = false;
        return;
#else
        if (rigidbody2D == null) {
            return;
        }
        _OrgGravityScale = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 0f;
#endif
    }

    void OnDisable ()
    {
#if !UNITY_4_3
        return;
#else
        if (rigidbody2D == null) {
            return;
        }
        rigidbody2D.gravityScale = _OrgGravityScale;
#endif
    }
}
