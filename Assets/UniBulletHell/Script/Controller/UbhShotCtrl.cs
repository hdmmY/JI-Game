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

    public bool m_FirstNotInLoop = false;
    public float m_InitFristEleDelay = 0f;

    // "List of shot information. this size is necessary at least 1 or more."
    public List<ShotInfo> _ShotList = new List<ShotInfo>();

    private float _timer;    
    
    private int _invokeNumber;

    private List<float> _delayTime;

    private void Start ()
    {
        _timer = 0f;
        _invokeNumber = 0;

        _delayTime = new List<float>(new float[_ShotList.Count]);
        for (int i = 0; i < _delayTime.Count; i++) _delayTime[i] = _ShotList[i]._DelayTime;

        if (m_FirstNotInLoop) _delayTime[0] = m_InitFristEleDelay;
    }

    private void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        while(_invokeNumber < _ShotList.Count && _timer >= _delayTime[_invokeNumber])
        {
            _ShotList[_invokeNumber++]._ShotObj.Shot();
        }

        if(_invokeNumber < _ShotList.Count)
        {
            ResetShot();
        }

    }

    private void ResetShot()
    {
        _timer = 0f;
        _invokeNumber = 0;

        _delayTime[0] = m_InitFristEleDelay;
    }

}