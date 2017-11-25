using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh linear lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Linear Shot (Lock On)")]
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

    protected override void Awake ()
    {
        base.Awake();
    }

    public override void Shot ()
    {
        if (_Shooting) {
            return;
        }

        AimTarget();

        if (m_targetTransform == null) {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        base.Shot();

        if (m_aiming) {
            StartCoroutine(AimingCoroutine());
        }
    }

    void AimTarget ()
    {
        if (m_targetTransform == null && m_setTargetFromTag) {
            m_targetTransform = UbhUtil.GetTransformFromTagName(m_targetTagName);
        }
        if (m_targetTransform != null) {
            m_shotAngle = UbhUtil.GetAngleFromTwoPosition(transform, m_targetTransform);
        }
    }

    IEnumerator AimingCoroutine ()
    {
        while (m_aiming) {
            if (_Shooting == false) {
                yield break;
            }

            AimTarget();

            yield return 0;
        }
    }
}