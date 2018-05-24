using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Boss.Falcon
{
    using MonsterLove.StateMachine;
    using MovementEffects.Extensions;
    using MovementEffects;

    public class FalconStageThree : MonoBehaviour
    {
        public enum States
        {
            None = 0,
            Fake,
            WindPause,
            ShotOne,
            ShotTwo,
            ShotThree,
            Empty
        }

        private States CurrentState
        {
            get
            {
                if (_fsm != null) return _fsm.State;
                return States.None;
            }
        }

        public StateMachine<States> _fsm;

        private Transform _player;

        [SerializeField] private Transform _falcon;

        private WindArea _fullScreenWind;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable ()
        {
            _player = GameObject.FindGameObjectWithTag ("Player").transform;

            _fsm = StateMachine<States>.Initialize (this);
        }

        #region Empty State

        private void Empty_Enter ()
        {
            _fsm.ChangeState (_fsm.LastState);
        }

        #endregion

        #region Fake State Variables

        private GameObject _fakeBoss;

        private GameObject _fakeStateBullet;

        private float _fakeStateDashSpeed;

        private float _fakeStateInitEmitBulletNum;

        private float _fakeBossCircleBulletNum;

        private float _fakeBossCircleBulletSpeed;

        private float _fakeBossCircleRadius;

        #endregion

        #region Fake State

        #endregion

        #region WindPause State Variables

        [BoxGroup ("WindPause"), SerializeField]
        private GameObject _windPauseStateBulletPrefab;

        [BoxGroup ("WindPause"), Range (0.1f, 10f), SerializeField]
        private float _windPauseTime;

        [BoxGroup ("WindPause"), Range (0.1f, 10f), SerializeField]
        private float _windPauseStateMoveSpeed;

        [BoxGroup ("WindPause"), SerializeField]
        private Vector3[] _windPauseStateMovePoints;

        [BoxGroup ("WindPause"), Range (0.02f, 1f), SerializeField]
        private float _windPauseStateShotInterval;

        private float _windPauseStatePauseTimer;

        private float _windPauseStateShotTimer;

        private SequenceInstance _windPauseMoveIns;

        #endregion

        #region WindPause State

        private void WindPause_Enter ()
        {
            if (_falcon == null) return;
            if (_windPauseStateBulletPrefab == null) return;
            if (_windPauseTime < 0.01f) return;
            if (_windPauseStateMoveSpeed < 0.01f) return;
            if (_windPauseStateMovePoints == null || _windPauseStateMovePoints.Length < 1) return;

            _windPauseStatePauseTimer = 0f;
            _windPauseStateShotTimer = 0f;

            // Boss Move
            WindPause_Move ();
        }

        private void WindPause_Update ()
        {
            _windPauseStateShotTimer += JITimer.Instance.DeltTime;
            _windPauseStatePauseTimer += JITimer.Instance.DeltTime;

            if (_windPauseStatePauseTimer > _windPauseTime)
            {
                _windPauseMoveIns.StopWith (null);
                WindPause_Transition ();
                return;
            }

            // if(_win)
        }

        private void WindPause_Transition ()
        {

        }

        private void WindPause_Move ()
        {
            var seq = new Sequence<Transform, Vector3> ();
            seq.Reference = _falcon;

            Effect<Transform, Vector3> effect;

            for (int i = 0; i < _windPauseStateMovePoints.Length - 1; i++)
            {
                effect = new Effect<Transform, Vector3> ();
                effect.Duration = (_windPauseStateMovePoints[i] - _windPauseStateMovePoints[i + 1]).magnitude /
                    _windPauseStateMoveSpeed;
                effect.RetrieveStart = (falcon, lastValue) => _windPauseStateMovePoints[i];
                effect.RetrieveEnd = (falcon) => _windPauseStateMovePoints[i + 1];
                effect.OnUpdate = (falcon, pos) => falcon.position = pos;
                seq.Add (effect);
            }
            effect = new Effect<Transform, Vector3> ();
            effect.Duration = (_windPauseStateMovePoints[_windPauseStateMovePoints.Length - 1] -
                _windPauseStateMovePoints[0]).magnitude / _windPauseStateMoveSpeed;
            effect.RetrieveStart = (falcon, lastValue) => lastValue;
            effect.RetrieveEnd = (falcon) => _windPauseStateMovePoints[0];
            effect.OnUpdate = (falcon, pos) => falcon.position = pos;
            seq.Add (effect);

            _windPauseMoveIns = Movement.Run (seq);
            _windPauseMoveIns.Loop = true;
        }

        #endregion
    }
}