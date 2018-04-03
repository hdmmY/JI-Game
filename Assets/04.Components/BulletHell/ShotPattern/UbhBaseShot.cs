using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;


/// <summary>
/// Ubh base shot.
/// Each shot pattern classes inherit this class.
/// </summary>
public abstract class UbhBaseShot : UbhMonoBehaviour
{
    /// <summary>
    /// Set a bullet prefab for the shot. (ex. sprite or model)
    /// </summary>
    [ValidateInput("HasBulletPrefab", "BulletPrefab is not set!")]
    public GameObject m_bulletPrefab;

    public bool m_bind;

    /// <summary>
    /// Bind the bullet transform to other
    /// </summary>
    [ShowIf("m_bind")]
    [ValidateInput("CorrectBindTranform", "BindTransform is not set!")]
    public Transform m_bindTransform = null;

    /// <summary>
    /// Set a bullet number of shot.
    /// </summary>
    [BoxGroup("Base")]
    public int m_bulletNum = 10;

    /// <summary>
    /// Set a bullet base speed of shot.
    /// </summary>
    [BoxGroup("Base")]
    public float m_bulletSpeed = 2f;

    /// <summary>
    /// Set an acceleration of bullet speed.
    /// </summary>
    [BoxGroup("Base")]
    public float m_accelerationSpeed = 0f;

    /// <summary>
    /// Set an speed of bullet turning.
    /// </summary>
    [BoxGroup("Base")]
    public float m_angleSpeed = 0f;


    /// <summary>
    /// This flag is pause and resume bullet at specified time.
    /// </summary>
    [BoxGroup("Pause")]
    public bool m_usePauseAndResume = false;

    /// <summary>
    /// Set a time to pause bullet.
    /// </summary>
    [BoxGroup("Pause")]
    public float m_pauseTime = 0f;

    /// <summary>
    /// Set a time to resume bullet.
    /// </summary>
    [BoxGroup("Pause")]
    public float m_resumeTime = 0f;

    /// <summary>
    /// This flag settings pooling bullet GameObject from object pool at initial awake.
    /// </summary>
    public bool m_initialPooling = false;

    /// <summary>
    /// This flag is automatically release the bullet GameObject at the specified time.
    /// </summary>
    public bool m_useAutoRelease = true;

    /// <summary>
    /// Set a time to automatically release after the shot at using UseAutoRelease. (sec)
    /// That is the bullet life time."
    /// </summary>
    [ShowIf("m_useAutoRelease")]
    public float m_autoReleaseTime = 20f;

    /// <summary>
    /// Call when the shot is finish
    /// </summary>
    [HideInInspector]
    public System.Action<UbhBaseShot> OnShotFinish;

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
                BulletPool.Instance.ReleaseGameObject(goBulletList[i]);
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

        StopAllCoroutines();
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
    protected void FinishedShot(UbhBaseShot shotPattern)
    {
        if (OnShotFinish != null)
        {
            OnShotFinish(shotPattern);
        }
        _Shooting = false;
    }

    /// <summary>
    /// Get bullet from object pool.
    /// </summary>                        
    /// <param name="forceInstantiate"> force the pool to return an instantiate bullet. </param>
    /// <returns></returns>
    protected JIBulletController GetBullet(Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        return GetBullet(m_bulletPrefab, position, rotation, forceInstantiate);
    }

    /// <summary>
    /// Get bullet from object pool.
    /// </summary>                        
    /// <param name="forceInstantiate"> force the pool to return an instantiate bullet. </param>
    /// <returns></returns>
    protected JIBulletController GetBullet(GameObject bulletPrefab, Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Cannot generate a bullet because BulletPrefab is not set.");
            return null;
        }

        // get Bullet GameObject from BulletPool
        var goBullet = BulletPool.Instance.GetGameObject(bulletPrefab, position, rotation, forceInstantiate);
        if (goBullet == null)
        {
            return null;
        }

        // Get or add JIBulletController component
        var bulletController = goBullet.GetComponent<JIBulletController>();
        if (bulletController == null)
        {
            bulletController = goBullet.AddComponent<JIBulletController>();
        }

        // Get or add JIBulletProperty component
        var bulletProperty = goBullet.GetComponent<JIBulletProperty>();
        if (bulletProperty == null)
        {
            bulletProperty = goBullet.AddComponent<JIBulletProperty>();
        }

        // Bind transfrom
        if (m_bindTransform != null)
        {
            goBullet.transform.parent = m_bindTransform;
        }

        return bulletController;
    }

    /// <summary>
    /// Abstract shot method.
    /// It is not the shot bullet method.
    /// </summary>
    public abstract void Shot();

    /// <summary>
    /// Shot JIBulletController object.
    /// </summary>
    protected void ShotBullet(JIBulletController bullet, float speed, float angle,
                               bool homing = false, Transform homingTarget = null, float homingAngleSpeed = 0f, float maxHomingAngle = 0f,
                               bool wave = false, float waveSpeed = 0f, float waveRangeSize = 0f)
    {
        if (bullet == null)
        {
            return;
        }
        bullet.Shot(speed, angle, m_angleSpeed, m_accelerationSpeed,
                    homing, homingTarget, homingAngleSpeed, maxHomingAngle,
                    wave, waveSpeed, waveRangeSize,
                    m_usePauseAndResume, m_pauseTime, m_resumeTime);
    }


    protected void ShotBullet(JIBulletController bullet, IEnumerator bulletMoveRoutine)
    {
        if (bullet == null)
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
        JICoroutine.StartIE(AutoReleaseBulletGameObjectCoroutine(goBullet));
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

            countUpTime += JITimer.Instance.DeltTime;
        }

        BulletPool.Instance.ReleaseGameObject(goBullet);
    }

    #region Inspector Function
    private bool HasBulletPrefab(GameObject bulletPrefab)
    {
        return bulletPrefab != null;
    }

    private bool CorrectBindTranform(Transform bindTransform)
    {
        if (m_bind)
            return bindTransform != null;
        return true;
    }
    #endregion
}