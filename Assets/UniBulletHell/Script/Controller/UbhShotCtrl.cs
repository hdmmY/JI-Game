using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;


/// <summary>
/// Ubh shot ctrl.
/// </summary>
[AddComponentMenu("UniBulletHell/Controller/Shot Controller")]
public class UbhShotCtrl : UbhMonoBehaviour
{
    [Serializable]
    public class ShotInfo
    {
        /// <summary>
        /// Set a shot pattern component (inherits UbhBaseShot).
        /// </summary>
        [ValidateInput("NotNull", "Shot Object is not set!")]
        public UbhBaseShot _ShotObj;

        /// <summary>
        /// Set a delay time to starting next shot pattern. (sec)
        /// </summary>
        [CustomValueDrawer("ShowDelayTime")]
        public float _DelayTime;

        #region Inspector Func
        private bool NotNull(UbhBaseShot shotComponent)
        {
            return shotComponent != null;
        }

        private static float ShowDelayTime(float value, GUIContent label)
        {
            value = value < 0 ? 0 : value;
            return UnityEditor.EditorGUILayout.FloatField(label, value);
        }
        #endregion
    }

    /// <summary>
    /// This flag repeats a shot routine.
    /// </summary>
    public bool m_loop = true;

    [ShowIf("m_loop")]
    public bool m_FirstNotInLoop = false;

    [ShowIf("m_loop")]
    [ShowIf("m_FirstNotInLoop")]
    public float m_InitFristEleDelay = 0f;


    /// <summary>
    /// List of shot information. this size is necessary at least 1 or more.
    /// </summary>
    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<ShotInfo> _ShotList = new List<ShotInfo>();

    private float _timer;

    private int _invokeNumber;

    private List<float> _delayTime;

    private void Start()
    {
        _timer = -0.5f;
        _invokeNumber = 0;

        _delayTime = new List<float>(new float[_ShotList.Count]);
        for (int i = 0; i < _delayTime.Count; i++) _delayTime[i] = _ShotList[i]._DelayTime;

        if (m_FirstNotInLoop) _delayTime[0] = m_InitFristEleDelay;
    }

    private void Update()
    {
        _timer += JITimer.Instance.DeltTime;

        while (_invokeNumber < _ShotList.Count && _timer >= _delayTime[_invokeNumber])
        {
            _ShotList[_invokeNumber++]._ShotObj.Shot();
            _timer = 0f;
        }

        if (_invokeNumber >= _ShotList.Count && m_loop)
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