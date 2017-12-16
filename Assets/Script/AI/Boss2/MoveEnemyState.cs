using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss2
{
    public class MoveEnemyState : BaseEnemyState
    {
        // Enemy move speed
        public float m_moveSpeed;

        // Move bound
        public Rect m_bound;

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

            Vector3 nextPosition = enemyProperty.transform.position + _moveDir * m_moveSpeed * JITimer.Instance.DeltTime;

            if (Vector3.Dot(nextPosition - _destination, _moveDir) > 0)      // move overhead, end move
            {
                _destination = FindNextPosition();
                _moveDir = (_destination - nextPosition).normalized;
            }

            enemyProperty.transform.position = nextPosition;
        }


        // Find the next position that enemy will move to
        private Vector3 FindNextPosition()
        {                                   
            // Range that enemy can't move
            Vector2 forbidenX = JIGlobalRef.Player.transform.position;
            forbidenX.x -= _colBound.size.x * 0.5f;

            Vector3 dest = new Vector3();
            while (true)
            {
                dest.x = Random.Range(m_bound.xMin, m_bound.xMax);
                dest.y = Random.Range(m_bound.yMin, m_bound.yMax);

                if (dest.x > forbidenX.x && dest.x < forbidenX.y)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            return dest;
        }




        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(m_bound.center, m_bound.size);
        }

    }
}


