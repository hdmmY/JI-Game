using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh timer.
/// </summary>
public class UbhTimer : UbhSingletonMonoBehavior<UbhTimer>
{
    float _LastTime;
    float _DeltaTime = 1f;
    float _TimeScale;
    float _FrameCount;
    bool _Pausing;


    // Time scale.
    // 1 is the normal, 0 is the pause.
    public float TimeScale
    {
        get 
        {
            if(Time.timeSinceLevelLoad <= 0.01f)
                _TimeScale = 1;
            return _TimeScale;    
        }
        set
        {
            if(_TimeScale < 0)
            {
                _TimeScale = 1;
            }
            else 
            {
                _TimeScale = value;
            }
        }
    }

    /// <summary>
    /// Get delta time of UniBulletHell.
    /// </summary>
    public float DeltaTime
    {
        get
        {
            return _Pausing ? 0f : _DeltaTime * _TimeScale;
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