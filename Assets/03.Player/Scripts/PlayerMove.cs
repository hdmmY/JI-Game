using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerProperty))]
public class PlayerMove : MonoBehaviour
{
    // Centre point
    public Vector2 m_moveAreaCentre;
    // Width & height
    public Vector2 m_moveAreaShape;

    private PlayerProperty _player;

    private void OnEnable ()
    {
        _player = GetComponent<PlayerProperty> ();
    }

    private void Update ()
    {
        float hInput = InputManager.Instance.InputCtrl.HorizontalInput;
        float vInput = InputManager.Instance.InputCtrl.VerticalInput;

        float hSpeed, vSpeed;
        if(_player.m_playerState == JIState.White)
        {
            hSpeed = _player.m_whiteHSpeed;
            vSpeed = _player.m_whiteVSpeed;
        }
        else if(_player.m_playerState == JIState.Black)
        {
            hSpeed = _player.m_blackHSpeed;
            vSpeed = _player.m_blackVSpeed;
        }
        else
        {
            return;
        }

        float horizontalMove = hInput * hSpeed;
        float VerticalMove = vInput * vSpeed;

        Vector2 playerPos = (Vector2) transform.position +
            new Vector2 (horizontalMove, VerticalMove) * Time.deltaTime;

        // player position is out of bound
        if (playerPos.x < m_moveAreaCentre.x - m_moveAreaShape.x / 2)
        {
            playerPos.x = m_moveAreaCentre.x - m_moveAreaShape.x / 2;
        }
        if (playerPos.x > m_moveAreaCentre.x + m_moveAreaShape.x / 2)
        {
            playerPos.x = m_moveAreaCentre.x + m_moveAreaShape.x / 2;
        }
        if (playerPos.y < m_moveAreaCentre.y - m_moveAreaShape.y / 2)
        {
            playerPos.y = m_moveAreaCentre.y - m_moveAreaShape.y / 2;
        }
        if (playerPos.y > m_moveAreaCentre.y + m_moveAreaShape.y / 2)
        {
            playerPos.y = m_moveAreaCentre.y + m_moveAreaShape.y / 2;
        }

        transform.position = new Vector3 (playerPos.x, playerPos.y, transform.position.z);
    }
    

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube (m_moveAreaCentre, new Vector3 (m_moveAreaShape.x, m_moveAreaShape.y, 0));
    }
}