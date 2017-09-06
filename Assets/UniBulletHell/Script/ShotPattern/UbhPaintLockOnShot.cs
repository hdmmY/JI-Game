using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh paint lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Paint Shot (Lock On)")]
public class UbhPaintLockOnShot : UbhPaintShot
{
    // "Set a target with tag name."
    public bool _SetTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    public string _TargetTagName = "Player";
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    // "Overwrite PaintCenterAngle in direction of target to Transform.position."
    public Transform _TargetTransform;

    protected override void Awake ()
    {
        base.Awake();
    }

    public override void Shot ()
    {
        if (_Shooting) {
            return;
        }
        if (_TargetTransform == null && _SetTargetFromTag) {
            _TargetTransform = UbhUtil.GetTransformFromTagName(_TargetTagName);
        }
        if (_TargetTransform == null) {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        _PaintCenterAngle = UbhUtil.GetAngleFromTwoPosition(transform, _TargetTransform, ShotCtrl._AxisMove);

        base.Shot();
    }
}