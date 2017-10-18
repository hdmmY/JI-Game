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
    public GameObject m_bulletPrefab;
    // "Set a bullet number of shot."
    public int m_bulletNum = 10;
    // "Set a bullet base speed of shot."
    public float m_bulletSpeed = 2f;
    // "Set an acceleration of bullet speed."
    public float m_accelerationSpeed = 0f;
    // "Set an acceleration of bullet turning."
    public float m_accelerationTurn = 0f;
    // "This flag is pause and resume bullet at specified time."
    public bool m_usePauseAndResume = false;
    // "Set a time to pause bullet."
    public float m_pauseTime = 0f;
    // "Set a time to resume bullet."
    public float m_resumeTime = 0f;
    // "This flag settings pooling bullet GameObject from object pool at initial awake."
    public bool m_initialPooling = false;
    // "This flag is automatically release the bullet GameObject at the specified time."
    public bool m_useAutoRelease = false;
    // "Set a time to automatically release after the shot at using UseAutoRelease. (sec)"
    // "That is the bullet life time."
    public float m_autoReleaseTime = 10f;
    // "Set a GameObject that receives callback method when shooting is over."
    public GameObject m_callbackReceiver;
    // "Set a name of callback method at using Call Back Receiver."
    public string m_callbackMethod;

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
        if (m_initialPooling)
        {
            var goBulletList = new List<GameObject>();
            for (int i = 0; i < m_bulletNum; i++)
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
        if (m_callbackReceiver != null && string.IsNullOrEmpty(m_callbackMethod) == false)
        {
            m_callbackReceiver.SendMessage(m_callbackMethod, SendMessageOptions.DontRequireReceiver);
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
        if (m_bulletPrefab == null)
        {
            Debug.LogWarning("Cannot generate a bullet because BulletPrefab is not set.");
            return null;
        }

        // get Bullet GameObject from ObjectPool
        var goBullet = UbhObjectPool.Instance.GetGameObject(m_bulletPrefab, position, rotation, forceInstantiate);
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
        bullet.Shot(speed, angle, m_accelerationSpeed, m_accelerationTurn,
                    homing, homingTarget, homingAngleSpeed, maxHomingAngle,
                    wave, waveSpeed, waveRangeSize,
                    m_usePauseAndResume, m_pauseTime, m_resumeTime,
                    ShotCtrl != null ? ShotCtrl._AxisMove : UbhUtil.AXIS.X_AND_Y);
    }


    protected void ShotBullet(UbhBullet bullet, IEnumerator bulletMoveRoutine)
    {
        if(bullet == null)
        {
            return;
        }

        bullet.Shot(bulletMoveRoutine);
    }


    /// <summary>
    /// Auto release bullet GameObject after m_autoReleaseTime sec.
    /// </summary>
    protected void AutoReleaseBulletGameObject(GameObject goBullet)
    {
        if (m_useAutoRelease == false || m_autoReleaseTime < 0f)
        {
            return;
        }
        UbhCoroutine.StartIE(AutoReleaseBulletGameObjectCoroutine(goBullet));
    }


    /// <summary>
    /// Auto release bullet GameObject after m_autoReleaseTime sec.
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

            if (m_autoReleaseTime <= countUpTime)
            {
                break;
            }

            yield return 0;

            countUpTime += UbhTimer.Instance.DeltaTime;
        }

        UbhObjectPool.Instance.ReleaseGameObject(goBullet);
    }
}