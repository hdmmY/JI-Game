using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Boss.Falcon
{
    using MonsterLove.StateMachine;
    using MovementEffects.Extensions;
    using MovementEffects;

    public class FalconStageTwo : MonoBehaviour
    {
        public enum States
        {
            None = 0,
            FullScreenWind,
            SideWind,
            ShotOne, // See the explain
            ShotTwo, // See the explain
            ShotThree, // See the explain
            Empty // Use for state loop
        }

        public States CurrentState
        {
            get
            {
                if (_fsm != null) return _fsm.State;
                return States.None;
            }
        }

        public StateMachine<States> _fsm;

        private bool _active;

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                if (value == true)
                {
                    Awake ();
                }
            }
        }

        // Variables that used by states
        private Transform _player;

        [SerializeField] private Transform _falcon;

        [SerializeField] private WindArea _fullScreeWindArea;

        [SerializeField] private WindArea _leftWindArea;

        [SerializeField] private WindArea _rightWindArea;

        private bool _lastStateIsFullWind;

        private States _secondShotPattern;

        #region FullScreenWind Vairables

        /// <summary>
        /// FullScreenWind State : Wind last time 
        /// </summary>
        [BoxGroup ("Full Screen Wind"), Range (0.1f, 10f), SerializeField]
        private float _fullWindLastTime;

        /// <summary>
        /// FullScreenWind State : Timer for wind last time
        /// </summary>
        private float _fullWindTimer;

        /// <summary>
        /// FullScreenWind State : Boss center position 
        /// </summary>
        [BoxGroup ("Full Screen Wind"), SerializeField]
        private Vector3 _fullWindBossCenter;

        /// <summary>
        /// FullScreenWind State : Speed when boss move to the center
        /// </summary>
        [BoxGroup ("Full Screen Wind"), Range (0.01f, 10), SerializeField]
        private float _fullWindBossMoveSpeed;

        [BoxGroup ("Full Screen Wind"), SerializeField]
        private GameObject _fullWindBullet;

        [BoxGroup ("Full Screen Wind"), Range (1, 10), SerializeField]
        private int _fullWindSecNumber;

        [BoxGroup ("Full Screen Wind"), Range (0, 360), SerializeField]
        private float _fullWindSecRange;

        [BoxGroup ("Full Screen Wind"), Range (1, 30f), SerializeField]
        private int _fullWindNumberPerSector;

        [BoxGroup ("Full Screen Wind"), Range (0.01f, 2f), SerializeField]
        private float _fullWindShotInterval;

        [BoxGroup ("Full Screen Wind"), Range (0.5f, 5f), SerializeField]
        private float _fullWindInitVec;

        [BoxGroup ("Full Screen Wind"), Range (0.5f, 5f), SerializeField]
        private float _fullWindInitDecelerateTime;

        [BoxGroup ("Full Screen Wind"), Range (0.01f, 2f), SerializeField]
        private float _fullWindDecelerateTimeInterval;

        [BoxGroup ("Full Screen Wind"), Range (0.05f, 5f), SerializeField]
        private float _fullWindWaitTimeWhenEndShot;

        #endregion

        #region  SideWind Variables

        /// <summary>
        /// SideWind : Wind last time
        /// </summary>
        [BoxGroup ("Side Wind"), Range (0.1f, 10f), SerializeField]
        private float _sideWindLastTime;

        /// <summary>
        /// SideWind : Boss move speed
        /// </summary>
        [BoxGroup ("Side Wind"), Range (1f, 30f), SerializeField]
        private float _sideWindMoveSpeed;

        [BoxGroup ("Side Wind"), SerializeField]
        private Vector3 _sideWindBossCenter;

        [BoxGroup ("Side Wind"), SerializeField]
        private GameObject _sideWindBullet;

        [BoxGroup ("Side Wind"), Range (1, 10), SerializeField]
        private int _sideWindSecNumber;

        [BoxGroup ("Side Wind"), Range (0, 360), SerializeField]
        private float _sideWindSecRange;

        [BoxGroup ("Side Wind"), Range (1, 30f), SerializeField]
        private int _sideWindNumberPerSector;

        [BoxGroup ("Side Wind"), Range (0.01f, 2f), SerializeField]
        private float _sideWindShotInterval;

        [BoxGroup ("Side Wind"), Range (0.5f, 5f), SerializeField]
        private float _sideWindBulletVec;

        private SequenceInstance _sideBossMoveSeqInstance;

        #endregion

        #region  ShotOne Variables

        /// <summary>
        /// ShotOne State : Emitter prefab
        /// </summary>
        [BoxGroup ("ShotOne"), SerializeField]
        private GameObject _shotOneEmitter;

        /// <summary>
        /// ShotOne State : Bullet prefab
        /// </summary>
        [BoxGroup ("ShotOne"), SerializeField]
        private GameObject _shotOneBullet;

        /// <summary>
        /// ShotOne State : Emitter spin circle radius
        /// </summary>
        [BoxGroup ("ShotOne"), Range (0.05f, 2f), SerializeField]
        private float _shotOneCircleRadius;

        [BoxGroup ("ShotOne"), Range (0.1f, 5f), SerializeField]
        /// <summary>
        /// ShotOne State : Emitter spin move speed
        /// </summary>
        private float _shotOneCircleMoveSpeed;

        /// <summary>
        /// ShotOne State : Emitter spin move time
        /// </summary>
        [BoxGroup ("ShotOne"), Range (0.1f, 20f), SerializeField]
        private float _shotOneCircleMoveTime;

        /// <summary>
        /// ShotOne State : Emitter shot interval
        /// </summary>
        [BoxGroup ("ShotOne"), Range (0.05f, 2f), SerializeField]
        private float _shotOneShotInterval;

        /// <summary>
        /// ShotOne State : Bullet move speed
        /// </summary>
        [BoxGroup ("ShotOne"), Range (0.05f, 20f), SerializeField]
        private float _shotOneBulletSpeed;

        #endregion 

        #region  ShotTwo Variables

        [BoxGroup ("ShotTwo"), SerializeField]
        private GameObject _shotTwoSecBullet;

        [BoxGroup ("ShotTwo"), Range (1, 50), SerializeField]
        private int _shotTwoSecBulletNum;

        [BoxGroup ("ShotTwo"), Range (0.05f, 1f), SerializeField]
        private float _shotTwoSecMaxEmitInterval;

        [BoxGroup ("ShotTwo"), Range (0.05f, 1f), SerializeField]
        private float _shotTwoSecMinEmitInterval;

        [BoxGroup ("ShotTwo"), Range (0.05f, 10f), SerializeField]
        private float _shotTwoSecMaxBulletSpeed;

        [BoxGroup ("ShotTwo"), Range (0.05f, 10f), SerializeField]
        private float _shotTwoSecMinBulletSpeed;

        [BoxGroup ("ShotTwo"), SerializeField]
        private GameObject _shotTwoCirBullet;

        [BoxGroup ("ShotTwo"), Range (0.05f, 1f), SerializeField]
        private float _shotTwoCirMaxEmitInterval;

        [BoxGroup ("ShotTwo"), Range (0.05f, 1f), SerializeField]
        private float _shotTwoCirMinEmitInterval;

        [BoxGroup ("ShotTwo"), Range (0.05f, 10f), SerializeField]
        private float _shotTwoCirMaxBulletSpeed;

        [BoxGroup ("ShotTwo"), Range (0.05f, 10f), SerializeField]
        private float _shotTwoCirMinBulletSpeed;

        private bool _shotTwoShotCircleBullet;

        #endregion

        #region ShotThree Variables

        [BoxGroup ("ShotThree"), SerializeField]
        private GameObject _shotThreeBullet;

        /// <summary>
        /// ShotThree State : Number of bullets per circle
        /// </summary>
        [BoxGroup ("ShotThree"), Range (1, 50), SerializeField]
        private int _shotThreeNumberPerCircle = 10;

        /// <summary>
        /// ShotThree State : Bullet init velocity
        /// </summary>
        [BoxGroup ("ShotThree"), Range (0.1f, 15f), SerializeField]
        private float _shotThreeInitVec = 10f;

        /// <summary>
        /// ShotThree State : Bullet acceleration
        /// </summary>
        [BoxGroup ("ShotThree"), Range (-10f, -0.01f), SerializeField]
        private float _shotThreeAccel = -3f;

        /// <summary>
        /// ShotThree State : Circle shot times
        /// </summary>
        [BoxGroup ("ShotThree"), Range (1, 10f), SerializeField]
        private int _shotThreeShotTimes;

        [BoxGroup ("ShotThree"), Range (0.02f, 1f), SerializeField]
        private float _shotThreeMaxShotInterval;

        [BoxGroup ("ShotThree"), Range (0.02f, 1f), SerializeField]
        private float _shotThreeMinShotInteral;

        [BoxGroup ("ShotThree"), Range (0.01f, 1f), SerializeField]
        private float _shotThreeMaxMoveDis;

        [BoxGroup ("ShotThree"), Range (0.01f, 1f), SerializeField]
        private float _shotThreeMinMoveDis;

        #endregion

        private void Awake ()
        {
            if (!Active) return;

            _player = GameObject.FindGameObjectWithTag ("Player").transform;

            _fsm = StateMachine<States>.Initialize (this);
            _fsm.ChangeState (States.FullScreenWind);
            _lastStateIsFullWind = true;
        }

        #region None State

        private void None_Enter ()
        {

        }

        private void None_Update ()
        {

        }

        #endregion

        #region Empty State

        private void Empty_Enter ()
        {
            _fsm.ChangeState (_fsm.LastState);
        }

        #endregion

        #region FullScreenWind State

        private IEnumerator FullScreenWind_Enter ()
        {
            if (_fullScreeWindArea == null) yield break;
            if (_falcon == null || _fullWindBullet == null) yield break;

            StartCoroutine (FullScreenWind_Sequence ());
        }

        private void FullScreenWind_Transition ()
        {
            if (Active == false)
            {
                _fsm.ChangeState (States.None);
                return;
            }

            int next = Random.Range (0, 3);
            int nextOfNext = Random.Range (0, 3);
            while (nextOfNext == next)
            {
                nextOfNext = Random.Range (0, 3);
            }

            States nextState = GetStates (next);
            _secondShotPattern = GetStates (nextOfNext);

            _fsm.ChangeState (nextState, StateTransition.Safe);
        }

        private IEnumerator FullScreenWind_Sequence ()
        {
            // Move
            yield return StartCoroutine (FullScreenWind_Move ());

            // Shot
            yield return StartCoroutine (FullScreenWind_Shot ());

            // Blow
            yield return StartCoroutine (FullScreenWind_Blow ());
        }

        private IEnumerator FullScreenWind_Blow ()
        {
            _fullScreeWindArea.WindActive = true;
            _fullWindTimer = 0f;

            while (_fullWindTimer <= _fullWindLastTime)
            {
                _fullWindTimer += JITimer.Instance.DeltTime;
                yield return null;
            }

            _fullScreeWindArea.WindActive = false;
            FullScreenWind_Transition ();
        }

        private IEnumerator FullScreenWind_Move ()
        {
            float moveTime = (_falcon.position - _fullWindBossCenter).magnitude / _fullWindBossMoveSpeed;

            var movEffect = new Effect<Transform, Vector3> ();
            movEffect.Duration = moveTime;
            movEffect.RetrieveStart = (falcon, lastValue) => falcon.position;
            movEffect.RetrieveEnd = (falcon) => _fullWindBossCenter;
            movEffect.OnUpdate = (falcon, pos) => falcon.position = pos;
            movEffect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);

            var seq = new Sequence<Transform, Vector3> ();
            seq.Reference = _falcon;
            seq.Add (movEffect);

            var seqInstance = Movement.Run (seq);

            while (moveTime > 0)
            {
                moveTime -= JITimer.Instance.DeltTime;
                seqInstance.Timescale = JITimer.Instance.TimeScale;
                yield return null;
            }
        }

        private IEnumerator FullScreenWind_Shot ()
        {
            var winds = new WindArea[] { _fullScreeWindArea, _leftWindArea, _rightWindArea };

            float timer = 0f;
            float deltAngle = _fullWindSecRange / _fullWindNumberPerSector;
            float startAngle = 270f - _fullWindSecRange / 2 + deltAngle / 2;

            for (int i = 0; i < _fullWindSecNumber; i++)
            {
                for (int j = 0; j < _fullWindNumberPerSector; j++)
                {
                    float angle = startAngle + deltAngle * j;
                    Quaternion rotation = Quaternion.Euler (0, 0, angle - 90f);

                    float rad = angle * Mathf.Deg2Rad;
                    Vector3 vec = new Vector3 (Mathf.Cos (rad), Mathf.Sin (rad), 0f) * _fullWindInitVec;

                    float decelerateTime = _fullWindInitDecelerateTime - i * _fullWindDecelerateTimeInterval;

                    Transform bullet = GetBullet (_fullWindBullet, _falcon.position, Quaternion.identity);
                    bullet.GetComponent<JIBulletController> ()
                        .Shot (WindBulletMove.PauseLinearMove (bullet, vec, decelerateTime, winds));
                }

                while (timer < _fullWindShotInterval)
                {
                    timer += JITimer.Instance.DeltTime;
                    yield return null;
                }

                timer = 0f;
            }

            yield return new WaitForSeconds (_fullWindWaitTimeWhenEndShot);
        }

        #endregion

        #region SideWind State

        private void SideWind_Enter ()
        {
            if (_leftWindArea == null || _rightWindArea == null) return;

            StartCoroutine (SideWind_Sequence ());
        }

        private void SideWind_Transition ()
        {
            if (Active == false)
            {
                _fsm.ChangeState (States.None);
                return;
            }

            int next = Random.Range (0, 3);
            int nextOfNext = Random.Range (0, 3);
            while (nextOfNext == next)
            {
                nextOfNext = Random.Range (0, 3);
            }

            States nextState = GetStates (next);
            _secondShotPattern = GetStates (nextOfNext);

            _fsm.ChangeState (nextState, StateTransition.Safe);
        }

        private IEnumerator SideWind_Sequence ()
        {
            yield return StartCoroutine (SideWind_BossMoveToCenter ());
            yield return StartCoroutine (SidenWind_Shot ());
            yield return StartCoroutine (SideWind_BossMove ());
            yield return StartCoroutine (SideWind_Blow ());
        }

        private IEnumerator SideWind_Blow ()
        {
            _leftWindArea.WindActive = true;
            _rightWindArea.WindActive = true;

            float timer = 0f;

            while (timer < _sideWindLastTime)
            {
                timer += JITimer.Instance.DeltTime;

                if (_sideBossMoveSeqInstance != null)
                {
                    _sideBossMoveSeqInstance.Timescale = JITimer.Instance.TimeScale;
                }

                yield return null;
            }

            _leftWindArea.WindActive = false;
            _rightWindArea.WindActive = false;

            SideWind_Transition ();
        }

        private IEnumerator SideWind_BossMoveToCenter ()
        {
            Vector3 downPoint = new Vector3 (_sideWindBossCenter.x,
                BulletDestroyBound.Instance.Down - 1f, _sideWindBossCenter.z);

            var effect = new Effect<Transform, Vector3> ();
            effect.Duration = 1f;
            effect.RetrieveStart = (falcon, lastValue) => falcon.position;
            effect.RetrieveEnd = (falcon) => _sideWindBossCenter;
            effect.OnUpdate = (falcon, value) => falcon.position = value;
            effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);

            var seq = new Sequence<Transform, Vector3> ();
            seq.Reference = _falcon;
            seq.Add (effect);

            _sideBossMoveSeqInstance = Movement.Run (seq);

            yield return new WaitForSeconds (effect.Duration);
        }

        private IEnumerator SideWind_BossMove ()
        {
            Vector3 downPoint = new Vector3 (_sideWindBossCenter.x,
                BulletDestroyBound.Instance.Down - 1f, _sideWindBossCenter.z);
            Vector3 upPoint = new Vector3 (_sideWindBossCenter.x,
                BulletDestroyBound.Instance.Up + 1f, _sideWindBossCenter.z);

            var effect1 = new Effect<Transform, Vector3> ();
            effect1.Duration = 3 * 1f / _sideWindMoveSpeed;
            effect1.RetrieveStart = (falcon, lastValue) => falcon.position;
            effect1.RetrieveEnd = (falcon) => downPoint;
            effect1.OnUpdate = (falcon, value) => falcon.position = value;
            effect1.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);

            var effect2 = new Effect<Transform, Vector3> ();
            effect2.Duration = 1 * 1f / _sideWindMoveSpeed;
            effect2.RetrieveStart = (falcon, lastValue) => upPoint;
            effect2.RetrieveEnd = (falcon) => _sideWindBossCenter;
            effect2.OnUpdate = (falcon, value) => falcon.position = value;
            effect2.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);

            var seq = new Sequence<Transform, Vector3> ();
            seq.Reference = _falcon;
            seq.Add (effect1);
            seq.Add (effect2);

            _sideBossMoveSeqInstance = Movement.Run (seq);

            yield return new WaitForSeconds (effect1.Duration + effect2.Duration);
        }

        private IEnumerator SidenWind_Shot ()
        {
            var winds = new WindArea[] { _fullScreeWindArea, _leftWindArea, _rightWindArea };

            float timer = 0f;
            float deltAngle = _sideWindSecRange / _sideWindNumberPerSector;
            float startAngle = 270f - _sideWindSecRange / 2 + deltAngle / 2;

            for (int i = 0; i < _sideWindSecNumber; i++)
            {
                for (int j = 0; j < _sideWindNumberPerSector; j++)
                {
                    float angle = startAngle + deltAngle * j;
                    Quaternion rotation = Quaternion.Euler (0, 0, angle - 90f);

                    float rad = angle * Mathf.Deg2Rad;
                    Vector3 vec = new Vector3 (Mathf.Cos (rad), Mathf.Sin (rad), 0f) * _sideWindBulletVec;

                    Transform bullet = GetBullet (_sideWindBullet, _falcon.position, Quaternion.identity);
                    bullet.GetComponent<JIBulletController> ()
                        .Shot (WindBulletMove.LinearWindMove (bullet, vec, Vector3.zero, winds));
                }

                while (timer < _sideWindShotInterval)
                {
                    timer += JITimer.Instance.DeltTime;
                    yield return null;
                }

                timer = 0f;
            }

            yield return new WaitForSeconds (_fullWindWaitTimeWhenEndShot);
        }

        #endregion

        #region  ShotOne State

        private void ShotOne_Enter ()
        {
            if (_falcon == null || _shotOneBullet == null || _shotOneEmitter == null) return;
            if (_shotOneCircleMoveTime < 0.1f) return;
            if (_shotOneCircleRadius < 0.1f) return;

            StartCoroutine (ShotOne_ShotEmitter ());
        }

        private IEnumerator ShotOne_ShotEmitter ()
        {
            var winds = new WindArea[] { _fullScreeWindArea, _leftWindArea, _rightWindArea };

            var emitterOne = Instantiate (_shotOneEmitter, _falcon.position, Quaternion.identity);
            var emitterTwo = Instantiate (_shotOneEmitter, _falcon.position, Quaternion.identity);

            float emitOneRad = 0;
            float emitTwoRad = Mathf.PI;

            Vector3 emitOnePos = new Vector3 (Mathf.Cos (emitOneRad), Mathf.Sin (emitOneRad), 0) *
                _shotOneCircleRadius + _falcon.position;
            Vector3 emitTwoPos = new Vector3 (Mathf.Cos (emitTwoRad), Mathf.Sin (emitTwoRad), 0) *
                _shotOneCircleRadius + _falcon.position;

            float movTimer = 0f;
            float shoTimer = 0;

            // Move emitters to the start of the circle
            while (movTimer < 0.5f)
            {
                movTimer += JITimer.Instance.DeltTime;
                emitterOne.transform.position = Vector3.Lerp (_falcon.position, emitOnePos, movTimer / 0.5f);
                emitterTwo.transform.position = Vector3.Lerp (_falcon.position, emitTwoPos, movTimer / 0.5f);
                yield return null;
            }
            movTimer = 0f;

            // Move emitters around the boss
            while (movTimer < _shotOneCircleMoveTime)
            {
                movTimer += JITimer.Instance.DeltTime;
                shoTimer += JITimer.Instance.DeltTime;

                emitOneRad = movTimer * _shotOneCircleMoveSpeed;
                emitTwoRad = Mathf.PI - movTimer * _shotOneCircleMoveSpeed;

                emitOnePos = new Vector3 (Mathf.Cos (emitOneRad), Mathf.Sin (emitOneRad), 0) *
                    _shotOneCircleRadius + _falcon.position;
                emitTwoPos = new Vector3 (Mathf.Cos (emitTwoRad), Mathf.Sin (emitTwoRad), 0) *
                    _shotOneCircleRadius + _falcon.position;

                emitterOne.transform.position = emitOnePos;
                emitterTwo.transform.position = emitTwoPos;

                if (shoTimer > _shotOneShotInterval - 0.01f)
                {
                    ShotOne_ShotBullet (emitOneRad, emitOnePos, winds);
                    ShotOne_ShotBullet (emitTwoRad, emitTwoPos, winds);
                    shoTimer = 0;
                }

                yield return null;
            }

            Destroy (emitterOne);
            Destroy (emitterTwo);

            yield return new WaitForSeconds (0.5f);

            ShotOne_Transition ();
        }

        private void ShotOne_ShotBullet (float emitRad, Vector3 emitPos, WindArea[] winds)
        {
            var bulletRotate = Quaternion.Euler (0, 0, emitRad * Mathf.Rad2Deg);
            var bulletSpeed = new Vector3 (Mathf.Cos (emitRad), Mathf.Sin (emitRad), 0) * _shotOneBulletSpeed;
            var bullet = GetBullet (_shotOneBullet, emitPos, bulletRotate);
            bullet.GetComponent<JIBulletController> ()
                .Shot (WindBulletMove.LinearWindMove (bullet.transform, bulletSpeed, Vector3.zero, winds));
        }

        private void ShotOne_Transition ()
        {
            if (Active == false)
            {
                _fsm.ChangeState (States.None);
                return;
            }

            if (_fsm.LastState == States.FullScreenWind || _fsm.LastState == States.SideWind)
            {
                _fsm.ChangeState (_secondShotPattern, StateTransition.Safe);
            }
            else
            {
                if (_lastStateIsFullWind)
                {
                    _fsm.ChangeState (States.SideWind, StateTransition.Safe);
                    _lastStateIsFullWind = false;
                }
                else
                {
                    _fsm.ChangeState (States.FullScreenWind, StateTransition.Safe);
                    _lastStateIsFullWind = true;
                }
            }
        }

        #endregion

        #region  ShotTwo State

        private void ShotTwo_Enter ()
        {
            if (_falcon == null) return;
            if (_shotTwoCirBullet == null || _shotTwoSecBullet == null) return;
            if (_shotTwoSecBulletNum < 0) return;
            if (_shotTwoCirMinEmitInterval < 0 || _shotTwoCirMaxEmitInterval < _shotTwoCirMinEmitInterval)
                return;
            if (_shotTwoSecMinEmitInterval < 0 || _shotTwoSecMaxEmitInterval < _shotTwoSecMinEmitInterval)
                return;
            if (_shotTwoCirMaxBulletSpeed < _shotTwoCirMinBulletSpeed)
                return;
            if (_shotTwoSecMaxBulletSpeed < _shotTwoSecMinBulletSpeed)
                return;

            _shotTwoShotCircleBullet = true;

            StartCoroutine (ShotTwo_CircleShot ());
            StartCoroutine (ShotTwo_SectorShot ());
        }

        private void ShotTwo_Transition ()
        {
            if (Active == false)
            {
                _fsm.ChangeState (States.None);
                return;
            }

            if (_fsm.LastState == States.FullScreenWind || _fsm.LastState == States.SideWind)
            {
                _fsm.ChangeState (_secondShotPattern, StateTransition.Safe);
            }
            else
            {
                if (_lastStateIsFullWind)
                {
                    _fsm.ChangeState (States.SideWind, StateTransition.Safe);
                    _lastStateIsFullWind = false;
                }
                else
                {
                    _fsm.ChangeState (States.FullScreenWind, StateTransition.Safe);
                    _lastStateIsFullWind = true;
                }
            }
        }

        private IEnumerator ShotTwo_SectorShot ()
        {
            float angle = Random.Range (0, 360);

            for (int i = 0; i < 4; i++)
            {
                StartCoroutine (ShotTwo_SecDirShot (angle));

                yield return new WaitForSeconds (0.5f);

                angle += 90f;
            }

            _shotTwoShotCircleBullet = false;

            yield return new WaitForSeconds (2f);

            ShotTwo_Transition ();
        }

        private IEnumerator ShotTwo_SecDirShot (float angle)
        {
            var winds = new WindArea[] { _fullScreeWindArea, _leftWindArea, _rightWindArea };
            float secRange = 60f;

            for (int i = 0; i < _shotTwoSecBulletNum; i++)
            {
                float speed = Random.Range (_shotTwoSecMinBulletSpeed, _shotTwoSecMaxBulletSpeed);
                float bulletAngle = angle + Random.Range (-secRange / 2, secRange / 2);
                float bulletRad = bulletAngle * Mathf.Deg2Rad;
                Vector3 vSpeed = new Vector3 (Mathf.Cos (bulletRad), Mathf.Sin (bulletRad), 0) * speed;

                var rotation = Quaternion.Euler (0, 0, bulletAngle - 90f);
                var bullet = GetBullet (_shotTwoSecBullet, _falcon.position, rotation);
                bullet.GetComponent<JIBulletController> ()
                    .Shot (WindBulletMove.LinearWindMove (bullet.transform, vSpeed, Vector3.zero, winds));

                float interval = Random.Range (_shotTwoSecMinEmitInterval, _shotTwoSecMaxEmitInterval);
                yield return new WaitForSeconds (interval);
            }
        }

        private IEnumerator ShotTwo_CircleShot ()
        {
            var winds = new WindArea[] { _fullScreeWindArea, _leftWindArea, _rightWindArea };

            while (true)
            {
                if (!_shotTwoShotCircleBullet) yield break;

                float angle = Random.Range (0, 360);
                float rad = angle * Mathf.Deg2Rad;
                float speed = Random.Range (_shotTwoCirMinBulletSpeed, _shotTwoCirMaxBulletSpeed);
                Vector3 vSpeed = new Vector3 (Mathf.Cos (rad), Mathf.Sin (rad), 0) * speed;

                var rotation = Quaternion.Euler (0, 0, angle - 90f);
                var bullet = GetBullet (_shotTwoCirBullet, _falcon.position, rotation);
                bullet.GetComponent<JIBulletController> ()
                    .Shot (WindBulletMove.LinearWindMove (bullet.transform, vSpeed, Vector3.zero, winds));

                float interval = Random.Range (_shotTwoCirMinEmitInterval, _shotTwoCirMaxEmitInterval);
                yield return new WaitForSeconds (interval);
            }
        }

        #endregion

        #region ShotThree State

        private IEnumerator ShotThree_Enter ()
        {
            if (_player == null || _falcon == null) yield break;
            if (_shotThreeBullet == null) yield break;
            if (_shotThreeNumberPerCircle <= 0) yield break;
            if (_shotThreeShotTimes <= 0) yield break;
            if (_shotThreeInitVec < 0 || _shotThreeAccel > 0) yield break;
            if (_shotThreeMinShotInteral < 0f || _shotThreeMinShotInteral > _shotThreeMaxShotInterval) yield break;
            if (_shotThreeMinMoveDis < 0f || _shotThreeMinMoveDis > _shotThreeMaxMoveDis) yield break;

            for (int i = 0; i < _shotThreeShotTimes; i++)
            {
                float moveTime = Random.Range (_shotThreeMinShotInteral, _shotThreeMaxShotInterval);
                Vector3 dest = ShotThree_FindPropPos ();

                var moveEffect = new Effect<Transform, Vector3> ();
                moveEffect.Duration = moveTime;
                moveEffect.RetrieveStart = (falcon, lastValue) => falcon.position;
                moveEffect.RetrieveEnd = (falcon) => dest;
                moveEffect.OnUpdate = (falcon, pos) => falcon.position = pos;
                moveEffect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2InOut);

                var moveSeq = new Sequence<Transform, Vector3> ();
                moveSeq.Reference = _falcon;
                moveSeq.Add (moveEffect);
                moveSeq.OnComplete = (falcon) => ShotThree_Shot ();

                var moveInstance = Movement.Run (moveSeq);
                while (moveTime > 0)
                {
                    moveInstance.Timescale = JITimer.Instance.TimeScale;
                    moveTime -= JITimer.Instance.DeltTime;
                    yield return null;
                }
            }

            if (_fsm.LastState == States.FullScreenWind || _fsm.LastState == States.SideWind)
            {
                yield return new WaitForSeconds (2f);
            }

            ShotThree_Transition ();
        }

        private void ShotThree_Transition ()
        {
            if (Active == false)
            {
                _fsm.ChangeState (States.None);
                return;
            }

            if (_fsm.LastState == States.FullScreenWind || _fsm.LastState == States.SideWind)
            {
                _fsm.ChangeState (_secondShotPattern, StateTransition.Safe);
            }
            else
            {
                if (_lastStateIsFullWind)
                {
                    _fsm.ChangeState (States.SideWind, StateTransition.Safe);
                    _lastStateIsFullWind = false;
                }
                else
                {
                    _fsm.ChangeState (States.FullScreenWind, StateTransition.Safe);
                    _lastStateIsFullWind = true;
                }
            }
        }

        private void ShotThree_Shot ()
        {
            var winds = new WindArea[] { _fullScreeWindArea, _leftWindArea, _rightWindArea };

            float delt = 360 / _shotThreeNumberPerCircle;

            for (int i = 0; i < _shotThreeNumberPerCircle; i++)
            {
                float angle = i * delt;
                float rad = angle * Mathf.Deg2Rad;
                Vector3 speed = new Vector3 (Mathf.Cos (rad), Mathf.Sin (rad), 0f) * _shotThreeInitVec;
                Vector3 accel = new Vector3 (Mathf.Cos (rad), Mathf.Sin (rad), 0f) * _shotThreeAccel;

                var bullet = GetBullet (_shotThreeBullet, _falcon.position, Quaternion.identity);
                bullet.GetComponent<JIBulletController> ()
                    .Shot (WindBulletMove.LinearWindMove (bullet.transform, speed, accel, winds));
            }
        }

        /// <summary>
        /// Find a appropriate destion position for falcon
        /// </summary>
        /// <returns></returns>
        private Vector3 ShotThree_FindPropPos ()
        {
            float rad = Random.Range (0, 2 * Mathf.PI);
            float dis = Random.Range (_shotThreeMinMoveDis, _shotThreeMaxMoveDis);
            Vector3 dest = _falcon.position + new Vector3 (Mathf.Cos (rad), Mathf.Sin (rad), 0f) * dis;

            Vector3 boundSize = BulletDestroyBound.Instance.Size;
            Vector3 boundCenter = BulletDestroyBound.Instance.Center;

            while (Mathf.Abs (dest.x - boundCenter.x) > (boundSize.x * 0.8f) ||
                Mathf.Abs (dest.y - boundCenter.y) > (boundSize.y * 0.7f) ||
                dest.y < boundCenter.y)
            {
                rad = Random.Range (0, 2 * Mathf.PI);
                dis = Random.Range (_shotThreeMinMoveDis, _shotThreeMaxMoveDis);
                dest = _falcon.position + new Vector3 (Mathf.Cos (rad), Mathf.Sin (rad), 0f) * dis;
            }

            return dest;
        }

        #endregion

        private void OnDrawGizmosSelected ()
        {
            // Draw Shot One State Circle
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere (_falcon.position, _shotOneCircleRadius);

        }

        /// <summary>
        /// Get a valid wind affected bullet from the pool. Return null if pool is full.
        /// </summary>
        private Transform GetBullet (GameObject bulletPrefab, Vector3 position = default (Vector3), Quaternion rotation = default (Quaternion))
        {
            var bullet = BulletPool.Instance.GetGameObject (
                bulletPrefab, position, rotation).transform;

            if (bullet == null) return null;
            if (bullet.GetComponent<JIBulletProperty> () == null)
                bullet.gameObject.AddComponent<JIBulletProperty> ();
            if (bullet.GetComponent<JIBulletController> () == null)
                bullet.gameObject.AddComponent<JIBulletController> ();

            return bullet;
        }

        private States GetStates (int stateNum)
        {
            stateNum = Mathf.Clamp (stateNum, 0, 2);

            switch (stateNum)
            {
                case 0:
                    return States.ShotOne;
                case 1:
                    return States.ShotTwo;
                case 2:
                    return States.ShotThree;
            }

            return States.None;
        }

    }
}