using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerProperty))]
[RequireComponent(typeof(PlayerReference))]
public class PlayerMove : MonoBehaviour
{

    private float _verticalSpeed;
    private float _horizontalSpeed;

    private PlayerReference _playerRefer;
    private PlayerProperty _playerProperty;

    private void OnEnable()
    {
        SetInitReference();

        _verticalSpeed = _playerProperty.m_horizontalSpeed;
        _horizontalSpeed = _playerProperty.m_verticalSpeed;
    }

    private void Update()
    {
        UpdateSpeed();

        transform.position += new Vector3(InputManager.Instance.HorizontalInput * _verticalSpeed,
                                          InputManager.Instance.VerticalInput, 0f) * Time.deltaTime;
    }


    void UpdateSpeed()
    {
        _verticalSpeed = _playerProperty.m_horizontalSpeed;
        _horizontalSpeed = _playerProperty.m_verticalSpeed;
    }


    void SetInitReference()
    {
        _playerRefer = GetComponent<PlayerReference>();
        _playerProperty = GetComponent<PlayerProperty>();
    }
}
