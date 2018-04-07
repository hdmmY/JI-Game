using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        using MovementEffects.Extensions;
        using MovementEffects;
        using Unity.Linq;

        public class MoveCtrl_1_3_False : MonoBehaviour
        {

            public Transform BossTrans;

            /// <summary>
            /// First target point
            /// </summary>
            public Vector3 TargetPoint1;

            /// <summary>
            /// First move time 
            /// </summary>
            public float MoveTime1 = 2;

            /// <summary>
            /// End target point
            /// </summary>
            public Vector3 TargetPoint2;

            /// <summary>
            /// Move time from TargetPoint1 to TargetPoint2
            /// </summary>
            public float MoveTime2 = 1.5f;

            private void OnEnable ()
            {
                var effect1 = new Effect<Transform, Vector3> ();
                effect1.Duration = MoveTime1;
                effect1.RetrieveStart = (trans, lastValue) => trans.position;
                effect1.RetrieveEnd = (trans) => TargetPoint1;
                effect1.OnUpdate = (trans, newValue) => trans.position = newValue;

                var effect2 = new Effect<Transform, Vector3> ();
                effect2.Duration = MoveTime2;
                effect2.RetrieveStart = (trans, lastValue) => lastValue;
                effect2.RetrieveEnd = (trans) => TargetPoint2;
                effect2.OnUpdate = (trans, newValue) => trans.position = newValue;
                effect2.CalculatePercentDone = Easing.Pow2In;

                var sequence = new Sequence<Transform, Vector3> ();
                sequence.Add (effect1);
                sequence.Add (effect2);
                sequence.Reference = BossTrans;
                sequence.IgnoreUnityTimescale = true;
                sequence.OnComplete = (trans) =>
                {
                    Debug.Log (true);
                    GetComponent<UbhBaseShot> ().Shot ();
                };
                
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
                Gizmos.DrawLine (startPos, TargetPoint1);
                Gizmos.DrawLine (TargetPoint1, TargetPoint2);
            }
        }
    }
}