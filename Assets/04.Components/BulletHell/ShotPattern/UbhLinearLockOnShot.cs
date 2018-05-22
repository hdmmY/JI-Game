using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh linear lock on shot.
/// </summary>
public class UbhLinearLockOnShot : UbhLinearShot
{
    // "Set a target with tag name."
    public bool m_setTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    public string m_targetTagName = "Player";
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    // "Overwrite Angle in direction of target to Transform.position."
    public Transform m_targetTransform;
    // "Always aim to target."
    public bool m_aiming;

    public override void Shot ()
    {
        if (_Shooting)
        {
            return;
        }

        if (m_targetTransform == null)
        {
            Debug.LogWarning ("Cannot shot because TargetTransform is not set.");
            return;
        }

        base.Shot ();
    }

    private void OnEnable ()
    {
        AimTarget ();
    }

    private void Update ()
    {
        AimTarget ();
    }

    private void AimTarget ()
    {
        if (m_targetTransform == null && m_setTargetFromTag)
        {
            m_targetTransform = UbhUtil.GetTransformFromTagName (m_targetTagName);
        }
        if (m_targetTransform != null)
        {
            m_shotAngle = UbhUtil.GetAngleFromTwoPosition (transform, m_targetTransform);
        }
    }
}