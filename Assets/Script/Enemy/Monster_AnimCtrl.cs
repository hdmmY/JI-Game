using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_AnimCtrl : MonoBehaviour 
{
    // The target that monster will look at.
    public Vector2 m_MonsterTarget;

    // Monster's animtor component
    private Animator _animtor;

    // The monster's rotate angle, use to determine the animation.
    private float _angle;

    // The last monster's position.
    private Vector3 _prePosition;
    // The last distance between target and this.transform.
    private float _preDistanceToTarget;


    // Make monster look to target
    void LateUpdate()
    {
        float curDistance = (transform.position - (Vector3)m_MonsterTarget).magnitude;

        if(curDistance < _preDistanceToTarget)
        {
            // The monster is moving to target, so the angle is from prev position to cur position.
             _angle = UbhUtil.GetAngleFromTwoPosition(_prePosition, transform.position);
        }
        else
        {
            // The monster is going away from target, so the angle is from cur position to prev position.
             _angle = UbhUtil.GetAngleFromTwoPosition(transform.position, _prePosition);
        }
        _angle = UbhUtil.Get360Angle(_angle + 90f);  // I don't know why, it works.



        // Update pre property.
        _prePosition = transform.position;
        _preDistanceToTarget = curDistance;

        // anim
        if(_angle < 85 || _angle > 275)
            _animtor.SetBool("look right", true);
        else
            _animtor.SetBool("look right", false);

        if(_angle > 95 && _angle <265)
            _animtor.SetBool("look left", true);
        else
            _animtor.SetBool("look left", false);

        // Debug
        float rad = _angle * Mathf.Deg2Rad;
        Debug.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * 3);
    }


    // Init
	private void OnEnable()
    {
        _animtor = GetComponent<Animator>();

        _prePosition = transform.position;

        // Since we just need to compare prev distance and curr distance,
        // so we don't need to calcualte the sqrmagnitude.
        _preDistanceToTarget = (transform.position - (Vector3)m_MonsterTarget).magnitude;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawCube(m_MonsterTarget, Vector3.one * 0.2f);
    }
}
