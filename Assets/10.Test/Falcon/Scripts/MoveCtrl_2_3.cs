using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        using MovementEffects.Extensions;
        using MovementEffects;

        public class MoveCtrl_2_3 : MonoBehaviour
        {
            public Transform BossTrans;

            public float TimeBeforeMove = 0.5f;

            [BoxGroup ("Way1")]
            public Vector3 Way1EndPoint;
            [BoxGroup ("Way1")]
            public float Way1Time;
            [BoxGroup ("Way1")]
            public Easing.EaseType EaseType1;

            [BoxGroup ("Way2")]
            public Vector3 Way2StartPoint;
            [BoxGroup ("Way2")]
            public Vector3 Way2EndPoint;
            [BoxGroup ("Way2")]
            public float Way2Time;
            [BoxGroup ("Way2")]
            public Easing.EaseType EaseType2;

            private void OnEnable ()
            {
                var effect1 = new Effect<Transform, Vector3> ();
                effect1.RetrieveStart = (boss, lastValue) => boss.position;
                effect1.RetrieveEnd = (boss) => Way1EndPoint;
                effect1.Duration = Way1Time;
                effect1.HoldEffectUntil = (boss, time) => time > TimeBeforeMove;
                effect1.OnUpdate = (boss, value) => boss.position = value;
                effect1.CalculatePercentDone = Easing.GetEase (EaseType1);

                var effect2 = new Effect<Transform, Vector3> ();
                effect2.RetrieveStart = (boss, lastValue) => Way2StartPoint;
                effect2.RetrieveEnd = (boss) => Way2EndPoint;
                effect2.Duration = Way2Time;
                effect2.OnUpdate = (boss, value) => boss.position = value;
                effect2.CalculatePercentDone = Easing.GetEase (EaseType2);

                var sequence = new Sequence<Transform, Vector3> ();
                sequence.Add (effect1);
                sequence.Add (effect2);
                sequence.Reference = BossTrans;

                Movement.Run (sequence);
            }

            private void OnDrawGizmos ()
            {
                if (BossTrans == null) return;

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine (BossTrans.position, Way1EndPoint);
                Gizmos.DrawLine (Way2StartPoint, Way1EndPoint);
            }
        }
    }
}