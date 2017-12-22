using System.Collections;
using System.Collections.Generic;
using UnityEngine;
                           

namespace BossLevel2
{
    /*******************   Behavior   *******************************************
    Firstly, move to the centre.
    Secondly, shot align bullet.
    Thridly, shot m_shotPattern
    Fourthly, wait for m_timeToWaitAfterShotDone seconds
    End
    ****************************************************************************/
    public class AlignAttackEnemyState : AttackEnemyState
    {
        // The boss position y when boss shot bullet.
        public float m_bossPositionY;                

        [Space]
        [Header("Align Shot")]
        public UbhBaseShot m_alignShotLeft;
        public UbhBaseShot m_alignShotRight;
        public float m_alignBulletSpeed;              // bullet speed for align way bullet
        public float m_alignBulletEmitInterval;       // bullet shot interval for m_alignShotLeft & m_alignShotRight


        // Control first, second, third sub state
        private bool _moveToCenterDone;
        private bool _alignShotDone;
        private bool _shotPatternDone;  

        public override void Initialize(EnemyProperty enemyProperty)
        {
            base.Initialize(enemyProperty);

            _moveToCenterDone = false;
            _alignShotDone = false;
            _shotPatternDone = false;

            _timer = 0f;
            _curShotTimes = m_shotTimes;

            m_alignShotLeft.m_bulletSpeed = m_alignShotRight.m_bulletSpeed = m_alignBulletSpeed;
        }


        public override void UpdateState(EnemyProperty enemyProperty)
        {
            if (_stateEnd) return;    

            // First sub state
            if(!_moveToCenterDone)
            {
                MoveToCentre(enemyProperty);
                return;
            }
                        
            // Second sub state
            if(_moveToCenterDone && !_alignShotDone)
            {
                StartCoroutine(ShotAlignBullet());
                return;
            }

            // Thrid sub state
            if(_moveToCenterDone && _alignShotDone && !_shotPatternDone)
            {
                if (_curShotTimes > 0)
                {
                    Shot();
                }
                return;
            }

            // Fourth sub state
            if(_moveToCenterDone && _alignShotDone && _shotPatternDone)
            {
                _timer += JITimer.Instance.DeltTime;

                if (_timer >= m_timeToWaitAfterShotDone)
                {
                    _stateEnd = true;
                    CallOnStateEnd();
                }
                return;
            }
        }


        public override void EndState(EnemyProperty enemyProperty)
        {
            base.EndState(enemyProperty);
        }       
    
                                          
        private void MoveToCentre(EnemyProperty enemyProperty)
        {
            Vector3 destination = JIGlobalRef.Player.transform.position;
            destination.y = m_bossPositionY;

            Vector3 moveDir = destination - enemyProperty.transform.position;

            if (moveDir.sqrMagnitude <= 0.1)  // End movement
            {
                _moveToCenterDone = true;
                enemyProperty.transform.position = destination;
            }
            else  // Move
            {
                enemyProperty.transform.position += moveDir.normalized * 2.0f * JITimer.Instance.DeltTime;
            }

            ResetBoundRect(m_shotPattern as SpecialShot.BounceMatrixShot, enemyProperty.transform.position.x);
        }


        private IEnumerator ShotAlignBullet()
        {
            _alignShotDone = true;

            float timer = 0f;

            while(!_stateEnd)
            {
                timer += JITimer.Instance.DeltTime;
                
                if(timer > m_alignBulletEmitInterval)
                {
                    timer -= m_alignBulletEmitInterval;

                    // The align shot bullet number is set to 1 to ensure that each 
                    // shot can only shot one bullet. Align shot bullet emit interval is controlled by m_alignBulletEmitInterval
                    m_alignShotLeft.m_bulletNum = m_alignShotRight.m_bulletNum = 1;   

                    m_alignShotLeft.Shot();
                    m_alignShotRight.Shot();
                }

                yield return null;
            }
        }


        private void Shot()
        {
            _timer += JITimer.Instance.DeltTime;

            if (_timer >= m_shotInterval)
            {
                m_shotPattern.Shot();
                _curShotTimes--;

                _timer -= m_shotInterval;
            }


            // Shot Done
            if (_curShotTimes == 0)
            {
                _shotPatternDone = true;
                _timer = 0;
            }
        }

        
        /// <summary>
        /// Reset the bouce rect of bounce matrix shot, so its center.x is x
        /// </summary>
        private void ResetBoundRect(SpecialShot.BounceMatrixShot bounceMatrixShot, float x)
        {
            if (bounceMatrixShot == null) return;

            Rect originRect = bounceMatrixShot.m_bounceBound;

            x = x - originRect.width / 2;
            originRect.Set(x, originRect.y, originRect.width, originRect.height);

            bounceMatrixShot.m_bounceBound = originRect;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(new Vector3(-100, m_bossPositionY, 0), new Vector3(100, m_bossPositionY, 0));
        }                                    
    }   
}

