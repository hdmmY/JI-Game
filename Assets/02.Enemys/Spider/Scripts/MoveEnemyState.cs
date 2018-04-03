using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossLevel2 {
    public class MoveEnemyState : BaseEnemyState {
        // Total move time for one state
        public float m_totalMoveTime;

        // Enemy move speed
        public float m_moveSpeed;

        // Move bound
        public Rect m_bound;

        // Destination you want to move to
        private Vector3 _destination;

        // Move direction, normalized
        private Vector3 _moveDir;

        private float _timer;

        public override void Initialize (EnemyProperty enemyProperty) {
            base.Initialize (enemyProperty);

            // Initialize destination
            _destination = FindNextPosition ();
            _moveDir = _destination.normalized;

            _timer = 0;
        }

        public override void UpdateState (EnemyProperty enemyProperty) {
            base.UpdateState (enemyProperty);
            if (_stateEnd) {
                return;
            }

            // Add _timer to check if it is end.
            _timer += JITimer.Instance.DeltTime;
            if (_timer >= m_totalMoveTime) {
                _stateEnd = true;
                CallOnStateEnd ();
            }

            Vector3 nextPosition = enemyProperty.transform.position + _moveDir * m_moveSpeed * JITimer.Instance.DeltTime;

            // Move overhead, find next destination
            if (Vector3.Dot (nextPosition - _destination, _moveDir) > 0) {
                _destination = FindNextPosition ();
                _moveDir = (_destination - nextPosition).normalized;
            } else {
                enemyProperty.transform.position = nextPosition;
            }
        }

        public override void EndState (EnemyProperty enemyProperty) {
            base.EndState (enemyProperty);
        }

        // Find the next position that enemy will move to
        private Vector3 FindNextPosition () {
            Vector3 dest = new Vector3 ();

            dest.x = Random.Range (m_bound.xMin, m_bound.xMax);
            dest.y = Random.Range (m_bound.yMin, m_bound.yMax);;

            return dest;
        }

        private void OnDrawGizmosSelected () {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube (m_bound.center, m_bound.size);
        }

    }
}