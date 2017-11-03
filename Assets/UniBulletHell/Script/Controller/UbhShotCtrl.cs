using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Ubh shot ctrl.
/// </summary>
[AddComponentMenu("UniBulletHell/Controller/Shot Controller")]
public class UbhShotCtrl : UbhMonoBehaviour
{
    [Serializable]
    public class ShotInfo
    {
        // "Set a delay time to starting next shot pattern. (sec)"
        public float _DelayTime;

        // "Set a shot pattern component (inherits UbhBaseShot)."
        public UbhBaseShot _ShotObj;
    }

    // "Axis on bullet move."
    [HideInInspector]
    public UbhUtil.AXIS m_AxisMove = UbhUtil.AXIS.X_AND_Y;
    
    // "This flag repeats a shot routine."
    public bool m_loop = true;

    public bool m_isFirstInLoop = false;

    // "List of shot information. this size is necessary at least 1 or more."
    public List<ShotInfo> _ShotList = new List<ShotInfo>();

    private float _timer;
    private List<float> _shotInvokeTime;
    private List<bool> _isInvoked;
    private int _invokeNumber;


    private void Start ()
    {
        _timer = 0f;
        _invokeNumber = 0;

        _shotInvokeTime = new List<float>(_ShotList.Count);
        _shotInvokeTime.Add(_ShotList[0]._DelayTime + 0.1f);
        for(int i = 1; i < _ShotList.Count; i++)
        {
            _shotInvokeTime.Add(_shotInvokeTime[i - 1] + _ShotList[i]._DelayTime);
        }

        _isInvoked = new List<bool>(_ShotList.Count);
        for (int i = 0; i < _ShotList.Count; i++) _isInvoked.Add(false);
    }

    private void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        // can be invoke and haven't been invoke
        if(_invokeNumber < _ShotList.Count && 
           _timer >= _shotInvokeTime[_invokeNumber] &&
           !_isInvoked[_invokeNumber])
        {
            _ShotList[_invokeNumber]._ShotObj.Shot();
            _isInvoked[_invokeNumber] = true;
            _invokeNumber++;
        }

        // all shot has been invoked
        if(!_isInvoked.Contains(false) && m_loop)
        {
            ResetShot();
        }

    }

    private void ResetShot()
    {
        _timer = 0f;

        if (m_isFirstInLoop)
            _invokeNumber = 1;
        else
            _invokeNumber = 0;

        for (int i = 0; i < _ShotList.Count; i++) _isInvoked[i] = false;
    }

}