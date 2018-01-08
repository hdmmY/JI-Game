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
        foreach (var timeGameObject in m_timeManager.m_timeGos)
        {
            foreach(var pathControl in timeGameObject.Go.GetComponentsInChildren<JiPathMoveCtrl>(true))
            {
                if (pathControl.m_Paths == null) pathControl.m_Paths = new List<JIPathInfo>();
                pathControl.m_Paths.Clear();

                foreach(var tmpPath in pathControl.m_tmpPath)
                {
                    var path = new JIPathInfo();
                    path.m_controlPoints = new List<Vector3>();
                    foreach(var node in tmpPath.m_controlPoints)
                    {
                        path.m_controlPoints.Add(new Vector3(node.x, node.y, node.z));
                    }
                    path.m_delayTime = tmpPath.m_delayTime;
                    path.m_time = tmpPath.m_time;
                    path.m_easeType = tmpPath.m_easeType;
                    path.m_loopType = tmpPath.m_loopType;
                    path.m_moveTo = tmpPath.m_moveTo;
                    pathControl.m_Paths.Add(path);
                }
            }
        }

    }
}
