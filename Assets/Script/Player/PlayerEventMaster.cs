using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEventMaster: MonoBehaviour
{
    // event 
    public delegate void OnPlayerStateChangeDelegate (PlayerProperty.PlayerStateType prevState);
    public OnPlayerStateChangeDelegate OnPlayerStateChangeEvent;


    public delegate void PlayerShootDelegate(GameObject bullet);
    public PlayerShootDelegate PlayerShootEvent;


    // call the event
    public void CallOnPlayerStateChangeEvent(PlayerProperty.PlayerStateType prevState)
    {
        if (OnPlayerStateChangeEvent != null)
        {
            OnPlayerStateChangeEvent(prevState);
        }
    }



    public void CallPlayerShootEvent(GameObject bullet)
    {
        if(PlayerShootEvent != null)
        {
            PlayerShootEvent(bullet);
        }
    }

}
