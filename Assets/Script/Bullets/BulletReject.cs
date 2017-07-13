using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReject : MonoBehaviour {

    public Transform m_TargetTrans;

    public float m_factor;

    private float _sqrDistance;
    private float _initialXVelocity;
    private float _deltXVelocity;

    private Rigidbody2D _rb2D;

    private void OnEnable()
    {
        _rb2D = GetComponent<Rigidbody2D>();

        _initialXVelocity = GetComponent<Rigidbody2D>().velocity.x;
        _deltXVelocity = 0f;
    }


    private void Update()
    {
        Vector3 deltPosition = transform.position - m_TargetTrans.position;

        _initialXVelocity = _rb2D.velocity.x - _deltXVelocity;

        _sqrDistance = deltPosition.sqrMagnitude;

        _deltXVelocity = m_factor / (_sqrDistance + 1f);
        _deltXVelocity = deltPosition.x > 0 ? _deltXVelocity : -_deltXVelocity;

        _rb2D.velocity = new Vector2(_initialXVelocity + _deltXVelocity, _rb2D.velocity.y);
    }


}
