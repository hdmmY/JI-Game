using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class JISingletonMonoBehavior<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if(_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Start()
    {
        if(this != _instance)
        {
            GameObject go = this.gameObject;

            Debug.Log(this);
            Debug.Log(_instance);
            Destroy(this);
            Destroy(go);
        }
    }



}
