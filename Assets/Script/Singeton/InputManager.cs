using UnityEngine;

public class InputManager : JISingletonMonoBehavior<InputManager>
{
    private float _HorizontalInput;
    private float _VerticalInput;

    public float HorizontalInput
    {
        get
        {
            return _HorizontalInput;
        }
    }

    public float VerticalInput
    {
        get
        {
            return _VerticalInput;
        }
    }

    private void Update()
    {
        bool goRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        bool goLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool goUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        bool goDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        if (goRight)  
        {
            _HorizontalInput = 1;
        }
        else if (goLeft)
        {
            _HorizontalInput = -1;
        }
        else
        {
            _HorizontalInput = 0;
        }

        if (goUp)
        {
            _VerticalInput = 1;
        }
        else if (goDown)
        {
            _VerticalInput = -1;
        }
        else
        {
            _VerticalInput = 0;
        }
    }

}
