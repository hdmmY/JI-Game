using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("UniBulletHell/Shot Pattern/Laser Shot")]
public class LaserShot : UbhBaseShot
{
    // Laser angle
    public float m_angle;

    // Laser width
    public float m_width;

    // Start alpha
    public float m_startAlpha;

    // Laser disappear speed
    public float m_laserDisappearSpeed;

    // Time before laser take damage
    public float m_waitingTime;

    // Time that laser 
    public float m_sustainTime;


    protected override void Awake()
    {
        base.Awake();
    }


    public override void Shot()
    {
        _Shooting = true;
                            
        Quaternion laserRotation = Quaternion.Euler(0, 0, m_angle - 180f);
        var laser = GetBullet(transform.position, laserRotation);
        ShotBullet(laser, ShotCoroutine(laser.transform.GetChild(0)));
        AutoReleaseBulletGameObject(laser.gameObject);

        _Shooting = false;
    }


    IEnumerator ShotCoroutine(Transform laser)
    {
        float laserWidth = m_width;
        float laserAngle = m_angle;
        float startAlpha = m_startAlpha;
        float laserDisappearSpeed = m_laserDisappearSpeed;

        float waitLaserTime = m_waitingTime;
        float sustainTime = m_sustainTime;

        float timer = 0f;

        bool laserAppear = false;

        Vector3 laserScale = laser.transform.localScale;
        laserScale.x = 0.1f;
        laser.transform.localScale = laserScale;

        SpriteRenderer laserSprite = laser.GetComponent<SpriteRenderer>();
        if (laserSprite == null) Debug.LogError("The laser doesn't have sprite component!");
        Color laserColor = laserSprite.color;
        laserColor.a = startAlpha;
        laserSprite.color = laserColor;

        // laser appear
        while (true)
        {
            timer += UbhTimer.Instance.DeltaTime;
            if (timer >= waitLaserTime)
            {
                laserAppear = true;
            }
            if (timer >= waitLaserTime + sustainTime)
            {
                break;
            }

            if (laserAppear)
            {
                laserScale.x = laserWidth;
                laser.transform.localScale = laserScale;

                laserColor.a = 1;
                laserSprite.color = laserColor;
            }
            yield return null;
        }


        // laser disapper
        while (laserColor.a > 0.05f)
        {
            laserColor.a -= UbhTimer.Instance.DeltaTime;
            laserSprite.color = laserColor;
        }

    }

}
