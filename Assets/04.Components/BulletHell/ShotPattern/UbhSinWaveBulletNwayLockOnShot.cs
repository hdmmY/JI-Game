using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh sin wave bullet nway lock on shot.
/// </summary>
public class UbhSinWaveBulletNwayLockOnShot : UbhSinWaveBulletNwayShot
{
    // "Set a target with tag name."
    public bool _SetTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    public string _TargetTagName = "Player";
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    // "Overwrite CenterAngle in direction of target to Transform.position."
    public Transform _TargetTransform;
    // "Always aim to target."
    public bool _Aiming;

    public override void Shot ()
    {
        if (_Shooting)
        {
            return;
        }

        AimTarget ();

        if (_TargetTransform == null)
        {
            Debug.LogWarning ("Cannot shot because TargetTransform is not set.");
            return;
        }

        base.Shot ();

        if (_Aiming)
        {
            StartCoroutine (AimingCoroutine ());
        }
    }

    void AimTarget ()
    {
        if (_TargetTransform == null && _SetTargetFromTag)
        {
            _TargetTransform = UbhUtil.GetTransformFromTagName (_TargetTagName);
        }
        if (_TargetTransform != null)
        {
            _CenterAngle = UbhUtil.GetAngleFromTwoPosition (transform, _TargetTransform);
        }
    }

    IEnumerator AimingCoroutine ()
    {
        while (_Aiming)
        {
            if (_Shooting == false)
            {
                yield break;
            }

            AimTarget ();

            yield return 0;
        }
    }
}