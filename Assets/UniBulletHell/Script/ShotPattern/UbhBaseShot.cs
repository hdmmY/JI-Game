using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Ubh base shot.
/// Each shot pattern classes inherit this class.
/// </summary>
public abstract class UbhBaseShot : UbhMonoBehaviour
{
    // "Set a bullet prefab for the shot. (ex. sprite or model)"
    public GameObject _BulletPrefab;
    // "Set a bullet number of shot."
    public int _BulletNum = 10;
    // "Set a bullet base speed of shot."
    public float _BulletSpeed = 2f;
    // "Set an acceleration of bullet speed."
    public float _AccelerationSpeed = 0f;
    // "Set an acceleration of bullet turning."
    public float _AccelerationTurn = 0f;
    // "This flag is pause and resume bullet at specified time."
    public bool _UsePauseAndResume = false;
    // "Set a time to pause bullet."
    public float _PauseTime = 0f;
    // "Set a time to resume bullet."
    public float _ResumeTime = 0f;
    // "This flag settings pooling bullet GameObject from object pool at initial awake."
    public bool _InitialPooling = false;
    // "This flag is automatically release the bullet GameObject at the specified time."
    public bool _UseAutoRelease = false;
    // "Set a time to automatically release after the shot at using UseAutoRelease. (sec)"
    // "That is the bullet life time."
    public float _AutoReleaseTime = 10f;
    // "Set a GameObject that receives callback method when shooting is over."
    public GameObject _CallbackReceiver;
    // "Set a name of callback method at using Call Back Receiver."
    public string _CallbackMethod;

    protected UbhShotCtrl ShotCtrl
    {
        get
        {
            if (_ShotCtrl == null)
            {
                _ShotCtrl = transform.GetComponentInParent<UbhShotCtrl>();
            }
            return _ShotCtrl;
        }
    }
    UbhShotCtrl _ShotCtrl;

    protected bool _Shooting;

    /// <summary>
    /// Call from override Awake method in inheriting classes.
    /// Example : protected override void Awake () { base.Awake (); }
    /// </summary>
    protected virtual void Awake()
    {
        if (_InitialPooling)
        {
            var goBulletList = new List<GameObject>();
            for (int i = 0; i < _BulletNum; i++)
            {
                var bullet = GetBullet(Vector3.zero, Quaternion.identity, true);
                if (bullet != null)
                {
                    goBulletList.Add(bullet.gameObject);
                }
            }
            for (int i = 0; i < goBulletList.Count; i++)
            {
                UbhObjectPool.Instance.ReleaseGameObject(goBulletList[i]);
            }
        }
    }

    /// <summary>
    /// Call from override OnDisable method in inheriting classes.
    /// Example : protected override void OnDisable () { base.OnDisable (); }
    /// </summary>
    protected virtual void OnDisable()
    {
        _Shooting = false;
    }

    /// <summary>
    /// UbhShotCtrl setter.
    /// </summary>
    public void SetShotCtrl(UbhShotCtrl shotCtrl)
    {
        _ShotCtrl = shotCtrl;
    }

    /// <summary>
    /// Finished shot.
    /// </summary>
    protected void FinishedShot()
    {
        if (_CallbackReceiver != null && string.IsNullOrEmpty(_CallbackMethod) == false)
        {
            _CallbackReceiver.SendMessage(_CallbackMethod, SendMessageOptions.DontRequireReceiver);
        }
        _Shooting = false;
    }

    /// <summary>
    /// Get bullet from object pool.
    /// </summary>                        
    /// <param name="forceInstantiate"> force the pool to return an instantiate bullet. </param>
    /// <returns></returns>
    protected UbhBullet GetBullet(Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        if (_BulletPrefab == null)
        {
            Debug.LogWarning("Cannot generate a bullet because BulletPrefab is not set.");
            return null;
        }

        // get Bullet GameObject from ObjectPool
        var goBullet = UbhObjectPool.Instance.GetGameObject(_BulletPrefab, position, rotation, forceInstantiate);
        if (goBullet == null)
        {
            return null;
        }

        // get or add UbhBullet component
        var bullet = goBullet.GetComponent<UbhBullet>();
        if (bullet == null)
        {
            bullet = goBullet.AddComponent<UbhBullet>();
        }

        return bullet;
    }

    /// <summary>
    /// Abstract shot method.
    /// It is not the shot bullet method.
    /// </summary>
    public abstract void Shot();

    /// <summary>
    /// Shot UbhBullet object.
    /// </summary>
    protected void ShotBullet(UbhBullet bullet, float speed, float angle,
                               bool homing = false, Transform homingTarget = null, float homingAngleSpeed = 0f, float maxHomingAngle = 0f,
                               bool wave = false, float waveSpeed = 0f, float waveRangeSize = 0f)
    {
        if (bullet == null)
        {
            return;
        }
        bullet.Shot(speed, angle, _AccelerationSpeed, _AccelerationTurn,
                    homing, homingTarget, homingAngleSpeed, maxHomingAngle,
                    wave, waveSpeed, waveRangeSize,
                    _UsePauseAndResume, _PauseTime, _ResumeTime,
                    ShotCtrl != null ? ShotCtrl._AxisMove : UbhUtil.AXIS.X_AND_Y);
    }

    /// <summary>
    /// Auto release bullet GameObject after _AutoReleaseTime sec.
    /// </summary>
    protected void AutoReleaseBulletGameObject(GameObject goBullet)
    {
        if (_UseAutoRelease == false || _AutoReleaseTime < 0f)
        {
            return;
        }
        UbhCoroutine.StartIE(AutoReleaseBulletGameObjectCoroutine(goBullet));
    }


    /// <summary>
    /// Auto release bullet GameObject after _AutoReleaseTime sec.
    /// </summary>
    IEnumerator AutoReleaseBulletGameObjectCoroutine(GameObject goBullet)
    {
        float countUpTime = 0f;

        while (true)
        {
            if (goBullet == null || goBullet.activeInHierarchy == false)
            {
                yield break;
            }

            if (_AutoReleaseTime <= countUpTime)
            {
                break;
            }

            yield return 0;

            countUpTime += UbhTimer.Instance.DeltaTime;
        }

        UbhObjectPool.Instance.ReleaseGameObject(goBullet);
    }
}