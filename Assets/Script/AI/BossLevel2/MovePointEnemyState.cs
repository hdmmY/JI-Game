using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossLevel2
{
    // Move the enemy to a point
    public class MovePointEnemyState : BaseEnemyState
    {
        public float m_moveSpeed;

        public Vector3 m_destination;

        private Vector3 _moveDir;

        public override void Initialize(Enemy_Property enemyProperty)
        {
            base.Initialize(enemyProperty);

            _moveDir = (m_destination - enemyProperty.transform.position).normalized;
        }

        public override void UpdateState(Enemy_Property enemyProperty)
        {
            base.UpdateState(enemyProperty);

            Vector3 nextPosition = enemyProperty.transform.position + _moveDir * m_moveSpeed * JITimer.Instance.DeltTime;
            
            // Move overhead, end the state
            if(Vector3.Dot(nextPosition - m_destination, _moveDir) > 0)
            {
                CallOnStateEnd();
                _stateEnd = true;
            }

            enemyProperty.transform.position = nextPosition;
        }

        public override void EndState(Enemy_Property enemyProperty)
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


