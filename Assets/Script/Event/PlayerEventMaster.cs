using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventMaster : BaseEventMaster
{
    public System.Action<JIBulletController> OnShot;

    public void CallOnShot(JIBulletController bullet)
    {
        if(OnShot != null)
        {
            OnShot(bullet);
        }
    }
    
}
