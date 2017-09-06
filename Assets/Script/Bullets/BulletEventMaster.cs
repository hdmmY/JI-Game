using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEventMaster: MonoBehaviour
{
    public delegate void OnBulletPropertyChangeDelegate(Bullet_Property prevProperty);
    public event OnBulletPropertyChangeDelegate OnBulletPropertyChangeEvent;

    public delegate void BulletPropertyInitDelegate(Bullet_Property initProperty);
    public event BulletPropertyInitDelegate BulletPropertyInitEvent;

    public delegate void TriggerPlayerDelegate(Bullet_Property bulletProperty);
    public event TriggerPlayerDelegate TriggerPlayerEvent;

    public delegate void TriggerEnemyDelegate(Bullet_Property bulletProperty, Enemy_Property enemyProperty);
    public event TriggerEnemyDelegate TriggerEnemyEvent;   

    public delegate void BulletDisableDelegate();
    public event BulletDisableDelegate BulletDisableEvent;



    public void CallBulletPropertyInitEvent(Bullet_Property initProperty)
    {
        if(BulletPropertyInitEvent != null)
        {
            BulletPropertyInitEvent(initProperty);
        }
    }

    public void CallOnBulletPropertyChangeEvent(Bullet_Property prevProperty)
    {
        if(OnBulletPropertyChangeEvent != null)
        {
            OnBulletPropertyChangeEvent(prevProperty);
        }
    }


    public void CallBulletDisabledEvent()
    {
        if(BulletDisableEvent != null)
        {
            BulletDisableEvent();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch(collision.tag)
        {
            case "Player":
                if (TriggerPlayerEvent != null)
                {
                    TriggerPlayerEvent(GetComponent<Bullet_Property>());
                }
                break;

            case "Enemy":
                if (TriggerEnemyEvent != null)
                {
                    TriggerEnemyEvent(GetComponent<Bullet_Property>(), collision.GetComponent<Enemy_Property>());
                }
                break;
        }
    }
}
