using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEventMaster: MonoBehaviour
{
    // call the event
    static public void CallOnPlayerStateChangeEvent(PlayerProperty.PlayerStateType prevState)
    {
        if (PlayerProperty.OnPlayerStateChangeEvent != null)
        {
            PlayerProperty.OnPlayerStateChangeEvent(prevState);
        }
    }



    static public void CallPlayerShootEvent(GameObject bullet)
    {
        if(PlayerProperty.PlayerShootEvent != null)
        {
            PlayerProperty.PlayerShootEvent(bullet);
        }
    }

}
