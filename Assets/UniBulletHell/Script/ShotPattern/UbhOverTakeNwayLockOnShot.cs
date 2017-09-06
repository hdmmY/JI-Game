using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh over take nway lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Over Take nWay Shot (Lock On)")]
public class UbhOverTakeNwayLockOnShot : UbhOverTakeNwayShot
{
    // "Set a target with tag name."
    public bool _SetTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    public string _TargetTagName = "Player";
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    // "Overwrite CenterAngle in direction of target to Transform.position."
    public Transform _TargetTransform;

    protected override void Awake ()
    {
        base.Awake();
    }

    public override void Shot ()
    {
        if (_TargetTransform == null && _SetTargetFromTag) {
            _TargetTransform = UbhUtil.GetTransformFromTagName(_TargetTagName);
        }
        if (_TargetTransform == null) {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        _CenterAngle = UbhUtil.GetAngleFromTwoPosition(transform, _TargetTransform, ShotCtrl._AxisMove);

        base.Shot();
    }
}