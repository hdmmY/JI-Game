using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
public class BulletDestroyBound : Singleton<BulletDestroyBound>
{
    /// <summary>
    /// Global bullet destroy bound center
    /// </summary>
    /// <returns></returns>
    [ShowInInspector]
    public Vector2 Center
    {
        get { return _center; }
        set { _center = value; InitCollider (); }
    }

    /// <summary>
    /// Global bullet destroy bound size 
    /// </summary>
    [ShowInInspector]
    public Vector2 Size
    {
        get { return _size; }
        set
        {
            _size = new Vector2 (Mathf.Abs (value.x), Mathf.Abs (value.y));
            InitCollider ();
        }
    }

    [SerializeField, HideInInspector] private Vector2 _center;

    [SerializeField, HideInInspector] private Vector2 _size;

    [SerializeField] private BoxCollider2D _upperBounds;

    [SerializeField] private BoxCollider2D _underBounds;

    [SerializeField] private BoxCollider2D _leftBounds;

    [SerializeField] private BoxCollider2D _rightBounds;

    public float Up => Center.y + Size.y;
    public float Down => Center.y - Size.y;
    public float Right => Center.x + Size.x;
    public float Left => Center.x - Size.x;

    private void OnEnable ()
    {
        InitCollider ();
    }

    private void InitCollider ()
    {
        if (!_rightBounds || !_leftBounds || !_upperBounds || !_underBounds)
            return;

        Action<BoxCollider2D, Vector2, Vector2> setColliderBounds = (col, offset, size) =>
        {
            col.offset = offset;
            col.size = size;
            col.tag = tag;
        };

        setColliderBounds (_upperBounds,
            new Vector2 (0, Up * 1.5f), new Vector2 (2 * Size.x + 0.2f, Size.y));
        setColliderBounds (_underBounds,
            new Vector2 (0, Down * 1.5f), new Vector2 (2 * Size.x + 0.2f, Size.y));
        setColliderBounds (_rightBounds,
            new Vector2 (Right * 1.5f, 0), new Vector2 (Size.x, 2 * Size.y + 0.2f));
        setColliderBounds (_leftBounds,
            new Vector2 (Left * 1.5f, 0), new Vector2 (Size.x, 2 * Size.y + 0.2f));
    }

    /// <summary>
    /// Whether a given position is out of the bound
    /// </summary>
    public bool OutBound (float x, float y)
    {
        if (x > Right || x < Left)
        {
            return true;
        }

        if (y > Up || y < Down)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Whether a given position is out of the bound
    /// </summary>
    public bool OutBound (Vector3 position)
    {
        return OutBound (position.x, position.y);;
    }
}