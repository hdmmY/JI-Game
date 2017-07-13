using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public float m_HorizontalInput;

    public float m_VerticalInput;

    public KeyCode m_ShootKey;

    public bool m_Shoot;

    private void Update()
    {
        m_HorizontalInput = Input.GetAxis("Horizontal");
        m_VerticalInput = Input.GetAxis("Vertical");

        if(Input.GetKey(m_ShootKey))
        {
            m_Shoot = true;    
        }
        else
        {
            m_Shoot = false;
        }
    }


}
