using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossLevel2
{
    // Move the enemy to a point
    public class MovePointEnemyState : BaseEnemyState
    {
        public float m_waitTimeWhenReachDest = 0;

        public float m_moveSpeed;

        public Vector3 m_destination;

        private Vector3 _moveDir;

        private bool _reachDestination;
        private float _waitTimer;

        public override void Initialize(EnemyProperty enemyProperty)
        {
            base.Initialize(enemyProperty);

            _moveDir = (m_destination - enemyProperty.transform.position).normalized;
        }

        public override void UpdateState(EnemyProperty enemyProperty)
        {
            base.UpdateState(enemyProperty);

            Vector3 nextPosition = enemyProperty.transform.position + _moveDir * m_moveSpeed * JITimer.Instance.DeltTime;

            // Is move overhead? If true, means reach destination
            if (Vector3.Dot(nextPosition - m_destination, _moveDir) > 0)
            {
                _reachDestination = true;
                _waitTimer = 0;
            }

            if (_reachDestination)   // Wait and end the state
            {
                _waitTimer += JITimer.Instance.DeltTime;
                if (_waitTimer >= m_waitTimeWhenReachDest)
                {
                    CallOnStateEnd();
                    _stateEnd = true;
                }
            }
            else  // Update  position
            {
                enemyProperty.transform.position = nextPosition;
            }
        }

        public override void EndState(EnemyProperty enemyProperty)
        {
            base.EndState(enemyProperty);
        }


        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(m_destination, Vector3.one * 0.1f);
        }
    }
}


