using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    public InputManager m_InputManager;

    public BulletPool m_BulletPool;

    public Transform m_SpriteReference;

    public List<ShootPoint> m_ShootTransform;

    public Bullet_Property m_BlackBulletProperty;
    public Bullet_Property m_WhiteBulletProperty;
}

[System.Serializable]
public class ShootPoint
{
    public string name;
    public Transform transform;
}
