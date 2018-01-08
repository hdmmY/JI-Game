using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HDMToggle : MonoBehaviour
{
    public TimeManager m_timeManager;

    [Button("OnClick", ButtonSizes.Medium)]
    public void OnClick()
    {
        foreach(var timeGameObject in m_timeManager.m_timeGos)
        {
            foreach(var pathControl in timeGameObject.Go.GetComponentsInChildren<JiPathMoveCtrl>())
            {
                foreach(var pathInfo in pathControl.m_Paths)
                {
                    if(pathInfo.m_controlPoints == null)
                    {
                        pathInfo.m_controlPoints = new List<Vector3>();
                    }
                    foreach(var point in pathInfo.m_PathData.m_controlPoints)
                    {
                        pathInfo.m_controlPoints.Add(point);
                    }
                }
            }
        }

    }
}
