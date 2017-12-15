using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JIGlobalRef
{
    private static PlayerProperty _player;
    public static PlayerProperty Player
    {
        get
        {
            if(_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();
            }

            if(_player == null)
            {
                Debug.LogWarning("There is no player!");
            }

            return _player;
        }
    }


    private static Camera _mainCamera;
    public static Camera MainCamera
    {
        get
        {
            if(_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }

            if(_mainCamera == null)
            {
                Debug.LogWarning("There is no main camera!");
            }

            return _mainCamera;
        }
    }

}
