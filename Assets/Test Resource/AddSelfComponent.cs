using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSelfComponent : MonoBehaviour
{
    public GameObject m_parent;

    public Sprite m_targetSprite;

    public void AddEventMaster()
    {

        foreach(var toggle in m_parent.GetComponentsInChildren<LaserHomingShotToggle>())
        {
            toggle.m_aimingTargetCompo = toggle.transform.parent.GetComponent<AnimationToggle.AimingTarget>();
        }
    }
}
