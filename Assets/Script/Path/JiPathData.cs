using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class JiPathData : ScriptableObject
{
    public string m_pathName;

    public List<Vector3> m_controlPoints;

    public JiPathData(string name, List<Vector3> controlPoint)
    {
        m_pathName = name;
        m_controlPoints = controlPoint;
    }
}
