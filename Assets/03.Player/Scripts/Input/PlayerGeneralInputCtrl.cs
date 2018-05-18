using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class PlayerGeneralInputCtrl : IPlayerInputCtrl
{
    private bool GoRight => Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);

    private bool GoLeft => Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);

    private bool GoUp => Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W);

    private bool GoDown => Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S);

    [ShowInInspector]
    public float VerticalInput
    {
        get
        {
            if (GoUp && GoDown) return 0;
            if (GoUp) return 1;
            if (GoDown) return -1;
            return 0;
        }
    }

    [ShowInInspector]
    public float HorizontalInput
    {
        get
        {
            if (GoRight && GoLeft) return 0;
            if (GoRight) return 1;
            if (GoLeft) return -1;
            return 0;
        }
    }

    [ShowInInspector]
    public bool ShotButton
    {
        get
        {
            return Input.GetKey (KeyCode.Z);
        }
    }

    [ShowInInspector]
    public bool ShotButtonUp
    {
        get
        {
            return Input.GetKeyUp (KeyCode.Z);
        }
    }

    [ShowInInspector]
    public bool ShotButtonDown
    {
        get
        {
            return Input.GetKeyDown (KeyCode.Z);
        }
    }

    [ShowInInspector]
    public bool ChangeStateButton
    {
        get
        {
            return Input.GetKey (KeyCode.LeftShift);
        }
    }

    [ShowInInspector]
    public bool ChangeStateButtonUp
    {
        get
        {
            return Input.GetKeyUp (KeyCode.LeftShift);
        }
    }

    [ShowInInspector]
    public bool ChangeStateButtonDown
    {
        get
        {
            return Input.GetKeyDown (KeyCode.LeftShift);
        }
    }

    [ShowInInspector]
    public bool MaxBlanceButton
    {
        get
        {
            return Input.GetKey (KeyCode.X);
        }
    }

    [ShowInInspector]
    public bool MaxBlanceButtonUp
    {
        get
        {
            return Input.GetKeyUp (KeyCode.X);
        }

    }

    [ShowInInspector]
    public bool MaxBlanceButtonDown
    {
        get
        {
            return Input.GetKeyDown (KeyCode.X);
        }
    }

    public void Update ()
    {

    }
}