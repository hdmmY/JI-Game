using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEventMaster: MonoBehaviour
{
    public delegate void OnBulletPropertyChangeDelegate(Bullet_Property prevProperty);
    public OnBulletPropertyChangeDelegate OnBulletPropertyChangeEvent;

    public delegate void BulletPropertyInitDelegate(Bullet_Property initProperty);
    public BulletPropertyInitDelegate BulletPropertyInitEvent;

    public delegate void TriggerPlayerDelegate(Bullet_Property bulletProperty);
    public TriggerPlayerDelegate TriggerPlayerEvent;

    public delegate void TriggerEnemyDelegate(Bullet_Property bulletProperty, Enemy_Property enemyProperty);
    public TriggerEnemyDelegate TriggerEnemyEvent;

    public delegate void TriggerEdgeDelegate();
    public TriggerEdgeDelegate TriggerEdgeEvent;

    public delegate void BulletDisableDelegate();
    public BulletDisableDelegate BulletDisableEvent;



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

            case "Edge":
                if(TriggerEdgeEvent != null)
                {
                    TriggerEdgeEvent();
                }
                break;
        }
    }
}
