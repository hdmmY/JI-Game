using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Boss.Falcon
{
    using MonsterLove.StateMachine;
    using MovementEffects.Extensions;
    using MovementEffects;

    public class FalconStageOne : MonoBehaviour
    {
        public enum States
        {
            MutiShot,
            MoveShot,
            Expansion,
            Fake,
            Empty // Use for state loop
        }

        private StateMachine<States> fsm;

        // Variables that used by states
        private Transform _player;
        [SerializeField]
        private Transform _falcon;

        /// <summary>
        /// Timer for fake state. Record the no fake state since last fake state
        /// </summary>
        [ShowInInspector, ReadOnly]
        private int _fakeStateTimer;

        #region MutiShot Variables

        [BoxGroup ("MutiShot"), SerializeField]
        private GameObject _mutiShotBullet;

        /// <summary>
        /// MutiShot : Numbers of bullets per circle
        /// </summary>
        [BoxGroup ("MutiShot"), Range (1, 20), SerializeField]
        private int _mutiShotNumberPerCircle = 10;

        /// <summary>
        /// MutiShot : Delay between circles
        /// </summary>
        [BoxGroup ("MutiShot"), Range (0.03f, 1f), SerializeField]

        private float _mutiShotDelay = 0.1f;

        /// <summary>
        /// MutiShot : Bullet init velocity
        /// </summary>
        [BoxGroup ("MutiShot"), Range (0.1f, 5f), SerializeField]
        private float _mutiShotInitVec = 1f;

        /// <summary>
        /// MutiShot : Bullet end velocity
        /// </summary>
        [BoxGroup ("MutiShot"), Range (0.1f, 5f), SerializeField]
        private float _mutiShotEndVec = 0.1f;

        /// <summary>
        /// MutiShot : Bullet negative acceleration
        /// </summary>
        [BoxGroup ("MutiShot"), Range (0.5f, 5f), SerializeField]
        private float _mutiShotAccel = 1f;

        #endregion

        #region Expansion Variables

        [BoxGroup ("Expansion"), SerializeField]
        private GameObject _expanBullet;

        /// <summary>
        /// Expansion State : Boss position
        /// </summary>
        [BoxGroup ("Expansion"), SerializeField]
        private Vector3 _expanBossPos;

        /// <summary>
        /// Expansion State : Bullet number
        /// </summary>
        [BoxGroup ("Expansion"), Range (1, 20), SerializeField]
        private int _expanBulletNum;

        /// <summary>
        /// Expansion State : Shot interval time
        /// </summary>
        [BoxGroup ("Expansion"), Range (0.05f, 1f), SerializeField]
        private float _expanBulletShotInterval;

        /// <summary>
        /// Expansion State : Bullet init distance from boss
        /// </summary>
        [BoxGroup ("Expansion"), Range (0.05f, 2f), SerializeField]
        private float _expanInitDistance;

        /// <summary>
        /// Expansion State : Bullet expansion location interval 
        /// </summary>
        [BoxGroup ("Expansion"), Range (0.1f, 1f), SerializeField]
        private float _expanBulletLocaInterval;

        /// <summary>
        /// Expansion State : Pause time when bullet reach target expansion position
        /// </summary>
        [BoxGroup ("Expansion"), Range (0.1f, 2f), SerializeField]
        private float _expanBulletPauseTime;

        /// <summary>
        /// Expansion State : Bullet speed when bullet expansion
        /// </summary>
        [BoxGroup ("Expansion"), Range (0.1f, 3f), SerializeField]
        private float _expanBulletExpanSpeed;

        /// <summary>
        /// Expansion State : Bullet acceleration when bullet homing shot target
        /// </summary>
        [BoxGroup ("Expansion"), Range (0.1f, 10f), SerializeField]
        private float _expanBulletAccel;

        #endregion

        #region MoveShot Variables

        [BoxGroup ("MoveShot"), SerializeField]
        private GameObject _moveShotBullet;

        /// <summary>
        /// MoveShot State : Move state last time
        /// </summary>
        [BoxGroup ("MoveShot"), Range (0.05f, 10f), SerializeField]
        private float _moveShotTime;

        /// <summary>
        /// MoveShot State : Move shot center position
        /// </summary>
        [BoxGroup ("MoveShot"), SerializeField]
        private Vector3 _moveShotMoveCenter;

        /// <summary>
        /// MoveShot State : Local left shot position
        /// </summary>
        [BoxGroup ("MoveShot"), SerializeField]
        private Vector3 _moveShotShotPosLeft;

        /// <summary>
        /// MoveState State : Local right shot position
        /// </summary>
        [BoxGroup ("MoveShot"), SerializeField]
        private Vector3 _moveShotShotPosRight;

        /// <summary>
        /// MoveShot State : Timer for move
        /// </summary>
        private float _moveShotMoveTimer;

        /// <summary>
        /// MoveShot State : Timer for shot
        /// </summary>
        private float _moveShotShotTimer;

        /// <summary>
        /// MoveShot State : Whether move boss
        /// </summary>
        private bool _moveShotMove;

        #endregion

        #region  Fake Variables

        /// <summary>
        /// Fake State : Fake boss prefab
        /// </summary>
        [BoxGroup ("Fake"), SerializeField]
        private GameObject _fakeBoss;

        /// <summary>
        /// Fake State : Fake boss dash time
        /// </summary>
        [BoxGroup ("Fake"), Range (0.5f, 3f), SerializeField]
        private float _fakeDashTime;

        /// <summary>
        /// Fake State : Wait for a while until translate to next state
        /// </summary>
        [BoxGroup ("Fake"), Range (0.5f, 10f), SerializeField]
        private float _fakeStateWaitTime;

        [BoxGroup ("Fake"), SerializeField]
        private UbhBaseShot _fakeBossShotPattern;

        [BoxGroup ("Fake"), SerializeField]
        private UbhBaseShot _realBossShotPattern;

        private SequenceInstance _fakeBossMoveSeq;

        private SequenceInstance _realBossMoveSeq;

        #endregion

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake ()
        {
            _player = GameObject.FindGameObjectWithTag ("Player").transform;

            fsm = StateMachine<States>.Initialize (this);
            fsm.ChangeState (States.Expansion);
        }

        #region Move State

        private void MoveShot_Enter ()
        {
            if (_player == null || _falcon == null)
            {
                MoveShot_Transition ();
                return;
            }
            if (_moveShotBullet == null)
            {
                MoveShot_Transition ();
                return;
            }
            if (_moveShotTime < 0.1f)
            {
                MoveShot_Transition ();
                return;
            }

            _moveShotMoveTimer = _moveShotTime;
            _moveShotShotTimer = 0f;
            _moveShotMove = true;
        }

        // Follow the player and shot
        private void MoveShot_LateUpdate ()
        {
            var playerProperty = _player.GetComponent<PlayerProperty> ();
            float moveSpeed = playerProperty.m_playerMoveState == PlayerProperty.PlayerMoveType.HighSpeed ?
                playerProperty.m_horizontalSpeed : playerProperty.m_slowHorizontalSpeed;
            float shotSpeed = playerProperty.m_shootInterval;
            float bulletSpeed = playerProperty.m_bulletSpeed;
            float targetX = _player.position.x;

            _moveShotMoveTimer -= JITimer.Instance.DeltTime;
            _moveShotShotTimer -= JITimer.Instance.DeltTime;

            // Shot bullet
            if (_moveShotShotTimer < 0f)
            {
                _moveShotShotTimer = shotSpeed;

                var left = GetBullet (_moveShotBullet, _falcon.TransformPoint (_moveShotShotPosLeft), Quaternion.identity);
                left.GetComponent<JIBulletController> ().Shot (bulletSpeed, 270f, 0, 0,
                    false, null, 0f, 0f,
                    false, 0f, 0f,
                    false, 0f, 0f);

                var right = GetBullet (_moveShotBullet, _falcon.TransformPoint (_moveShotShotPosRight), Quaternion.identity);
                right.GetComponent<JIBulletController> ().Shot (bulletSpeed, 270f, 0, 0,
                    false, null, 0f, 0f,
                    false, 0f, 0f,
                    false, 0f, 0f);
            }

            // Move
            if (_moveShotMoveTimer > 0f)
            {
                float distance = targetX - _falcon.position.x;

                if (Mathf.Abs (distance) <= moveSpeed * JITimer.Instance.DeltTime)
                {
                    _falcon.position = new Vector3 (targetX, _falcon.position.y, _falcon.position.z);
                }
                else
                {
                    _falcon.position += new Vector3 (Mathf.Sign (distance), 0, 0) * moveSpeed * JITimer.Instance.DeltTime;
                }
            }
            else if (_moveShotMove)
            {
                MoveShot_EndMove ();
                _moveShotMove = false;
            }
        }

        private void MoveShot_EndMove ()
        {
            float distance = Mathf.Abs (_falcon.position.x - BulletDestroyBound.Instance.Center.x);

            // Reach map edge
            if (distance > (BulletDestroyBound.Instance.Size.x * 0.7f))
            {
                var effect = new Effect<Transform, Vector3> ();
                effect.Duration = 1.5f;
                effect.RetrieveStart = (falcon, lastValue) => falcon.position;
                effect.RetrieveEnd = (falcon) => _moveShotMoveCenter;
                effect.OnUpdate = (falcon, pos) => falcon.position = pos;
                effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);
                var seq = new Sequence<Transform, Vector3> ();
                seq.Reference = _falcon;
                seq.Add (effect);
                seq.OnComplete = (falcon) => MoveShot_Transition ();
                var seqInstance = Movement.Run (seq);
                seqInstance.Timescale = JITimer.Instance.TimeScale;
            }
            else
            {
                MoveShot_Transition ();
            }
        }

        private void MoveShot_Transition ()
        {
            _fakeStateTimer++;
            if (_fakeStateTimer == 4)
            {
                _fakeStateTimer = 0;
                fsm.ChangeState (States.Fake, StateTransition.Safe);
                return;
            }

            switch (Random.Range (0, 2))
            {
                case 0:
                    fsm.ChangeState (States.MutiShot, StateTransition.Safe);
                    break;
                case 1:
                    fsm.ChangeState (States.Expansion, StateTransition.Safe);
                    break;
            }
        }

        #endregion

        #region  MutiShot State

        /// <summary>
        /// Shot the bullet
        /// </summary>
        private IEnumerator MutiShot_Enter ()
        {
            if (_player == null || _falcon == null) yield break;
            if (_mutiShotBullet == null) yield break;
            if (_mutiShotNumberPerCircle <= 0) yield break;
            if (_mutiShotInitVec <= 0 || _mutiShotInitVec <= _mutiShotEndVec) yield break;
            if (_mutiShotAccel <= 0) yield break;

            for (int i = 0; i < 4; i++)
            {
                float moveTime = Random.Range (0.2f, 0.6f);
                Vector3 dest = MutiShot_FindPropPos ();

                var moveEffect = new Effect<Transform, Vector3> ();
                moveEffect.Duration = moveTime;
                moveEffect.RetrieveStart = (falcon, lastValue) => falcon.position;
                moveEffect.RetrieveEnd = (falcon) => dest;
                moveEffect.OnUpdate = (falcon, position) => falcon.position = position;

                var moveSequence = new Sequence<Transform, Vector3> ();
                moveSequence.Reference = _falcon;
                moveSequence.Add (moveEffect);
                moveSequence.OnComplete = (falcon) => MutiShot_Shot ();

                var moveInstance = Movement.Run (moveSequence);

                while (moveTime > 0f)
                {
                    moveTime -= JITimer.Instance.DeltTime;
                    moveInstance.Timescale = JITimer.Instance.TimeScale;
                    yield return null;
                }
            }

            MutiShot_Transition ();
        }

        private void MutiShot_Transition ()
        {
            _fakeStateTimer++;
            if (_fakeStateTimer == 4)
            {
                _fakeStateTimer = 0;
                fsm.ChangeState (States.Fake, StateTransition.Safe);
                return;
            }

            var distance = Mathf.Abs (_player.position.x - _falcon.position.x);
            if (distance > BulletDestroyBound.Instance.Size.x)
            {
                fsm.ChangeState (States.MoveShot, StateTransition.Safe);
                return;
            }

            switch (Random.Range (0, 2))
            {
                case 0:
                    fsm.ChangeState (States.Expansion, StateTransition.Safe);
                    break;
                case 1:
                    fsm.ChangeState (States.Empty, StateTransition.Safe);
                    break;
            }
        }

        private void MutiShot_Shot ()
        {
            float delt = 360 / _mutiShotNumberPerCircle;

            for (int i = 0; i < _mutiShotNumberPerCircle; i++)
            {
                float angle = i * delt;

                var bullet = GetBullet (_mutiShotBullet, _falcon.position, Quaternion.identity);
                bullet.GetComponent<JIBulletController> ().Shot (MutiShot_BulletMove (bullet, angle));
            }
        }

        private IEnumerator MutiShot_BulletMove (Transform bullet, float angle)
        {
            float initSpeed = _mutiShotInitVec;
            float endSpeed = _mutiShotEndVec;
            float accel = _mutiShotAccel;

            if (bullet == null)
            {
                Debug.LogWarning ("The shooting bullet is not exist!");
                yield break;
            }

            bullet.SetEulerAnglesZ (angle - 90f);

            // Accelerate
            while (initSpeed > endSpeed)
            {
                bullet.position += bullet.up * initSpeed * JITimer.Instance.DeltTime;
                initSpeed -= accel * JITimer.Instance.DeltTime;
                yield return null;
            }

            // Normal move
            while (true)
            {
                bullet.position += bullet.up * endSpeed * JITimer.Instance.DeltTime;
                yield return null;
            }
        }

        /// <summary>
        /// Find a appropriate destion position for falcon
        /// </summary>
        /// <returns></returns>
        private Vector3 MutiShot_FindPropPos ()
        {
            Vector3 dest = _falcon.position + new Vector3 (
                Random.Range (-0.6f, 0.6f), Random.Range (-0.2f, 0.2f), 0f);

            while (BulletDestroyBound.Instance.OutBound (dest) || Mathf.Abs (dest.x) < 0.2f)
            {
                dest = _falcon.position + new Vector3 (
                    Random.Range (-0.6f, 0.6f), Random.Range (-0.2f, 0.2f), 0f);
            }

            return dest;
        }

        #endregion 

        #region Expansion State

        private IEnumerator Expansion_Enter ()
        {
            if (_falcon == null || _player == null) yield break;
            if (_expanBullet == null) yield break;
            if (_expanBulletNum < 1) yield break;
            if (_expanBulletShotInterval < 0.01f) yield break;
            if (_expanBulletLocaInterval < 0) yield break;
            if (_expanBulletPauseTime < 0f) yield break;
            if (_expanBulletExpanSpeed < 0f) yield break;
            if (_expanBulletAccel < 0f) yield break;

            // Move Boss to target position
            var moveTime = 1.5f;
            var moveEffect = new Effect<Transform, Vector3> ();
            moveEffect.Duration = moveTime;
            moveEffect.RetrieveStart = (falcon, lastValue) => falcon.position;
            moveEffect.RetrieveEnd = (falcon) => _expanBossPos;
            moveEffect.OnUpdate = (falcon, pos) => falcon.position = pos;
            moveEffect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);
            var moveSeq = new Sequence<Transform, Vector3> ();
            moveSeq.Add (moveEffect);
            moveSeq.Reference = _falcon;
            moveSeq.OnComplete = (falcon) => StartCoroutine (Expansion_Shot ());
            var moveInstance = Movement.Run (moveSeq);

            while (moveTime > 0)
            {
                moveTime -= JITimer.Instance.DeltTime;
                moveInstance.Timescale = JITimer.Instance.TimeScale;
                yield return null;
            }
        }

        private void Expansion_Finally ()
        {
            StopCoroutine ("Expansion_Shot");
        }

        private void Expansion_Transition ()
        {
            _fakeStateTimer++;
            if (_fakeStateTimer == 4)
            {
                _fakeStateTimer = 0;
                fsm.ChangeState (States.Fake, StateTransition.Safe);
                return;
            }

            var distance = Mathf.Abs (_player.position.x - _falcon.position.x);
            if (distance > BulletDestroyBound.Instance.Size.x)
            {
                fsm.ChangeState (States.MoveShot, StateTransition.Safe);
                return;
            }

            switch (Random.Range (0, 2))
            {
                case 0:
                    fsm.ChangeState (States.MutiShot, StateTransition.Safe);
                    break;
                case 1:
                    fsm.ChangeState (States.Empty, StateTransition.Safe);
                    break;
            }
        }

        /// <summary>
        /// Shot the expansion bullet
        /// </summary>
        private IEnumerator Expansion_Shot ()
        {
            Vector3 destRight;
            Vector3 destLeft;
            float timer = 0f;

            for (int i = _expanBulletNum - 1; i >= 0; i--)
            {
                destRight = _falcon.position + Vector3.right * (_expanInitDistance + i * _expanBulletLocaInterval);
                destLeft = _falcon.position + Vector3.left * (_expanInitDistance + i * _expanBulletLocaInterval);

                var left = GetBullet (_expanBullet, _falcon.position, Quaternion.identity);
                left.GetComponent<JIBulletController> ().Shot (Expansion_BulletMove (left, destLeft));

                var right = GetBullet (_expanBullet, _falcon.position, Quaternion.identity);
                right.GetComponent<JIBulletController> ().Shot (Expansion_BulletMove (right, destRight));

                timer = _expanBulletShotInterval;
                while (timer > 0f)
                {
                    timer -= JITimer.Instance.DeltTime;
                    yield return null;
                }
            }

            yield return new WaitForSeconds (_expanBulletPauseTime);

            Expansion_Transition ();
        }

        private IEnumerator Expansion_BulletMove (Transform bullet, Vector3 dest)
        {
            float expanSpeed = _expanBulletExpanSpeed;
            Vector3 expanDir = (dest - bullet.position).normalized;
            float pauseTime = _expanBulletPauseTime;
            float accel = _expanBulletAccel;

            if (bullet == null)
            {
                Debug.LogWarning ("The shooting bullet is not exist!");
                yield break;
            }

            // Expansion move
            while ((bullet.position - dest).sqrMagnitude > 0.01f)
            {
                bullet.position += expanDir * expanSpeed * JITimer.Instance.DeltTime;
                yield return null;
            }

            // Pause
            while (pauseTime > 0f)
            {
                pauseTime -= JITimer.Instance.DeltTime;
                yield return null;
            }

            // Homing 
            var player = GameObject.FindGameObjectWithTag ("Player").transform.position;
            var accDir = (player - bullet.position).normalized;
            var speed = 3f;
            while (true)
            {
                speed += JITimer.Instance.DeltTime * accel;
                bullet.position += accDir * JITimer.Instance.DeltTime * speed;
                yield return null;
            }
        }

        #endregion

        #region  Fake State

        private void Fake_Enter ()
        {
            if (_falcon == null || _fakeBoss == null) return;
            if (_fakeBossShotPattern == null || _realBossShotPattern == null) return;
            if (_fakeDashTime < 0.1f) return;

            var fakeBoss = Instantiate (_fakeBoss, _falcon.position, _falcon.rotation);

            Vector3 rightPos = new Vector3 (0, 0, _falcon.position.z);
            rightPos.x = BulletDestroyBound.Instance.Center.x + BulletDestroyBound.Instance.Size.x * 0.7f;
            rightPos.y = BulletDestroyBound.Instance.Center.y + BulletDestroyBound.Instance.Size.y * 0.75f;
            Vector3 leftPos = new Vector3 (0, rightPos.y, rightPos.z);
            leftPos.x = BulletDestroyBound.Instance.Center.x - BulletDestroyBound.Instance.Size.x * 0.75f;

            Vector3 realDest = Vector3.zero;
            Vector3 fakeDest = Vector3.zero;
            switch (Random.Range (0, 2))
            {
                case 0:
                    realDest = leftPos;
                    fakeDest = rightPos;
                    break;
                case 1:
                    realDest = rightPos;
                    fakeDest = leftPos;
                    break;
            }

            Fake_FakeBossMove (fakeBoss.transform, fakeDest);
            Fake_RealBossMove (realDest);
        }

        private void Fake_Update ()
        {
            if (_fakeBossMoveSeq != null)
                _fakeBossMoveSeq.Timescale = JITimer.Instance.TimeScale;
            if (_realBossMoveSeq != null)
                _realBossMoveSeq.Timescale = JITimer.Instance.TimeScale;
        }

        private void Fake_Transistion ()
        {
            float distance = Mathf.Abs (_player.position.x - _falcon.position.x);

            if (distance > BulletDestroyBound.Instance.Size.x)
            {
                fsm.ChangeState (States.MoveShot, StateTransition.Safe);
                return;
            }

            switch (Random.Range (1, 2))
            {
                case 1:
                    fsm.ChangeState (States.Expansion, StateTransition.Safe);
                    break;
                case 2:
                    fsm.ChangeState (States.MutiShot, StateTransition.Safe);
                    break;
            }
        }

        private void Fake_FakeBossMove (Transform fakeBoss, Vector3 dest)
        {
            var effect1 = new Effect<Transform, Vector3> ();
            effect1.Duration = 2f;
            effect1.RetrieveStart = (fake, lastValue) => fake.position;
            effect1.RetrieveEnd = (fake) => dest;
            effect1.OnUpdate = (fake, pos) => fake.position = pos;

            var effect2 = new Effect<Transform, Vector3> ();
            effect2.Duration = _fakeDashTime;
            effect2.RetrieveStart = (fake, lastValue) => lastValue;
            effect2.RetrieveEnd = (fake) => new Vector3 (dest.x,
                BulletDestroyBound.Instance.Center.y - BulletDestroyBound.Instance.Size.y * 0.65f, dest.z);
            effect2.OnUpdate = (fake, pos) => fake.position = pos;
            effect2.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow4In);

            var seq = new Sequence<Transform, Vector3> ();
            seq.Add (effect1);
            seq.Add (effect2);
            seq.Reference = fakeBoss;
            seq.OnComplete = (fake) =>
            {
                _fakeBossShotPattern.transform.position = fake.position;
                _fakeBossShotPattern.OnShotFinish = (shotPattern) => Destroy (fake.gameObject);
                _fakeBossShotPattern.Shot ();
            };

            _fakeBossMoveSeq = Movement.Run (seq);
        }

        private void Fake_RealBossMove (Vector3 dest)
        {
            var moveEffect = new Effect<Transform, Vector3> ();
            moveEffect.Duration = 2f;
            moveEffect.RetrieveStart = (falcon, lastValue) => falcon.position;
            moveEffect.RetrieveEnd = (falcon) => dest;
            moveEffect.OnUpdate = (falcon, pos) => falcon.position = pos;
            moveEffect.RunEffectUntilTime = (curTime, stopTime) => curTime < stopTime + _fakeDashTime;
            moveEffect.OnDone = (falcon) =>
            {
                _realBossShotPattern.transform.position = falcon.position;
                _realBossShotPattern.Shot ();
            };

            var pauseEffect = new Effect<Transform, Vector3> ();
            pauseEffect.Duration = _fakeStateWaitTime;
            pauseEffect.OnUpdate = (falcon, pos) => Debug.Log (Time.time);

            var seq = new Sequence<Transform, Vector3> ();
            seq.Add (moveEffect);
            seq.Add (pauseEffect);
            seq.Reference = _falcon;
            seq.OnComplete = (falcon) => Fake_Transistion ();

            _realBossMoveSeq = Movement.Run (seq);
        }

        #endregion

        #region  Empty State

        /// <summary>
        /// Use for state loop
        /// </summary>
        private void Empty_Enter ()
        {
            fsm.ChangeState (fsm.LastState);
        }

        #endregion 

        /// <summary>
        /// Get a valid bullet from the pool. Return null if pool is full.
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