using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        using MovementEffects.Extensions;
        using MovementEffects;

        public class MoveCtrl_1_2 : MonoBehaviour
        {
            /// <summary>
            /// Boss's transform
            /// </summary>
            public Transform BossTrans;

            /// <summary>
            /// Position that boss will stand at
            /// </summary>
            public Vector3 TargetPoint;

            private void OnEnable ()
            {
                var effect = new Effect<Transform, Vector3> ();
                effect.Duration = 2f;
                effect.RetrieveStart = (trans, lastEndValue) => trans.position;
                effect.RetrieveEnd = (trans) => TargetPoint;
                effect.OnUpdate = (trans, newValue) => trans.position = newValue;
                effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2InOut);

                var sequence = new Sequence<Transform, Vector3> ();
                sequence.Add (effect);
                sequence.Reference = BossTrans;
                sequence.IgnoreUnityTimescale = true;
                sequence.OnComplete = (trans) => GetComponent<ShotCtrl_1_2> ().Emit ();

                Movement.Run (sequence);
            }

            private void OnDrawGizmosSelected ()
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube (TargetPoint, Vector3.one * 0.15f);
            }
        }
    }
}