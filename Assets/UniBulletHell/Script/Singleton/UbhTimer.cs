using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh timer.
/// </summary>
public class UbhTimer : UbhSingletonMonoBehavior<UbhTimer>
{
    float _lastTime;
    float _deltTime = 1f;
    
    public int FrameCount
    {
        get;
        private set;
    }

    public bool Pause
    {
        get;
        set;
    }

    private float _timeScale;

    // Time scale.
    // 1 is the normal, 0 is the pause.
    public float TimeScale
    {
        get 
        {
            if(Time.timeSinceLevelLoad <= 0.01f)
                _timeScale = 1;
            return _timeScale;    
        }
        set
        {
            if(_timeScale < 0)
            {
                _timeScale = 1;
            }
            else 
            {
                _timeScale = value;
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
            return Pause ? 0f : _deltTime * TimeScale;
        }
    }


    protected override void Awake ()
    {
        _lastTime = Time.time;

        base.Awake();
    }


    void Update ()
    {
        float nowTime = Time.time;
        _deltTime = nowTime - _lastTime;
        _lastTime = nowTime;

        if (!Pause) {
            FrameCount++;
        }
    }
}