using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh timer.
/// </summary>
public class UbhTimer : UbhSingletonMonoBehavior<UbhTimer>
{
    float _LastTime;
    float _DeltaTime;
    float _FrameCount;
    bool _Pausing;

    /// <summary>
    /// Get delta time of UniBulletHell.
    /// </summary>
    public float DeltaTime
    {
        get
        {
            return _Pausing ? 0f : _DeltaTime;
        }
    }

    /// <summary>
    /// Get frame count of UniBulletHell.
    /// </summary>
    public float FrameCount
    {
        get
        {
            return _FrameCount;
        }
    }

    protected override void Awake ()
    {
        _LastTime = Time.time;

        base.Awake();
    }

    void Update ()
    {
        float nowTime = Time.time;
        _DeltaTime = nowTime - _LastTime;
        _LastTime = nowTime;

        if (_Pausing == false) {
            _FrameCount++;
        }
    }

    /// <summary>
    /// Pause time of UniBulletHell.
    /// </summary>
    public void Pause ()
    {
        _Pausing = true;
    }

    /// <summary>
    /// Resume time of UniBulletHell.
    /// </summary>
    public void Resume ()
    {
        _Pausing = false;
        _LastTime = Time.time;
    }
}