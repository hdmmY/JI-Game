using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;


/// <summary>
/// Ubh random lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Random Shot (Lock On)")]
public class UbhRandomLockOnShot : UbhRandomShot
{
    /// <summary>
    /// Always aim to target.
    /// </summary>
    [BoxGroup("Aim")]
    public bool _Aiming;

    /// <summary>
    /// Set a target with tag name.
    /// </summary>
    [ShowIf("_Aiming")]
    [BoxGroup("Aim")]
    public bool _SetTargetFromTag = true;

    /// <summary>
    /// Set a unique tag name of target at using SetTargetFromTag.
    /// </summary>
    [ValidateInput("SetTargetCorrect", "Cannot shot because target is not set.")]
    [ShowIf("_Aiming")]
    [ShowIf("_SetTargetFromTag")]
    [BoxGroup("Aim")]
    public string _TargetTagName = "Player";

    /// <summary>
    /// Transform of lock on target.
    /// It is not necessary if you want to specify target in tag.
    /// Overwrite RandomCenterAngle in direction of target to Transform.position.
    /// </summary>
    [ValidateInput("SetTargetCorrect", "Cannot shot because target is not set.")]
    [ShowIf("_Aiming")]
    [HideIf("_SetTargetFromTag")]
    [BoxGroup("Aim")]
    public Transform _TargetTransform;


    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {
        if (_Shooting)
        {
            return;
        }

        if (_Aiming) AimTarget();

        if (_TargetTransform == null)
        {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        base.Shot();

        if (_Aiming)
        {
            StartCoroutine(AimingCoroutine());
        }
    }

    void AimTarget()
    {
        if (_TargetTransform == null && _SetTargetFromTag)
        {
            _TargetTransform = UbhUtil.GetTransformFromTagName(_TargetTagName);
        }
        if (_TargetTransform != null)
        {
            _RandomCenterAngle = UbhUtil.GetAngleFromTwoPosition(transform, _TargetTransform);
        }
    }

    IEnumerator AimingCoroutine()
    {
        while (_Aiming)
        {
            if (_Shooting == false)
            {
                yield break;
            }

            AimTarget();

            yield return 0;
        }
    }

    #region Inspector Func
    private bool SetTargetCorrect(string targetTag)
    {
        if (_Aiming)
        {
            if (_SetTargetFromTag)
                return !string.IsNullOrEmpty(targetTag);
            else
                return _TargetTransform == null;
        }

        return true;
    }

    private bool SetTargetCorrect(Transform targetTrans)
    {
        if (_Aiming)
        {
            if (_SetTargetFromTag)
                return !string.IsNullOrEmpty(_TargetTagName);
            else
                return targetTrans != null;
        }

        return true;
    }
    #endregion
}