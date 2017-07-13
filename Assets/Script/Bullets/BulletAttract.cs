using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttract : MonoBehaviour
{                          
    public Transform m_TargetTrans;

    private Rigidbody2D _rb2D;
    private bool _canAttract;

    private void OnEnable()
    {
        _rb2D = GetComponent<Rigidbody2D>();

        float distance = (m_TargetTrans.position - transform.position).sqrMagnitude;
        if (distance <= 0.1f)
        {
            _canAttract = false;
        }
        else
        {
            _canAttract = true;
        }
    }

    private void Update()
    {
        if (_canAttract)
        {
            Attract();
        }
    }


    private void Attract()
    {
        Vector3 dir = m_TargetTrans.position - transform.position;
        float time = dir.y / _rb2D.velocity.y;   
        _rb2D.velocity = new Vector2(dir.x / time, _rb2D.velocity.y);
    }
}
