using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBulletMoveCtrl : BaseBulletMoveCtrl
{
    public float Angle = 90f;
    public float Speed = 0f;
    public float Accelerate = 0f;
    public float AngleSpeed = 0f;
    public float AngleAccel = 0f;

    [Space]
    public bool Homing = false;
    public Transform HomeTarget = null;
    public float HomeAngleSpeed = 0f;
    public float MaxHomeAngle = 0f;

    [Space]
    public bool Pause = false;
    public float PauseTime = 0f;
    public float ResumeTime = 0f;

    public override void Init ()
    {
        _angle = Angle - 90f;
        _homeAngle = 0f;
        _selfTimeCount = 0f;

        transform.SetEulerAnglesZ (_angle);

        _initialized = true;
    }

    #region  Private Variables and Methods
    private float _angle;
    private float _homeAngle;
    private float _selfTimeCount;
    private bool _initialized;

    private void OnDisable ()
    {
        _initialized = false;
    }

    private void Update ()
    {
        if (!_initialized) return;

        _bulletMove.Speed = Speed;
        _bulletMove.Accelerate = Accelerate;
        _selfTimeCount += JITimer.Instance.DeltTime;

        if (Homing)
        {
            if (HomeTarget != null && HomeAngleSpeed > 0)
            {
                float rotateAngle = UbhUtil.GetAngleFromTwoPosition (transform, HomeTarget) - 90;
                float myAngle = transform.eulerAngles.z;
                float toAngle = Mathf.MoveTowardsAngle (myAngle, rotateAngle,
                    JITimer.Instance.DeltTime * HomeAngleSpeed);
                float angleSpeed = JITimer.Instance.DeltTime == 0f ? 0f :
                    Mathf.Abs (toAngle - myAngle) / JITimer.Instance.DeltTime;

                _homeAngle += angleSpeed * JITimer.Instance.DeltTime;
                if (_homeAngle <= MaxHomeAngle)
                {
                    _bulletMove.AngleSpeed = angleSpeed;
                }
            }
        }
        else
        {
            _bulletMove.AngleSpeed = AngleSpeed;
        }

        if (Pause && PauseTime > 0 && ResumeTime > PauseTime)
        {
            if (PauseTime < _selfTimeCount && _selfTimeCount < ResumeTime)
            {
                _bulletMove.Stop = true;
            }
            else
            {
                _bulletMove.Stop = false;
            }
        }
    }
    
    #endregion
}