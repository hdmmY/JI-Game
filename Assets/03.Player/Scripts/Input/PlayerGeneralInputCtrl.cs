using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGeneralInputCtrl : IPlayerInputCtrl
{
    private bool GoRight => Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);

    private bool GoLeft => Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);

    private bool GoUp => Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W);

    private bool GoDown => Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S);

    public float VerticalInput ()
    {
        if (GoUp && GoDown) return 0;
        if (GoUp) return 1;
        if (GoDown) return -1;
        return 0;
    }

    public float HorizontalInput ()
    {
        if (GoRight && GoLeft) return 0; 
        if (GoRight) return 1;
        if (GoLeft) return -1;
        return 0;
    }

    public bool ShotButton ()
    {
        return Input.GetKey (KeyCode.Z);
    }

    public bool ShotButtonUp ()
    {
        return Input.GetKeyUp (KeyCode.Z);
    }

    public bool ShotButtonDown ()
    {
        return Input.GetKeyDown (KeyCode.Z);
    }

    public bool ChangeStateButton ()
    {
        return Input.GetKey (KeyCode.LeftShift);
    }

    public bool ChangeStateButtonUp ()
    {
        return Input.GetKeyUp (KeyCode.LeftShift);
    }

    public bool ChangeStateButtonDown ()
    {
        return Input.GetKeyDown (KeyCode.LeftShift);
    }

    public bool MaxBlanceButton ()
    {
        return Input.GetKey (KeyCode.X);
    }

    public bool MaxBlanceButtonUp ()
    {
        return Input.GetKeyUp (KeyCode.X);
    }

    public bool MaxBlanceButtonDown ()
    {
        return Input.GetKeyDown (KeyCode.X);
    }

    public void Update () { }
}