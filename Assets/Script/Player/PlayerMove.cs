using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public InputManager m_InputManager;

    public float m_verticalSpeed;
    public float m_horizontalSpeed;

    private void Update()
    {
        transform.position += new Vector3(m_InputManager.m_HorizontalInput * m_horizontalSpeed,
                                          m_InputManager.m_VerticalInput * m_verticalSpeed, 0f) * Time.deltaTime;
    }




}
