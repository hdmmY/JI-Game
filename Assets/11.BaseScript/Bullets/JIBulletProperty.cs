using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JIBulletProperty : MonoBehaviour
{
    public int m_damage;

    public JIState State
    {
        get
        {
            string name = transform.name.ToLower ();

            if (name.Contains ("white"))
                return JIState.White;
            if (name.Contains ("black"))
                return JIState.Black;

            Debug.LogError ("The bullet name not correct!");
            return JIState.Black;
        }
    }
}