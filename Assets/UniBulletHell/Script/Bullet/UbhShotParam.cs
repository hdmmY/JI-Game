using UnityEngine;

public class UbhShotParam : UbhMonoBehaviour
{

    // This is the bullet speed.
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }


    // This is the bullet angle speed
    public float AngleSpeed
    {
        get
        {
            return _angleSpeed;
        }
        set
        {
            _angleSpeed = value;
        }
    }

    // This is the exist time of the AngleSpeed.
    public float AngleSpeedTime
    {
        get
        {
            return _angleSpeedTime;
        }
        set
        {
            _angleSpeedTime = value < 0 ? 0 : value;
        }
    }


    // This is the bullet inital angle. 
    public float Angle
    {
        get
        {
            return _angle;
        }
        set
        {
            _angle = UbhUtil.Get360Angle(value);
        }
    }

    //  This is the bullet accelerated speed.
    public float AccelSpeed
    {
        get
        {
            return _accelSpeed;
        }
        set
        {
            _accelSpeed = value;
        }
    }


    // This is the bullet accelerated time.
    public float AccelTime
    {
        get
        {
            return _accelTime;
        }
        set
        {
            _accelTime = value < 0 ? 0 : value;
        }
    }


    // This is the bullet AngleSpeed accelerated speed.
    public float AccelTurning
    {
        get
        {
            return _accelTurning;
        }
        set
        {
            _accelTurning = value;
        }
    }


    // This is the exist time of AccelTurning.
    public float AccelTurningTime
    {
        get
        {
            return _accelTurningTime;
        }
        set
        {
            _accelTurningTime = value < 0 ? 0 : value;
        }
    }
    

    // The bullet can homging to the target
    public bool Homing
    {
        get
        {
            return _homing;
        }
        set
        {
            _homing = value;
        }
    }
    

    // The bullet homing target.
    public Transform HomingTarget
    {
        get
        {
            return _homingTarget;
        }
        set
        {
            _homingTarget = value;
        }
    }

    
    // The bullet homing angle speed. Positive means anticlockwise, negative means clockwise.
    public float HomingAngleSpeed
    {
        get
        {
            return _homingAngleSpeed;
        }  
        set
        {
            _homingAngleSpeed = value;
        }
    }

    // Bullet total homing angle is limited by the MaxHomgingAngle
    public float MaxHomgingAngle
    {
        get
        {
            return _maxHomingAngle;
        }
        set
        {
            _maxHomingAngle = value <= 0 ? 0 : value;
        }
    }


    // 
    public bool Wave
    {
        get
        {
            return _wave;
        }
        set
        {
            _wave = value;
        }
    }


    //
    public float WaveSpeed
    {
        get
        {
            return _waveSpeed;
        }
        set
        {
            _waveSpeed = value;
        }
    }


    public float WaveAngleSize
    {
        get
        {
            return _waveAngleSize;
        }
        set
        {
            _waveAngleSize = value;
        }
    }


    // The bullet movement can be pause and resume
    public bool PauseAndResume
    {
        get
        {
            return _pauseAndResume;
        }
        set
        {
            _pauseAndResume = value;
        }
    }

    // The bullet movement will be pause at the specific PauseTime.
    public float PauseTime
    {
        get
        {
            return _pauseTime;
        }
        set
        {
            _pauseTime = value < 0 ? 0f : value;
        }
    }


    // The bullet movement will be resumed at the specific ResumeTime.
    public float ResumeTIme
    {
        get
        {
            return _resumeTime;
        }
        set
        {
            _resumeTime = value < 0 ? 0f : value;
        }
    }


    // X-Y Or X-Z axis movement
    public UbhUtil.AXIS AxisMove
    {
        get
        {
            return _axisMove;
        }
        set
        {
            _axisMove = value;
        } 
    }

    

    float _speed;
    float _angleSpeed;
    float _angleSpeedTime;
    float _angle;

    float _accelSpeed;
    float _accelTime;
    float _accelTurning;
    float _accelTurningTime;

    bool _homing;
    Transform _homingTarget;
    float _homingAngleSpeed;
    float _maxHomingAngle;

    bool _wave;
    float _waveSpeed;
    float _waveAngleSize;

    bool _pauseAndResume;
    float _pauseTime;
    float _resumeTime;

    UbhUtil.AXIS _axisMove;    
}
