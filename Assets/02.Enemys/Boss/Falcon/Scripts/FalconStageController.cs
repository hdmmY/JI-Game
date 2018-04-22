using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Falcon
{
    using MonsterLove.StateMachine;

    public class FalconStageController : MonoBehaviour
    {
        public FalconStageOne StageOneScript;

        public FalconStageTwo StageTwoScript;

        public FalconStageThree StageThreeScript;

        public EnemyProperty BossProperty;

        public int StageOneChangeHealth;

        public int StageTwoChangeHealth;

        public enum States
        {
            StageOne,
            StageTwo,
            StageThree
        }

        private StateMachine<States> _fsm;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake ()
        {
            _fsm = StateMachine<States>.Initialize (this);
            _fsm.ChangeState (States.StageOne);
        }

        private IEnumerator StageOne_Enter ()
        {
            yield return new WaitForSeconds (2f);

            StageOneScript.Active = true;
            StageTwoScript.Active = false;
        }

        private void StageOne_Update ()
        {
            if (BossProperty.m_health <= StageOneChangeHealth)
            {
                _fsm.ChangeState (States.StageTwo);
            }
        }

        private IEnumerator StageTwo_Enter ()
        {
            yield return new WaitForSeconds (2f);

            StageOneScript.Active = false;
            StageTwoScript.Active = true;
        }

        private void StageTwo_Update ()
        {
            if (BossProperty.m_health <= StageTwoChangeHealth)
            {
                _fsm.ChangeState (States.StageThree);
            }
        }

    }
}