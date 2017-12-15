﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JITimer : JISingletonMonoBehavior<JITimer>
{
    // Is the game paused?
    public bool Pause
    {
        get
        {
            return _timeScale == 0;
        }             
        set
        {
            if (value)
                _timeScale = 0;    
        }
    }


    // Time scale.
    // TimeScale = 0 means pause; TimeScale = 1 means normal
    private float _timeScale;
    public float TimeScale
    {
        get
        {
            return _timeScale;    
        }
        set
        {
            _timeScale = value < 0 ? 0 : value;
        }
    }


    private float _deltTime;
    public float DeltTime
    {
        get
        {
            return _deltTime * TimeScale;
        }
    }


    // Ignore the TimeScale
    public float RealDeltTime
    {
        get
        {
            return _deltTime;
        }
    }



    private float _lastTime;
    protected override void Start()
    {
        base.Start();

        _timeScale = 1;
        _lastTime = Time.time;
    }


    private void Update()
    {
        float nowTime = Time.time;

        _deltTime = nowTime - _lastTime;
        _lastTime = nowTime;
    }                   
}