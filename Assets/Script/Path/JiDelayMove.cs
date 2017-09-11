using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The class is to delay a JiPathMoveCtrl component.
public class JiDelayMove : MonoBehaviour 
{
    public JiPathMoveCtrl m_JiPathMoveCtrlScript;

    // The delay time for Invoke JiPathMoveCtrl component.
    public float m_delayTime = 0.1f;

    private float _timer;

	private void OnEnable()
    {
        if(m_JiPathMoveCtrlScript == null)
        {
            m_JiPathMoveCtrlScript = GetComponent<JiPathMoveCtrl>();
            if(m_JiPathMoveCtrlScript == null)
            {
                Debug.LogError("The JiPathMoveCtrl is not set!");
                return;
            }            
        }

        m_JiPathMoveCtrlScript.enabled = false;

        if(m_delayTime <= 0)
            m_delayTime = 0.1f;

        _timer = 0f;
    }

    private void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        if(_timer >= m_delayTime)
        {
            m_JiPathMoveCtrlScript.enabled = true;
            this.enabled = false;
        }
    }
}
