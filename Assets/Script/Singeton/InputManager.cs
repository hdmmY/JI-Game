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
        _HorizontalInput = Input.GetAxis("Horizontal");
        _VerticalInput = Input.GetAxis("Vertical");
    }

}
