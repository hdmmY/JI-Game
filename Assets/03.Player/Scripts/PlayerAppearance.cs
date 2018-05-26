using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerProperty))]
public class PlayerAppearance : MonoBehaviour
{
    public Sprite PlayerBlackSprite;

    public Sprite PlayerWhiteSprite;

    private PlayerProperty _player;

    private SpriteRenderer _playerSprite;

    private void OnEnable ()
    {
        _player = GetComponent<PlayerProperty> ();
    }

    private void Update ()
    {
        _playerSprite = _player.m_spriteReference;

        if (_playerSprite == null) return;

        if (_player.m_playerState == JIState.Black)
        {
            _playerSprite
            .sprite = PlayerBlackSprite;
        }
        else if (_player.m_playerState == JIState.White)
        {
            _playerSprite.sprite = PlayerWhiteSprite;
        }
        else
        {
            _playerSprite.sprite = null;
        }
    }
}