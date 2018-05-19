using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent (typeof (PlayerProperty))]
public class PlayerShoot : MonoBehaviour
{
    #region Public variables

    // Weapon overload
    [ShowInInspector, ReadOnly]
    public bool Overload { get; private set; }

    [BoxGroup ("Black")]
    public AnimationCurve LaserWidthCurve;

    [BoxGroup ("Black")]
    public AnimationCurve LaserDamageCurve;

    [BoxGroup ("Black"), Range (0, 1)]
    public float LaserInterval;

    [BoxGroup ("Black")]
    public float LaserGrowthSpeed = 1;

    [BoxGroup ("Black")]
    public GrowthLaser LaserPrefab;

    [BoxGroup ("Black")]
    public float BlackOverloadTime;

    [BoxGroup ("White")]
    public WBulletEmitController WhiteBulletEmitter;

    [BoxGroup ("White")]
    public AnimationCurve WhiteEmitInterval;

    [BoxGroup ("White")]
    public AnimationCurve WhiteBulletSpeed;

    [BoxGroup ("White")]
    public float WhiteOverloadTime;

    #endregion

    #region Monobehavior

    private void OnEnable ()
    {
        _player = GetComponent<PlayerProperty> ();

        _blackOverloadTimer = 0f;
        _whiteOverloadTimer = 0f;

        if (_player.m_playerState == JIState.Black)
        {
            BeforeChangeToBState ();
        }
        else
        {
            BeforeChangeToWState ();
        }
    }

    private void Update ()
    {
        // Change state
        if (InputManager.Instance.InputCtrl.ChangeStateButtonDown)
        {
            if (_player.m_playerState == JIState.Black)
            {
                BeforeChangeToWState ();
                _player.m_playerState = JIState.White;
            }
            else if (_player.m_playerState == JIState.White)
            {
                BeforeChangeToBState ();
                _player.m_playerState = JIState.Black;
            }
        }

        if (_player.m_playerState == JIState.Black)
        {
            BlackStateLaserShot ();
        }
        else if (_player.m_playerState == JIState.White)
        {
            WhiteStateShot ();
        }
    }

    #endregion

    #region Private member and method

    private PlayerProperty _player;

    private float _blackOverloadTimer;

    private float _whiteOverloadTimer;

    private GrowthLaser _curLaser;

    private float _whiteShotTimer;

    private WBulletEmitController _curEmitter;

    // Called before change to black state
    private void BeforeChangeToBState ()
    {
        Overload = false;

        _blackOverloadTimer = 0f;

        Vector3 laserPos = transform.position + new Vector3 (0, 0.258f, 0);
        Quaternion laserRot = Quaternion.identity;
        _curLaser = Instantiate (LaserPrefab, laserPos, laserRot, transform) as GrowthLaser;
        _curLaser.LaserWidth = LaserWidthCurve.Evaluate (_blackOverloadTimer);
        _curLaser.Damage = (int) LaserDamageCurve.Evaluate (_blackOverloadTimer);
        _curLaser.Interval = LaserInterval;
        _curLaser.GrowthSpeed = LaserGrowthSpeed;

        if (_curEmitter != null)
        {
            Destroy (_curEmitter.gameObject);
            _curEmitter = null;
        }
    }

    // Black state shot : shot the laser
    private void BlackStateLaserShot ()
    {
        if (InputManager.Instance.InputCtrl.ShotButton)
        {
            _blackOverloadTimer += JITimer.Instance.DeltTime;

            if (!_curLaser.gameObject.activeInHierarchy)
            {
                _curLaser.UpdateLaserAppear ();
                _curLaser.gameObject.SetActive (true);
            }
        }
        else
        {
            _blackOverloadTimer = 0f;
            _curLaser.LaserLength = 0f;

            if (_curLaser.gameObject.activeInHierarchy)
                _curLaser.gameObject.SetActive (false);
        }

        Overload = _blackOverloadTimer >= BlackOverloadTime;

        _curLaser.LaserWidth = LaserWidthCurve.Evaluate (_blackOverloadTimer);
        _curLaser.Damage = (int) LaserDamageCurve.Evaluate (_blackOverloadTimer);
        _curLaser.Interval = LaserInterval;
        _curLaser.GrowthSpeed = LaserGrowthSpeed;
    }

    // Called before change to white state
    private void BeforeChangeToWState ()
    {
        Overload = false;

        _whiteOverloadTimer = 0f;
        _whiteShotTimer = 100f;

        Vector3 emitterPos = transform.position + new Vector3 (0, 0.4f, 0);
        Quaternion emitterRot = Quaternion.identity;
        _curEmitter = Instantiate (WhiteBulletEmitter, emitterPos, emitterRot,
            transform) as WBulletEmitController;

        if (_curLaser)
        {
            Destroy (_curLaser.gameObject);
            _curLaser = null;
        }

    }

    // White state shot : shot the white sector bullets
    private void WhiteStateShot ()
    {
        _whiteShotTimer += JITimer.Instance.DeltTime;

        WhiteBulletEmitter.BulletSpeed = WhiteBulletSpeed.Evaluate (_whiteOverloadTimer);

        if (InputManager.Instance.InputCtrl.ShotButton)
        {
            _whiteOverloadTimer += JITimer.Instance.DeltTime;

            if (_whiteShotTimer >= WhiteEmitInterval.Evaluate (_whiteOverloadTimer)) // shot
            {
                _whiteShotTimer = 0f;

                if (_whiteOverloadTimer >= WhiteOverloadTime)
                {
                    _curEmitter.OverloadShot ();
                }
                else
                {
                    _curEmitter.NormalShot ();
                }
            }
        }
        else
        {
            _whiteOverloadTimer = 0f;
        }

        Overload = _whiteOverloadTimer >= WhiteEmitInterval.Evaluate (_whiteOverloadTimer);
    }

    #endregion

}