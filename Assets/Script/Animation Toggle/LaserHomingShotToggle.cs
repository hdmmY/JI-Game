using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Disable homing when the laser has been shot
public class LaserHomingShotToggle : MonoBehaviour
{
    public AnimationToggle.AimingTarget m_aimingTargetCompo;
    public UbhBaseShot m_baseShotComp;

    private void Start()
    {
        m_baseShotComp.OnShotFinish += ShotDownAiming;
    }

    private void OnDisable()
    {
        m_baseShotComp.OnShotFinish -= ShotDownAiming;
    }


    private void ShotDownAiming(UbhBaseShot baseShot)
    {
        m_aimingTargetCompo.m_aiming = false;      
    }




}
