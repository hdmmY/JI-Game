using UnityEngine;

public class InputManager : MonoBehaviour {

    static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();

                if(_instance == null)
                    _instance = new GameObject("InputManager").AddComponent<InputManager>();
            }
            return _instance;
        }
    }

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
        if(Input.GetKey(KeyCode.RightArrow))  // 玩家想要向右移动
        {
            _HorizontalInput = 1;
        } 

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            _HorizontalInput = -1;
        }

        if(!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            _HorizontalInput = 0;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            _VerticalInput = 1;
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            _VerticalInput = -1;
        }

        if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            _VerticalInput = 0;
        }
    }

}
