using System.Collections.Generic;
using UnityEngine;

public class JiPath : MonoBehaviour {

    // Eash path name that identify the path
	public string m_PathName = "path";

    // Control nodes of the path, determine the path shape.
    public List<Vector3> m_CtrolNode = new List<Vector3>();
    
    // The Control node cout, it must greater or equal than 2.
    public int m_CtrolNodeCount = 2;

    // Path is visable even when you not select the object.
    public bool m_alwaysVisable = false;

    //public bool m_revearse = false;

    void OnDrawGizmosSelected()
    {
        if(!m_alwaysVisable)
        {
            if(m_CtrolNode.Count >= 2)
                iTween.DrawPath(m_CtrolNode.ToArray());
        }
    }   

    void OnDrawGizmos()
    {
        if(m_alwaysVisable)
        {
            if(m_CtrolNode.Count >= 2)
                iTween.DrawPath(m_CtrolNode.ToArray());
        }
    }

}
