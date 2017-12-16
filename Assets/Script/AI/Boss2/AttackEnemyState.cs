using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Boss2
{
    public class AttackEnemyState : BaseEnemyState
    {                                     
        // Matrix shot pattern that will be use
        public UbhBaseShot m_shotPattern;

        // Total Shot times
        public int m_shotTimes = 2;            



        public override void Initialize(Enemy_Property enemyProperty)
        {
            base.Initialize(enemyProperty);         
        }


        public override void UpdateState(Enemy_Property enemyProperty)
        {
            base.UpdateState(enemyProperty);

            if (m_shotTimes > 0)
            {
                m_shotPattern.Shot();
                m_shotTimes--;
            }            
        }

       
    }

}



