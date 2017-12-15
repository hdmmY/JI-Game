using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyState : BaseEnemyState
{
    // Enemy move speed
    public float m_moveSpeed;

    // Move bound
    public Transform m_bound;

    // The enemy box2D collider bound
    private Bounds _colBound;

    // Destination you want to move to
    private Vector3 _destination;

    // Move direction, normalized
    private Vector3 _moveDir;


    public override void Initialize(Enemy_Property enemyProperty)
    {
        base.Initialize(enemyProperty);

        var boxCol = enemyProperty.GetComponent<BoxCollider2D>();
        if (boxCol == null)
        {
            Debug.LogWarning("Cannot get enemy box collider");
            return;
        }
        _colBound = boxCol.bounds;

        // Initialize destination
        _destination = FindNextPosition();
        _moveDir = _destination.normalized;
    }


    public override void UpdateState(Enemy_Property enemyProperty)
    {
        base.UpdateState(enemyProperty);

        Vector3 nextPosition = enemyProperty.transform.position + _moveDir * m_moveSpeed * UbhTimer.Instance.DeltaTime;

        //Debug.Log("next position : " + nextPosition);

        if (Vector3.Dot(nextPosition - _destination, _moveDir) > 0)      // move overhead, end move
        {
            Debug.Log("Find next position!");
            nextPosition = _destination;

            _destination = FindNextPosition();
            _moveDir = (_destination - nextPosition).normalized;
        }

        enemyProperty.transform.position = nextPosition;
        Debug.Log(string.Format("enemy pos : {0}, next position : {1}", enemyProperty.transform.position, nextPosition));
    }


    // Find the next position that enemy will move to
    private Vector3 FindNextPosition()
    {
        // (min, max) bound Y
        Vector2 boundY = new Vector2(m_bound.localScale.y, m_bound.localScale.y);
        boundY.x = m_bound.position.y + (0.5f - 0.67f) * boundY.x;
        boundY.y = m_bound.position.y + 0.5f * boundY.y;

        // (min, max) bound X
        Vector2 boundX = m_bound.position;
        boundX.x -= m_bound.localScale.x * 0.5f;
        boundX.y += m_bound.localScale.x * 0.5f;

        // Range that enemy can't move
        Vector2 forbidenX = JIGlobalRef.Player.transform.position;
        forbidenX.x -= _colBound.size.x * 0.5f;

        Vector3 dest = new Vector3();
        while(true)
        {
            dest.x = Random.Range(boundX.x, boundX.y);
            dest.y = Random.Range(boundY.x, boundY.y);

            if(dest.x > forbidenX.x && dest.x < forbidenX.y)
            {
                continue;
            }
            else
            {
                break;
            }

            Debug.Log("Loop");
        }

        Debug.Log(dest);
        return dest;
    }

}
