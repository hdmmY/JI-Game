using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BossLevel2
{
    public class AttackEnemyState : BaseEnemyState
    {
        // Matrix shot pattern that will be use
        public UbhBaseShot m_shotPattern;

        // Total Shot times
        public int m_shotTimes = 2;

        // Interval between each shot
        public int m_shotInterval;

        // Time to wait after all shot done
        public int m_timeToWaitAfterShotDone;

        private int _curShotTimes;
        private bool _allShotDone;
        private float _timer;



        public override void Initialize(Enemy_Property enemyProperty)
        {
            base.Initialize(enemyProperty);

            _curShotTimes = m_shotTimes;
            _allShotDone = false;
            _timer = 0f;
        }


        public override void UpdateState(Enemy_Property enemyProperty)
        {
            base.UpdateState(enemyProperty);
            if (_stateEnd)
            {
                return;
            }

            // Shot 
            if(_curShotTimes > 0)
            {
                Shot();
            }         

            // After m_timeTWaitAfterShot seconds, end the state.
            if (_allShotDone)
            {
                _timer += JITimer.Instance.DeltTime;

                if (_timer >= m_timeToWaitAfterShotDone)
                {
                    _stateEnd = true;
                    CallOnStateEnd();
                }
            }
        }

        /// <summary>
        /// Call to force to end the state.
        /// </summary>
        public override void EndState(Enemy_Property enemyProperty)
        {
            base.EndState(enemyProperty);
        }


        private void Shot()
        {
            _timer += JITimer.Instance.DeltTime;

            if(_timer >= m_shotInterval)
            {               
                m_shotPattern.Shot();
                _curShotTimes--;

                _timer -= m_shotInterval;
            }


            // Shot Done
            if(_curShotTimes == 0)
            {
                _allShotDone = true;
                _timer = 0;
            }
        }

    }

}



