using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh hole circle lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Hole Circle Shot (Lock On)")]
public class UbhHoleCircleLockOnShot : UbhHoleCircleShot
{
    // "Set a target with tag name."
    public bool _SetTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    public string _TargetTagName = "Player";
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    public Transform _TargetTransform;

    protected override void Awake ()
    {
        base.Awake();
    }

    // "Overwrite HoleCenterAngle in direction of target to Transform.position."
    public override void Shot ()
    {
        if (_TargetTransform == null && _SetTargetFromTag) {
            _TargetTransform = UbhUtil.GetTransformFromTagName(_TargetTagName);
        }

        if (_TargetTransform == null) {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        _HoleCenterAngle = UbhUtil.GetAngleFromTwoPosition(transform, _TargetTransform);

        base.Shot();
    }
}