using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BossLevel2
{
    public class AIController : BaseAIController
    {
        [Space]
        [Header("Stage1")]
        public int m_attackTimes;
        public int m_moveTimes;


        #region MonoBehavior
        protected override void Start()
        {
            base.Start();

            _currentEnemyAI = m_totalEnemyAIStates[0];
            _currentEnemyAI.Initialize(_enemyProperty);

            InitAIParams();
            InitEventCallBack();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void OnDisable()
        {
            ReleaseCallBack();
        }
        #endregion

        private void InitAIParams()
        {
            SetInt("MoveTimes", 0);
            SetInt("AttackTimes", 0);
        }

        #region Events
        private void InitEventCallBack()
        {
            MoveEnemyState moveState = GetState("Move State") as MoveEnemyState;
            if (moveState != null)
            {
                moveState.OnStateEnd += AddMoveTimes;
                moveState.OnStateEnd += MoveStateTransition;
            }

            AttackEnemyState attackState = GetState("Attack State") as AttackEnemyState;
            if (attackState != null)
            {
                attackState.OnStateEnd += AddAttackTimes;
                attackState.OnStateEnd += AttackStateTransition;
            }

            AlignAttackEnemyState alignAttackState = GetState("AlignAttack State") as AlignAttackEnemyState;
            if(alignAttackState != null)
            {
                alignAttackState.OnStateEnd += AlignAttackStateTransition;
            }
        }

        private void ReleaseCallBack()
        {
            MoveEnemyState moveState = GetState("Move State") as MoveEnemyState;
            if (moveState != null)
            {
                moveState.OnStateEnd -= AddMoveTimes;
                moveState.OnStateEnd -= MoveStateTransition;
            }         

            AttackEnemyState attackState = GetState("Attack State") as AttackEnemyState;
            if (attackState != null)
            {
                attackState.OnStateEnd -= AddAttackTimes;
                attackState.OnStateEnd -= AttackStateTransition;
            }

            AlignAttackEnemyState alignAttackState = GetState("AlignAttack State") as AlignAttackEnemyState;
            if (alignAttackState != null)
            {
                alignAttackState.OnStateEnd -= AlignAttackStateTransition;
            }
        }
        #endregion                                          


        private void AddMoveTimes()
        {
            SetInt("MoveTimes", GetInt("MoveTimes") + 1);
        }

        private void AddAttackTimes()
        {
            SetInt("AttackTimes", GetInt("AttackTimes") + 1);
        }



        #region State Transition
        private void MoveStateTransition()
        {
            if (GetInt("AttackTimes") < m_attackTimes)
            {
                _currentEnemyAI = GetState("Attack State");
                _currentEnemyAI.Initialize(_enemyProperty);
            }
            else
            {
                _currentEnemyAI = GetState("AlignAttack State");
                _currentEnemyAI.Initialize(_enemyProperty);
            }
        }

        private void AttackStateTransition()
        {
            if (GetInt("MoveTimes") < m_moveTimes)
            {
                _currentEnemyAI = GetState("Move State");
                _currentEnemyAI.Initialize(_enemyProperty);
            }
            else
            {
                _currentEnemyAI = GetState("AlignAttack State");
                _currentEnemyAI.Initialize(_enemyProperty);
            }
        }

        private void AlignAttackStateTransition()
        {
            SetInt("MoveTimes", 0);
            SetInt("AttackTimes", 0);

            _currentEnemyAI = GetState("Move State");
            _currentEnemyAI.Initialize(_enemyProperty);
        }

        #endregion

    }
}


