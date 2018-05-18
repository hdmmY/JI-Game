using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input with wind resistance
/// </summary>
public class PlayerWindInputCtrl : IPlayerInputCtrl
{
    /// <summary>
    /// Strength to restore from the wind.
    /// Must be no negtive
    /// </summary>
    public float PlayerRestoreStrength
    {
        get
        {
            return _strength;
        }
        set
        {
            _strength = value > 0 ? value : 0;
        }
    }

    /// <summary>
    /// Wind direction
    /// </summary>
    public Vector2 WindDirection;

    private float _strength;

    private float _upHoldTime;

    private float _downHoldTime;

    private float _leftHoldTime;

    private float _rightHoldTime;

    private bool GoRight => Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);

    private bool GoLeft => Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);

    private bool GoUp => Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W);

    private bool GoDown => Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S);

    public void Update ()
    {
        float deltT = JITimer.Instance.DeltTime;

        _upHoldTime = GoUp ? _upHoldTime + deltT : 0f;
        _downHoldTime = GoDown ? _downHoldTime + deltT : 0f;
        _leftHoldTime = GoLeft ? _leftHoldTime + deltT : 0f;
        _rightHoldTime = GoRight ? _rightHoldTime + deltT : 0f;
    }

    public float VerticalInput
    {
        get
        {
            if (GoUp && GoDown) return Mathf.Sign (WindDirection.y);

            if (GoUp)
            {
                if (WindDirection.y >= 0)
                    return 1;
                else
                {
                    var vMove = Mathf.Clamp01 (_upHoldTime * _strength);
                    vMove = -1 + 2 * Mathf.Sin (vMove * Mathf.PI * 0.5f);
                    return vMove;
                }
            }

            if (GoDown)
            {
                if (WindDirection.y <= 0)
                    return -1;
                else
                {
                    var vMove = Mathf.Clamp01 (_upHoldTime * _strength);
                    vMove = 1 - 2 * Mathf.Sin (vMove * Mathf.PI * 0.5f);
                    return vMove;
                }
            }

            return WindDirection.y == 0 ? 0 : Mathf.Sign (WindDirection.y);
        }
    }

    public float HorizontalInput
    {
        get
        {
            if (GoRight && GoLeft) return Mathf.Sign (WindDirection.x);

            if (GoRight)
            {
                if (WindDirection.x >= 0)
                    return 1;
                else if (WindDirection.x < 0)
                {
                    var hMove = Mathf.Clamp01 (_rightHoldTime * _strength);
                    hMove = -1 + 2 * Mathf.Sin (hMove * Mathf.PI * 0.5f);
                    return hMove;
                }
            }

            if (GoLeft)
            {
                if (WindDirection.x <= 0)
                    return -1;
                else
                {
                    var hMove = Mathf.Clamp01 (_rightHoldTime * _strength);
                    hMove = 1 - 2 * Mathf.Sin (hMove * Mathf.PI * 0.5f);
                    return hMove;
                }
            }

            return WindDirection.x == 0 ? 0 : Mathf.Sign (WindDirection.x);
        }
    }
    public bool ShotButton
    {
        get
        {
            return Input.GetKey (KeyCode.Z);
        }
    }

    public bool ShotButtonUp
    {
        get
        {
            return Input.GetKeyUp (KeyCode.Z);
        }
    }

    public bool ShotButtonDown
    {
        get
        {
            return Input.GetKeyDown (KeyCode.Z);
        }
    }

    public bool ChangeStateButton
    {
        get
        {
            return Input.GetKey (KeyCode.LeftShift);
        }
    }

    public bool ChangeStateButtonUp
    {
        get
        {
            return Input.GetKeyUp (KeyCode.LeftShift);
        }
    }

    public bool ChangeStateButtonDown
    {
        get
        {
            return Input.GetKeyDown (KeyCode.LeftShift);
        }
    }

    public bool MaxBlanceButton
    {
        get
        {
            return Input.GetKey (KeyCode.X);
        }
    }

    public bool MaxBlanceButtonUp
    {
        get
        {
            return Input.GetKeyUp (KeyCode.X);
        }
    }

    public bool MaxBlanceButtonDown
    {
        get
        {
            return Input.GetKeyDown (KeyCode.X);
        }
    }
}