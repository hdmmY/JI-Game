using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerProperty))]
public class PlayerMove : MonoBehaviour
{

    private float _verticalSpeed;
    private float _horizontalSpeed;

    private PlayerProperty _playerProperty;

    private Animator _animator;

    private void OnEnable()
    {
        SetInitReference();

        _verticalSpeed = _playerProperty.m_horizontalSpeed;
        _horizontalSpeed = _playerProperty.m_verticalSpeed;
    }

    private void Update()
    {
        UpdateSpeed();

        transform.position += new Vector3
                (InputManager.Instance.HorizontalInput * _horizontalSpeed,
                 InputManager.Instance.VerticalInput * _verticalSpeed, 0f) * Time.deltaTime;
        _animator.SetFloat("HorizontalMove", InputManager.Instance.HorizontalInput);
    }


    void UpdateSpeed()
    {
        _horizontalSpeed = _playerProperty.m_horizontalSpeed;
        _verticalSpeed = _playerProperty.m_verticalSpeed;
    }


    void SetInitReference()
    {
        _playerProperty = GetComponent<PlayerProperty>();
        _animator = GetComponent<Animator>();
    }
}
