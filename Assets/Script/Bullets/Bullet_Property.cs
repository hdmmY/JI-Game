using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Property : MonoBehaviour
{

    /// <summary>
    /// The time of a bullet life
    /// </summary>
    [Range(0.05f, 20f)]
    public float m_LifeTime;

    /// <summary>
    /// The color of the bullet
    /// </summary>
    public Color m_BulletColor;

    /// <summary>
    /// The alpha of the bullet
    /// </summary>
    [Range(0, 1)]
    public float m_Alpha = 1;

    /// <summary>
    /// The direction of the bullet mesh
    /// </summary>
    [Range(0, 360)]
    public int m_SpriteDirection = 0;

    /// <summary>
    /// Wether the bullet rotation align with the velocity direction
    /// </summary>
    public bool m_AlignWithVelocity = true;

    /// <summary>
    /// The bullet speed
    /// </summary>
    public float m_BulletSpeed = 2;

    /// <summary>
    /// The bullet acceleration
    /// </summary>
    public float m_Accelerate = 0;

    /// <summary>
    /// The angle of the acceleration direction.  
    /// ( (1, 0, 0) represents the angle 0', (1, 1, 0) represents the angle 45')
    /// </summary>
    [Range(0, 365)]
    public int m_AcceleratDir = 0;
    
    public float m_HorizontalVelocityFactor = 1;

    public float m_VerticalVelocityFactor = 1;

    public float m_CurTime;
}
