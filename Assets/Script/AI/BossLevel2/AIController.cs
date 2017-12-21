using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BossLevel2
{
    public class AIController : BaseAIController
    {
        [Space]
        [Header("P1")]
        public int m_P1attackTimes;
        public int m_P1moveTimes;

        [Space]
        [Header("P2")]
        public int m_P2alignAttackTimes;
        public int m_P2sectorMoveAttackTimes;


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
            SetInt("P1_MoveTimes", 0);
            SetInt("P1_AttackTimes", 0);
        }

        #region Events
        private void InitEventCallBack()
        {
            MoveEnemyState moveState = GetState("P1_Move") as MoveEnemyState;
            if (moveState != null)
            {
                moveState.OnStateEnd += AddMoveTimes;
                moveState.OnStateEnd += P1_MoveTransition;
            }

            AttackEnemyState attackState = GetState("P1_MatrixAttack") as AttackEnemyState;
            if (attackState != null)
            {
                attackState.OnStateEnd += AddAttackTimes;
                attackState.OnStateEnd += P1_AttackTransition;
            }

            AlignAttackEnemyState alignAttackState = GetState("P2_AlignAttack") as AlignAttackEnemyState;
            if (alignAttackState != null)
            {
                alignAttackState.OnStateEnd += AddAlignAttackTimes;
                alignAttackState.OnStateEnd += P2_AlignAttackTransition;
            }

            MoveAttackEnemyState sectorMoveAttState = GetState("P2_SectorMoveAttack") as MoveAttackEnemyState;
            if (sectorMoveAttState != null)
            {
                sectorMoveAttState.OnStateEnd += AddSectorMoveAttackTimes;
                sectorMoveAttState.OnStateEnd += P2_SectorMoveAttackTransition;
            }
        }

        private void ReleaseCallBack()
        {
            foreach (var state in m_totalEnemyAIStates)
            {
                if(state.OnStateEnd != null)
                {
                    var dels = state.OnStateEnd.GetInvocationList();
                    foreach (var del in dels)
                    {
                        state.OnStateEnd -= del as System.Action;
                    }
                }
               
            }
        }
        #endregion                                          


        private void AddMoveTimes()
        {
            SetInt("P1_MoveTimes", GetInt("P1_MoveTimes") + 1);
        }

        private void AddAttackTimes()
        {
            SetInt("P1_AttackTimes", GetInt("P1_AttackTimes") + 1);
        }

        private void AddAlignAttackTimes()
        {
            SetInt("P2_AlignAttackTimes", GetInt("P2_AlignAttackTimes") + 1);
        }

        private void AddSectorMoveAttackTimes()
        {
            SetInt("P2_SectorMoveAttackTimes", GetInt("P2_SectorMoveAttackTimes") + 1);
        }


        #region State Transition
        private void P1_MoveTransition()
        {
            if (GetInt("P1_AttackTimes") < m_P1attackTimes)
            {
                _currentEnemyAI = GetState("P1_MatrixAttack");
                _currentEnemyAI.Initialize(_enemyProperty);
            }
            else
            {
                _currentEnemyAI = GetState("P2_AlignAttack");
                _currentEnemyAI.Initialize(_enemyProperty);
            }
        }

        private void P1_AttackTransition()
        {
            if (GetInt("P1_MoveTimes") < m_P1moveTimes)
            {
                _currentEnemyAI = GetState("P1_Move");
                _currentEnemyAI.Initialize(_enemyProperty);
            }
            else
            {
                _currentEnemyAI = GetState("P2_AlignAttack");
                _currentEnemyAI.Initialize(_enemyProperty);
            }
        }

        private void P2_AlignAttackTransition()
        {
            _currentEnemyAI = GetState("P2_SectorMoveAttack");
            _currentEnemyAI.Initialize(_enemyProperty);
        }

        private void P2_SectorMoveAttackTransition()
        {
            _currentEnemyAI = GetState("P2_AlignAttack");
            _currentEnemyAI.Initialize(_enemyProperty);
        }

        #endregion

    }
}


