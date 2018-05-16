using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerProperty))]
public class PlayerPointManager : MonoBehaviour
{
    // private PlayerProperty _player;

    // private void OnEnable ()
    // {
    //     _player = GetComponent<PlayerProperty> ();

    //     EventManager.Instance.AddListener<EnemyHurtedEvent> (OnEnemyHurted);
    // }

    // private void OnDisable ()
    // {
    //     EventManager.Instance.RemoveListener<EnemyHurtedEvent> (OnEnemyHurted);
    // }

    // private void OnEnemyHurted (EnemyHurtedEvent enemyHurtEvent)
    // {
    //     // if (_player.m_superState)
    //     // {
    //     //     return;
    //     // }

    //     // switch (enemyHurtEvent.PlayerBulletState)
    //     // {
    //     //     case JIState.All:
    //     //         _player.m_playerBlackPoint += _player.m_addValue;
    //     //         _player.m_playerWhitePoint += _player.m_addValue;
    //     //         break;
    //     //     case JIState.Black:
    //     //         _player.m_playerBlackPoint += _player.m_addValue;
    //     //         break;
    //     //     case JIState.White:
    //     //         _player.m_playerWhitePoint += _player.m_addValue;
    //     //         break;
    //     // }

    //     // _player.m_playerBlackPoint = Mathf.Clamp (_player.m_playerBlackPoint, 0, _player.m_maxPlayerPoint);
    //     // _player.m_playerWhitePoint = Mathf.Clamp (_player.m_playerWhitePoint, 0, _player.m_maxPlayerPoint);
    // }
}