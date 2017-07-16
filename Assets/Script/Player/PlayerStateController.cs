using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerProperty))]
[RequireComponent(typeof(PlayerReference))]
public class PlayerStateController : MonoBehaviour
{
    private InputManager _inputManager;

    private PlayerProperty _playerProperty;
    private PlayerReference _playerReference;
    private PlayerEventMaster _playerEventMaster;

    private SpriteRenderer _playerSpriteRender;

    void OnEnable()
    {
        SetInitReference();


        // event
        _inputManager.ChangeStateKeyPressEvent += OnChangePressKeyPressed;
        _playerEventMaster.OnPlayerStateChangeEvent += ChangePlayerSprite;
    }


    private void OnDisable()
    {
        _inputManager.ChangeStateKeyPressEvent -= OnChangePressKeyPressed;
        _playerEventMaster.OnPlayerStateChangeEvent -= ChangePlayerSprite;
    }


    void SetInitReference()
    {
        _inputManager = GetComponent<PlayerReference>().m_InputManager;

        _playerProperty = GetComponent<PlayerProperty>();
        _playerReference = GetComponent<PlayerReference>();
        _playerEventMaster = GetComponent<PlayerEventMaster>();

        _playerSpriteRender = _playerReference.m_SpriteReference.GetComponent<SpriteRenderer>();
    }


    void OnChangePressKeyPressed()
    {
        PlayerProperty.PlayerStateType prevState = _playerProperty.m_playerState;
        PlayerProperty.PlayerStateType afterState = _playerProperty.m_playerState == PlayerProperty.PlayerStateType.Black ? 
                        PlayerProperty.PlayerStateType.White : PlayerProperty.PlayerStateType.Black;

        _playerProperty.m_playerState = afterState;

        _playerEventMaster.CallOnPlayerStateChangeEvent(prevState);        
    }


    void ChangePlayerSprite(PlayerProperty.PlayerStateType prevState)
    {
        switch (prevState)
        {
            case PlayerProperty.PlayerStateType.Black:
                _playerSpriteRender.sprite = _playerProperty.m_WhiteSprite;
                break;

            case PlayerProperty.PlayerStateType.White:
                _playerSpriteRender.sprite = _playerProperty.m_BlackSprite;
                break;
        }
    }
    
}
