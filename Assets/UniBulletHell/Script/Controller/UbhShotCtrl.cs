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

    [HideInInspector] public bool m_FirstNotInLoop = false;
    [HideInInspector] public float m_InitFristEleDelay = 0f;

    // "This flag repeats a shot routine."
    [HideInInspector] public bool m_loop = true;


    // "List of shot information. this size is necessary at least 1 or more."
    public List<ShotInfo> _ShotList = new List<ShotInfo>();

    private float _timer;    
    
    private int _invokeNumber;

    private List<float> _delayTime;

    private void Start ()
    {
        _timer = -0.5f;
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
            _timer = 0f;
        }

        if(_invokeNumber >= _ShotList.Count && m_loop)
        {
            ResetShot();
        }

    }

    private void ResetShot()
    {
        _invokeNumber = 0;

        _delayTime[0] = _ShotList[0]._DelayTime;
    }

}