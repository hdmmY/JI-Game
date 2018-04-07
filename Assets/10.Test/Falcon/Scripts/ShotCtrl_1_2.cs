using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        using MovementEffects.Extensions;
        using MovementEffects;

        public class ShotCtrl_1_2 : MonoBehaviour
        {
            /// <summary>
            /// Boss Gameobject reference
            /// </summary>
            public Transform BossTrans;

            /// <summary>
            /// Shot bullet prefab
            /// </summary>
            public GameObject BulletPrefab;

            public int EmitTimes = 3;

            public float EmitInterval = 3f;

            /// <summary>
            /// Shotted bullets number per side
            /// </summary>
            [Range (0, 10)]
            public int NumPerSide;

            /// <summary>
            /// First bullet horizontal length
            /// </summary>
            public float StartLength = 0.2f;
            /// <summary>
            /// Horizontal interval between each bullet
            /// </summary>
            public float DeltLength = 0.1f;

            /// <summary>
            /// First bullet move time
            /// </summary>
            public float StartTime = 0.5f;
            /// <summary>
            /// Time interval between each bullet
            /// </summary>
            public float DeltTime = 0.1f;

            /// <summary>
            /// Wait for a while before shot anim player
            /// </summary>
            public float WaitingTime = 0.5f;

            /// <summary>
            /// Bullet speed when moving aim player
            /// </summary>
            public float AimShotSpeed;

            private void OnDrawGizmosSelected ()
            {
                Gizmos.color = Color.yellow;

                float y = GetComponent<MoveCtrl_1_2> ()?.TargetPoint.y ?? 0f;

                for (int i = 0; i < NumPerSide; i++)
                {
                    for (int sign = -1; sign <= 1; sign += 2)
                    {
                        var endPos = sign * (i * DeltLength + StartLength) * Vector3.right;
                        endPos.y = y;
                        Gizmos.DrawCube (endPos, Vector3.one * 0.05f);
                    }
                }
            }

            public void Emit ()
            {
                StartCoroutine (EmitCoroutine ());
            }

            private IEnumerator EmitCoroutine ()
            {
                float timer = 0f;

                for (int i = 0; i < EmitTimes; i++)
                {
                    EmitOnce ();
                    timer = 0f;
                    while (timer < EmitInterval)
                    {
                        timer += JITimer.Instance.DeltTime;
                        yield return null;
                    }
                }
            }

            private void EmitOnce ()
            {
                Transform player = GameObject.FindGameObjectWithTag ("Player").transform;

                for (int i = 0; i < NumPerSide; i++)
                {
                    for (int sign = -1; sign <= 1; sign += 2)
                    {
                        var bullet = BulletPool.Instance.GetGameObject (
                            BulletPrefab, new Vector3 (0, 100, 0), Quaternion.identity).transform;
                        var endPos = sign * (i * DeltLength + StartLength) * Vector3.right;
                        endPos.y = BossTrans.position.y;
                        var bulletMove = new BulletMove
                        {
                            Bullet = bullet,
                            Player = player,
                            StartPosition = BossTrans.position,
                            FirstEffectEndPosition = endPos,
                            FirstEffectDuration = StartTime + i * DeltTime,
                            SecondEffectHoldTime = (NumPerSide - i) * DeltTime + WaitingTime,
                            SecondEffectSpeed = AimShotSpeed
                        };
                        bulletMove.Move ();
                    }
                }
            }

            internal sealed class BulletMove
            {
                /// <summary>
                /// Bullet reference
                /// </summary>
                public Transform Bullet;

                /// <summary>
                /// Player Reference
                /// </summary>
                public Transform Player;

                public Vector3 StartPosition;

                public Vector3 FirstEffectEndPosition;

                public float FirstEffectDuration;

                public float SecondEffectHoldTime;

                public float SecondEffectSpeed;

                public void Move ()
                {
                    var firstEffect = new Effect<Transform, Vector3> ();
                    firstEffect.Duration = FirstEffectDuration;
                    firstEffect.RetrieveStart = (bullet, lastEndValue) => StartPosition;
                    firstEffect.RetrieveEnd = (bullet) => FirstEffectEndPosition;
                    firstEffect.OnUpdate = (bullet, newValue) => bullet.position = newValue;
                    firstEffect.RunEffectUntilTime = (curTime, stopTime) =>
                        curTime < (SecondEffectHoldTime + FirstEffectDuration);

                    var secondEffect = new Effect<Transform, Vector3> ();
                    secondEffect.Duration = 10 / SecondEffectSpeed;
                    secondEffect.RetrieveEnd = (bullet) =>
                        (Player.position - bullet.position).normalized * 10 + bullet.position;
                    secondEffect.OnUpdate =
                        (bullet, newValue) => bullet.position = newValue;
                    secondEffect.RunEffectUntilValue =
                        (cur, start, end) => !BulletDestroyBound.Instance.OutBound (cur);

                    var sequence = new Sequence<Transform, Vector3> ();
                    sequence.Add (firstEffect);
                    sequence.Add (secondEffect);
                    sequence.Reference = Bullet;
                    sequence.IgnoreUnityTimescale = true;

                    Movement.Run (sequence);
                }
            }
        }
    }
}