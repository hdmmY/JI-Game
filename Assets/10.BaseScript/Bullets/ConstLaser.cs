using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstLaser : Laser
{
    public override LaserType LaserType => LaserType.Constant;

    public void OnAnimEnd ()
    {
        Destroy (this.gameObject);
    }

    protected override void Update ()
    {
        base.Update ();

        UpdateLaserAppear ();
    }
}