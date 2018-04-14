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

    private float _verticalSpeed;
    private float _horizontalSpeed;

    private PlayerProperty _playerProperty;

    private enum TimeState
    {
        Normal,
        Pausing,
        Resume
    }

    TimeState _timeState;

    private void OnEnable ()
    {
        SetInitReference ();

        _verticalSpeed = _playerProperty.m_horizontalSpeed;
        _horizontalSpeed = _playerProperty.m_verticalSpeed;
    }

    private void Update ()
    {
        UpdateSpeed ();

        float horizontalMove = InputManager.Instance.InputCtrl.HorizontalInput () * _horizontalSpeed;
        float VerticalMove = InputManager.Instance.InputCtrl.VerticalInput () * _verticalSpeed;

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
    void UpdateSpeed ()
    {
        if (_playerProperty.m_playerMoveState == PlayerProperty.PlayerMoveType.HighSpeed)
        {
            _horizontalSpeed = _playerProperty.m_horizontalSpeed;
            _verticalSpeed = _playerProperty.m_verticalSpeed;
        }
        else
        {
            _horizontalSpeed = _playerProperty.m_slowHorizontalSpeed;
            _verticalSpeed = _playerProperty.m_slowVerticalSpeed;
        }
    }

    void SetInitReference ()
    {
        _playerProperty = GetComponent<PlayerProperty> ();
        //_animator = GetComponent<Animator>();
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube (m_moveAreaCentre, new Vector3 (m_moveAreaShape.x, m_moveAreaShape.y, 0));
    }
}