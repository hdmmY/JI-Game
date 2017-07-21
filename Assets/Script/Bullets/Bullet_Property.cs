using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Property : MonoBehaviour
{
    // bullet attrack 
    public bool m_useBulletAttrack = BulletPropertyDefault.UseBulletAttrack;
    public float m_attrackFactor = BulletPropertyDefault.AttrackFactor;
    
    // bullet reject
    public bool m_useBulletReject = BulletPropertyDefault.UseBulletReject;
    public float m_rejectFactor = BulletPropertyDefault.RejectFactor;

    // attrack or reject target
    public Transform m_targetTrans = BulletPropertyDefault.TargetTrans;

    // bullet damage
    public int m_BulletDamage;

    // for debug use
    public Vector2 m_Velocity = Vector3.zero;

    /// <summary>
    /// The time of a bullet life
    /// </summary>
    [Range(0.05f, 20f)]
    public float m_LifeTime = BulletPropertyDefault.LifeTime;

    /// <summary>
    /// The color of the bullet
    /// </summary>
    public Color m_BulletColor = BulletPropertyDefault.Color;

    /// <summary>
    /// The alpha of the bullet
    /// </summary>
    [Range(0, 1)]
    public float m_Alpha = BulletPropertyDefault.Alpha;

    /// <summary>
    /// The direction of the bullet mesh
    /// </summary>
    [Range(0, 360)]
    public int m_SpriteDirection = BulletPropertyDefault.SpriteDirection;

    /// <summary>
    /// Wether the bullet rotation align with the velocity direction
    /// </summary>
    public bool m_AlignWithVelocity = BulletPropertyDefault.AlignWithVelocity;

    /// <summary>
    /// The bullet speed
    /// </summary>
    public float m_BulletSpeed = BulletPropertyDefault.BulletSpeed;

    /// <summary>
    /// The bullet acceleration
    /// </summary>
    public float m_Accelerate = BulletPropertyDefault.Accelerator;

    /// <summary>
    /// The angle of the acceleration direction.  
    /// ( (1, 0, 0) represents the angle 0', (1, 1, 0) represents the angle 45')
    /// </summary>
    [Range(0, 365)]
    public int m_AcceleratDir = BulletPropertyDefault.AcceleratorDirection;

    public float m_HorizontalVelocityFactor = BulletPropertyDefault.HorizontalVelocityFactor;

    public float m_VerticalVelocityFactor = BulletPropertyDefault.VerticalVelocityFactor;

    public float m_CurTime = 0f;


    /// <summary>
    /// copy other bulletproperty component to this bulletproperty component
    /// </summary>
    /// <param name="_property"></param>
    public void CopyProperty(Bullet_Property _property)
    {
        m_useBulletAttrack = _property.m_useBulletAttrack;
        m_attrackFactor = _property.m_attrackFactor;

        m_useBulletReject = _property.m_useBulletReject;
        m_rejectFactor = _property.m_rejectFactor;

        m_BulletDamage = _property.m_BulletDamage;

        m_targetTrans = _property.m_targetTrans;
        m_Velocity = _property.m_Velocity;

        m_LifeTime = _property.m_LifeTime;
        m_BulletColor = _property.m_BulletColor;
        m_Alpha = _property.m_Alpha;
        m_SpriteDirection = _property.m_SpriteDirection;
        m_AlignWithVelocity = _property.m_AlignWithVelocity;
        m_BulletSpeed = _property.m_BulletSpeed;
        m_Accelerate = _property.m_Accelerate;
        m_AcceleratDir = _property.m_AcceleratDir;

        m_HorizontalVelocityFactor = _property.m_HorizontalVelocityFactor;
        m_VerticalVelocityFactor = _property.m_VerticalVelocityFactor;
        //m_CurTime = _property.m_CurTime;
    }

}




public static class BulletPropertyDefault
{
    public readonly static float LifeTime = 10f;

    public readonly static Color Color = Color.black;

    public readonly static float Alpha = 1f;

    public readonly static int SpriteDirection = 0;

    public readonly static bool AlignWithVelocity = true;

    public readonly static float BulletSpeed = 5f;

    public readonly static float Accelerator = 0;

    public readonly static int AcceleratorDirection = 0;

    public readonly static float HorizontalVelocityFactor = 1;

    public readonly static float VerticalVelocityFactor = 1;

    public readonly static bool UseBulletAttrack = false;

    public readonly static bool UseBulletReject = false;

    public readonly static float AttrackFactor = 5f;

    public readonly static float RejectFactor = 5f;

    public readonly static Transform TargetTrans = null;

    public readonly static int BulletDamage = 100;
}
