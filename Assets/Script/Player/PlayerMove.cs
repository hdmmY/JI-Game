using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerProperty))]
public class PlayerMove : MonoBehaviour
{
    private float _verticalSpeed;
    private float _horizontalSpeed;

    private PlayerProperty _playerProperty;

    //private Animator _animator;

    private enum TimeState
    {
        Normal,
        Pausing,
        Resume
    };
    TimeState _timeState;

    private void OnEnable()
    {
        SetInitReference();

        _verticalSpeed = _playerProperty.m_horizontalSpeed;
        _horizontalSpeed = _playerProperty.m_verticalSpeed;

        UbhTimer.Instance.TimeScale = 1;
    }

    private void Update()
    {
        UpdateSpeed();

        // player move is not affected by time scale.
        float deltTime = Time.deltaTime;

        transform.position += new Vector3
                (InputManager.Instance.HorizontalInput * _horizontalSpeed,
                 InputManager.Instance.VerticalInput * _verticalSpeed, 0f) * UbhTimer.Instance.DeltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_timeState == TimeState.Pausing)
            {
                _timeState = TimeState.Resume;
                StopAllCoroutines();
                StartCoroutine(Resuming(UbhTimer.Instance.TimeScale, 1.5f));
            }
            else if(_timeState == TimeState.Normal || _timeState == TimeState.Resume)
            {
                _timeState = TimeState.Pausing;
                StopAllCoroutines();
                StartCoroutine(Pausing(0.1f, 0.5f));
            }
        }


    }


    IEnumerator Pausing(float endTimeScale, float lastTime)
    {
        while (UbhTimer.Instance.TimeScale >= endTimeScale)
        {
            UbhTimer.Instance.TimeScale -= (1 - endTimeScale) * Time.deltaTime / lastTime;
            yield return null;
        }

        UbhTimer.Instance.TimeScale = endTimeScale;
    }

    IEnumerator Resuming(float curTimeScale, float lastTime)
    {
        while (UbhTimer.Instance.TimeScale < 1f)
        {
            UbhTimer.Instance.TimeScale += (1 - curTimeScale) * Time.deltaTime / lastTime;
            yield return null;
        }
        UbhTimer.Instance.TimeScale = 1f;
        _timeState = TimeState.Normal;
    }


    void UpdateSpeed()
    {
        if (_playerProperty.m_playerMoveState == PlayerProperty.PlayerMoveType.HighSpeed)
            _horizontalSpeed = _playerProperty.m_horizontalSpeed;
        else
            _horizontalSpeed = _playerProperty.m_slowHorizontalSpeed;
        _verticalSpeed = _playerProperty.m_verticalSpeed;
    }


    void SetInitReference()
    {
        _playerProperty = GetComponent<PlayerProperty>();
        //_animator = GetComponent<Animator>();
    }
}
