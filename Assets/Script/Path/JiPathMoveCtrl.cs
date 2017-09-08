using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiPathMoveCtrl : MonoBehaviour 
{
	[System.Serializable]
    public class JiPathInfo
    {
        // Select a path that you will move on.
        public JiPath m_Path;

        // Set a delay time to start move when this component is enabled
        public float m_DelayTime;
    }
    public JiPathInfo m_pathInfo;

    // An individual name for stopping itween
    //private string _itweenName = "";

    // A curved animation path for target to move
    //private Vector3[] _Path = null;

    // whether automatically generate a curve from Gameobject's current position to 
    // the begining of the path.
    public bool m_movetoPath = true;

    // whether the gameobject will orient to its direction of travel.
    public bool m_orientedToPath = false;

    // A target that the GameObject will look at.
    // If the m_lookTarget_Transform is null, m_lookTarget_Vector3 will be used.
    public Transform m_lookTarget_Trans = null;
    public Vector3 m_lookTarget_Vector = Vector3.zero;

    // How much of a percentage (from 0 to 1) to look ahead on a path to influence 
    // how strict "orienttopath" is and how much the object will anticipate each curve 
    public float m_lookAhead;

    // For whether the movement is in wordspace or in localspace.
    public bool m_isLocal = false;

    // Time in seconds the movement will take to complete.
    // If the time is 0, than we will use the speed variable instead.
    // Or, we will use the time.
    public float m_time = 0f;

    // The speed of the GameObject movement.
    public float m_speed = 0f;

    // The ease curve of the movement
    public AnimationCurve m_easeAnimCurve;

    // The ease type of the movement.
    public iTween.EaseType m_easeType = iTween.EaseType.linear;

    // The loop type of the movement.
    public iTween.LoopType m_loopType = iTween.LoopType.none;


    private void Start()
    {
        iTween.Init(this.gameObject);
        Hashtable args = Lauch();
        iTween.MoveTo(this.gameObject, args);        
    }


    private void OnDisabled()
    {

    }


    // Lauch the itween variabeles
    private Hashtable Lauch()
    {
        Hashtable args = new Hashtable();

        args.Add("axis", "z");   // restrict the rotation to z-axis only.

        args.Add("name", m_pathInfo.m_Path.m_PathName);
        args.Add("path", m_pathInfo.m_Path.m_CtrolNode.ToArray()); 
        args.Add("delay", m_pathInfo.m_DelayTime); 
        args.Add("movetopath", m_movetoPath);
        args.Add("orienttopath", m_orientedToPath);

        if(m_lookTarget_Trans != null)
            args.Add("looktarget", m_lookTarget_Trans);
        else if(m_lookTarget_Vector != Vector3.zero)
            args.Add("looktarget", m_lookTarget_Vector);

        args.Add("islocal", m_isLocal);

        if(Mathf.Approximately(m_time, 0))
            args.Add("speed", m_speed);
        else
            args.Add("time", m_time);

        args.Add("easetype", m_easeType);
        args.Add("easeAnimCurve", m_easeAnimCurve);
        args.Add("looptype", m_loopType);

        return args;
    }

}
