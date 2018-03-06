using Sirenix.OdinInspector;
using UnityEngine;

public class JITimer : JISingletonMonoBehavior<JITimer>
{
    // Is the game paused?
    [ShowInInspector, ReadOnly]
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
            else
                _timeScale = 1;
        }
    }

    // Time scale.
    // TimeScale = 0 means pause; TimeScale = 1 means normal
    private float _timeScale;

    [ShowInInspector, ReadOnly]
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

    [ShowInInspector, ReadOnly]
    public float DeltTime
    {
        get
        {
            return _deltTime * TimeScale;
        }
    }

    [ShowInInspector, ReadOnly]
    // Ignore the TimeScale
    public float RealDeltTime
    {
        get
        {
            return _deltTime;
        }
    }

    /// <summary>
    /// Total Frame Count.Not affect by JITimer.Instance.TimeScale
    /// </summary>
    [ShowInInspector, ReadOnly]
    public float FrameCount
    {
        get;
        private set;
    }

    private float _lastTime;
    protected override void Start ()
    {
        base.Start ();

        _timeScale = 1;
        _lastTime = Time.time;
    }

    private void Update ()
    {
        float nowTime = Time.time;

        _deltTime = nowTime - _lastTime;
        _lastTime = nowTime;

        if (!Pause)
        {
            FrameCount++;
        }
    }
}