using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossLevel2
{
    public class MoveAttackEnemyState : BaseEnemyState
    {
        [Header("Move Properties")]
        // Total move and attack time for one state
        public float m_totalTime;

        // Enemy move speed
        public float m_moveSpeed;

        // Move bound
        public Rect m_bound;


        [Space]
        [Header("Attack Properties")]
        // Matrix shot pattern that will be use
        public UbhBaseShot m_shotPattern;

        // Interval between each shot
        public float m_shotInterval;

        // Time to wait after all shot done
        public int m_timeToWaitAfterShotDone;


        // Move relative local variable                                               
        private Vector3 _destination;        // Destination you want to move to        
        private Vector3 _moveDir;            // Move direction, normalized
        private float _moveTimer;
        private bool _moveDone;              // Move timer > total move time, move end

        // Attack relative local variable
        private float _attackTimer;

        public override void Initialize(EnemyProperty enemyProperty)
        {
            base.Initialize(enemyProperty);

            InitializeAttack(enemyProperty);
            InitializeMove(enemyProperty);
        }

        public override void UpdateState(EnemyProperty enemyProperty)
        {
            base.UpdateState(enemyProperty);
            if(_stateEnd)
            {
                return;
            }

            _moveTimer += JITimer.Instance.DeltTime;
            _attackTimer += JITimer.Instance.DeltTime;

            // Update attack
            UpdateAttack(enemyProperty);

            // Update movement
            if (_moveTimer < m_totalTime)
            {
                UpdateMove(enemyProperty);
            }
            // Movement end
            else if(!_moveDone)
            {
                _moveDone= true;
                _attackTimer = 0f;
            }
        }

        public override void EndState(EnemyProperty enemyProperty)
        {
            base.EndState(enemyProperty);
        }


        private void InitializeMove(EnemyProperty enemyProperty)
        {                                    
            // Initialize destination
            _destination = FindNextPosition();
            _moveDir = _destination.normalized;

            _moveTimer = 0;
            _moveDone = false;
        }

        private void InitializeAttack(EnemyProperty enemyProperty)
        {
            _attackTimer = 0f;
        }

        private void UpdateAttack(EnemyProperty enemyProperty)
        {
            // Move done, after m_timeTWaitAfterShot seconds, end the state.
            if (_moveDone)
            {
                if (_attackTimer >= m_timeToWaitAfterShotDone)
                {
                    CallOnStateEnd();
                    _stateEnd = true;
                }          
            }
            // move not done, shot bullet
            else if(_attackTimer > m_shotInterval)
            {
                _attackTimer -= m_shotInterval;
                m_shotPattern.Shot();
            }

            
        }

        private void UpdateMove(EnemyProperty enemyProperty)
        {
            Vector3 nextPosition = enemyProperty.transform.position + _moveDir * m_moveSpeed * JITimer.Instance.DeltTime;

            // Move overhead, find next destination
            if (Vector3.Dot(nextPosition - _destination, _moveDir) > 0)      
            {
                _destination = FindNextPosition();
                _moveDir = (_destination - nextPosition).normalized;
            }
            else
            {
                enemyProperty.transform.position = nextPosition;
            }
        }



        // Find the next position that enemy will move to
        private Vector3 FindNextPosition()
        {                                                  
            Vector3 dest = new Vector3();

            dest.x = Random.Range(m_bound.xMin, m_bound.xMax);
            dest.y = Random.Range(m_bound.yMin, m_bound.yMax);

            return dest;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(m_bound.center, m_bound.size);
        }

    }

}

