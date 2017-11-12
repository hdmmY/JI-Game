using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AngleHomingShot : UbhBaseShot 
{
    public float _StartAngle = 0f;

	// "Set a delay time between bullet and next bullet. (sec)"
    public float _BetweenDelay = 0.1f;
    // "Set a speed of homing angle."
    public float _HomingAngleSpeed = 20f;
    // "Set the max turning angle in homing."
    public float _MaxHomingAngle = 60f;
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

    public override void Shot ()
    {
        StartCoroutine(ShotCoroutine());
    }

    IEnumerator ShotCoroutine ()
    {
        if (m_bulletNum <= 0) {
            Debug.LogWarning("Cannot shot because BulletNum is not set.");
            yield break;
        }
        if (_Shooting) {
            yield break;
        }
        _Shooting = true;

        for (int i = 0; i < m_bulletNum; i++) {
            if (0 < i && 0f < _BetweenDelay) {
                yield return StartCoroutine(UbhUtil.WaitForSeconds(_BetweenDelay));
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            if (_TargetTransform == null && _SetTargetFromTag) {
                _TargetTransform = UbhUtil.GetTransformFromTagName(_TargetTagName);
            }

            if (_TargetTransform == null){
                Debug.LogWarning("Can not shoot because _TargetTransform is not set!");
                yield break;
            }

            //float angle = UbhUtil.GetAngleFromTwoPosition(transform, _TargetTransform, ShotCtrl.m_AxisMove);

            ShotBullet(bullet, m_bulletSpeed, _StartAngle, true, _TargetTransform, _HomingAngleSpeed, _MaxHomingAngle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }
}
