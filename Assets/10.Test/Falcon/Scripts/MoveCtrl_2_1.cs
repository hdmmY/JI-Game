using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        using MovementEffects.Extensions;
        using MovementEffects;

        public class MoveCtrl_2_1 : MonoBehaviour
        {
            public Transform BossTrans;

            public Vector3 TargetPoint;

            private void OnEnable ()
            {
                var effect = new Effect<Transform, Vector3> ();
                effect.CalculatePercentDone = Easing.GetEase(Easing.EaseType.Pow2Out);
                effect.Duration = 1f;
                effect.OnUpdate = (boss, value) => boss.position = value;
                effect.RetrieveEnd = (boss) => TargetPoint;
                effect.RetrieveStart = (boss, lastEndValue) => boss.position;

                var seq = new Sequence<Transform, Vector3> ();
                seq.Add (effect);
                seq.Reference = BossTrans;
                seq.OnComplete = (trans) => GetComponent<ShotCtrl_2_1> ().Shot ();

                Movement.Run (seq);
            }

            private void OnDrawGizmosSelected ()
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube (TargetPoint, Vector3.one * 0.15f);
            }
        }
    }
}