using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventMaster: MonoBehaviour
{
    public delegate void OnEnemyDeathDelegate();
    public OnEnemyDeathDelegate OnEnemyDeathEvent;



    public void CallEnemyDeathEvent()
    {
        if(OnEnemyDeathEvent != null)
        {
            OnEnemyDeathEvent();
        }
    }
}
