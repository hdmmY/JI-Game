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

        private StateMachine<States> _fsm;

        // Variables that used by states
        private Transform _player;

        [SerializeField] private Transform _falcon;

        [SerializeField] private WindArea _fullScreeWindArea;

        [SerializeField] private WindArea _leftWindArea;

        [SerializeField] private WindArea _rightWindArea;

        private bool _lastStateIsFullWind;

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

        #endregion

        #region  SideWind Variables

        /// <summary>
        /// SideWind : Wind last time
        /// </summary>
        [BoxGroup ("Side Wind"), Range (0.1f, 10f), SerializeField]
        private float _sideWindLastTime;

        /// <summary>
        /// SideWind : Boss wait before move
        /// </summary>
        [BoxGroup ("Side Wind"), Range (0.1f, 10f), SerializeField]
        private float _sideWindTimeBeforeMove;

        /// <summary>
        /// SideWind : Boss move speed
        /// </summary>
        [BoxGroup ("Side Wind"), Range (1f, 30f), SerializeField]
        private float _sideWindMoveSpeed;

        /// <summary>
        /// SideWind : Timer for wind last time
        /// </summary>
        private float _sideWindTimer;

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

        [BoxGroup ("ShotTwo"), Range (1, 20), SerializeField]
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

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake ()
        {
            _player = GameObject.FindGameObjectWithTag ("Player").transform;

            _fsm = StateMachine<States>.Initialize (this);
            _fsm.ChangeState (States.ShotOne);

            _lastStateIsFullWind = false;
        }

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

            _fullScreeWindArea.WindActive = true;
            _fullWindTimer = 0f;

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

        private void FullScreenWind_Update ()
        {
            _fullWindTimer += JITimer.Instance.DeltTime;

            if (_fullWindTimer > _fullWindLastTime)
            {
                _fullScreeWindArea.WindActive = false;
                FullScreenWind_Transition ();
            }
        }

        private void FullScreenWind_Transition ()
        {
            switch (Random.Range (0, 3))
            {
                case 0:
                    _fsm.ChangeState (States.ShotOne, StateTransition.Safe);
                    break;
                case 1:
                    _fsm.ChangeState (States.ShotTwo, StateTransition.Safe);
                    break;
                case 2:
                    _fsm.ChangeState (States.ShotThree, StateTransition.Safe);
                    break;
            }
        }

        #endregion

        #region SideWind State

        private void SideWind_Enter ()
        {
            if (_leftWindArea == null || _rightWindArea == null) return;

            _leftWindArea.WindActive = true;
            _rightWindArea.WindActive = true;

            _sideWindTimer = 0f;

            SideWind_BossMove ();
        }

        private void SideWind_Update ()
        {
            if (_sideBossMoveSeqInstance != null)
            {
                _sideBossMoveSeqInstance.Timescale = JITimer.Instance.TimeScale;
            }

            _sideWindTimer += JITimer.Instance.DeltTime;

            if (_sideWindTimer > _sideWindLastTime)
            {
                _leftWindArea.WindActive = false;
                _rightWindArea.WindActive = false;

                SideWind_Transition ();
            }
        }

        private void SideWind_Transition ()
        {
            switch (Random.Range (0, 3))
            {
                case 0:
                    _fsm.ChangeState (States.ShotOne, StateTransition.Safe);
                    break;
                case 1:
                    _fsm.ChangeState (States.ShotTwo, StateTransition.Safe);
                    break;
                case 2:
                    _fsm.ChangeState (States.ShotThree, StateTransition.Safe);
                    break;
            }
        }

        private void SideWind_BossMove ()
        {
            Vector3 endPoint1 = new Vector3 (_falcon.position.x,
                BulletDestroyBound.Instance.Down - 1f, _falcon.position.z);
            Vector3 startPoint2 = new Vector3 (_falcon.position.x,
                BulletDestroyBound.Instance.Up + 1f, _falcon.position.z);
            Vector3 endPoint2 = _falcon.position;

            var effect1 = new Effect<Transform, Vector3> ();
            effect1.Duration = 3 * 1f / _sideWindMoveSpeed;
            effect1.RetrieveStart = (falcon, lastValue) => falcon.position;
            effect1.RetrieveEnd = (falcon) => endPoint1;
            effect1.OnUpdate = (falcon, value) => falcon.position = value;
            effect1.HoldEffectUntil = (falcon, timeHoldSofar) => timeHoldSofar > _sideWindTimeBeforeMove;
            effect1.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);

            var effect2 = new Effect<Transform, Vector3> ();
            effect2.Duration = 1 * 1f / _sideWindMoveSpeed;
            effect2.RetrieveStart = (falcon, lastValue) => startPoint2;
            effect2.RetrieveEnd = (falcon) => endPoint2;
            effect2.OnUpdate = (falcon, value) => falcon.position = value;
            effect2.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);

            var seq = new Sequence<Transform, Vector3> ();
            seq.Reference = _falcon;
            seq.Add (effect1);
            seq.Add (effect2);

            _sideBossMoveSeqInstance = Movement.Run (seq);
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
                .Shot (LinearWindBulletMove (bullet.transform, bulletSpeed, Vector3.zero, winds));
        }

        private void ShotOne_Transition ()
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

        private IEnumerator ShotTwo_SectorShot ()
        {
            var winds = new WindArea[] { _fullScreeWindArea, _leftWindArea, _rightWindArea };

            float angle = Random.Range (0, 360);
            float secRange = 30f;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < _shotTwoSecBulletNum; j++)
                {
                    float speed = Random.Range (_shotTwoSecMinBulletSpeed, _shotTwoSecMaxBulletSpeed);
                    float bulletAngle = angle + Random.Range (-secRange / 2, secRange / 2);
                    float bulletRad = bulletAngle * Mathf.Deg2Rad;
                    Vector3 vSpeed = new Vector3 (Mathf.Cos (bulletRad), Mathf.Sin (bulletRad), 0) * speed;

                    var rotation = Quaternion.Euler (0, 0, bulletAngle - 90f);
                    var bullet = GetBullet (_shotTwoSecBullet, _falcon.position, rotation);
                    bullet.GetComponent<JIBulletController> ()
                        .Shot (LinearWindBulletMove (bullet.transform, vSpeed, Vector3.zero, winds));

                    float interval = Random.Range (_shotTwoSecMinEmitInterval, _shotTwoSecMaxEmitInterval);
                    yield return new WaitForSeconds (interval);
                }

                angle += 90f;
            }

            _shotTwoShotCircleBullet = false;

            ShotTwo_Transition ();
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
                    .Shot (LinearWindBulletMove (bullet.transform, vSpeed, Vector3.zero, winds));

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

            ShotThree_Transition ();
        }

        private void ShotThree_Transition ()
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
                    .Shot (LinearWindBulletMove (bullet.transform, speed, accel, winds));
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

        #region Wind Bullet Move

        private IEnumerator LinearWindBulletMove (Transform bullet, Vector3 speed, Vector3 accel, WindArea[] winds)
        {
            if (bullet == null)
            {
                Debug.LogWarning ("The shooting bullet is not exist!");
                yield break;
            }

            while (true)
            {
                var curAccel = accel;

                if (winds != null && winds.Length > 0)
                {
                    foreach (var wind in winds)
                    {
                        curAccel += wind.AffectByWind (bullet.position) ? wind.WindForce : Vector3.zero;
                    }
                }

                speed += curAccel * JITimer.Instance.DeltTime;
                bullet.position += speed * JITimer.Instance.DeltTime;

                yield return null;
            }
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

    }
}