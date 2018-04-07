using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        using MovementEffects.Extensions;
        using MovementEffects;
        using Unity.Linq;

        public class MoveCtrl_1_3_True : MonoBehaviour
        {
            public Transform BossTrans;

            public Vector3 TargetPoint;

            public float MoveTime = 2;

            private void OnEnable ()
            {
                var effect = new Effect<Transform, Vector3> ();
                effect.Duration = MoveTime;
                effect.RetrieveStart = (trans, lastValue) => trans.position;
                effect.RetrieveEnd = (trans) => TargetPoint;
                effect.OnUpdate = (trans, newValue) => trans.position = newValue;
                effect.CalculatePercentDone = Easing.Pow2Out;

                var sequence = new Sequence<Transform, Vector3> ();
                sequence.Add (effect);
                sequence.Reference = BossTrans;
                sequence.IgnoreUnityTimescale = true;
                sequence.OnComplete = (trans) => GetComponent<UbhBaseShot> ().Shot ();

                Movement.Run (sequence);
            }

            private void OnDrawGizmosSelected ()
            {
                Vector3 startPos = Vector3.zero;

                foreach (var go in gameObject.BeforeSelf ())
                {
                    if (go.GetComponent<MoveCtrl_1_2> ())
                    {
                        startPos = go.GetComponent<MoveCtrl_1_2> ().TargetPoint;
                        break;
                    }
                }

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine (startPos, TargetPoint);
            }
        }
    }
}