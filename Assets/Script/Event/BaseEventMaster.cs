using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEventMaster : MonoBehaviour
{
    /// <summary>
    /// Call when this gameobject OnEnable
    /// </summary>
    public System.Action<GameObject> WhenEnable;

    /// <summary>
    /// Call when this gameobject Destory
    /// </summary>
    public System.Action<GameObject> WhenDestroy;


    protected void CallWhenEnable()
    {
        if(WhenEnable != null)
        {
            WhenEnable(gameObject);
        }
    }

    protected void CallWhenDestory()
    {
        if(WhenDestroy != null)
        {
            WhenDestroy(gameObject);
        }
    }             

    protected void OnEnable()
    {
        CallWhenEnable();
    }

    protected void OnDestroy()
    {
        CallWhenDestory();
    }          

    
}
