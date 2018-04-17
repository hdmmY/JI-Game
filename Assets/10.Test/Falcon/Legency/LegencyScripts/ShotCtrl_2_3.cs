using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        using MovementEffects.Extensions;
        using MovementEffects;

        public class ShotCtrl_2_3 : MonoBehaviour
        {
            public GameObject BulletPrefab;

            public Transform BossTrans;


            [Header("Shots")]
            public UbhBaseShot CenterShot;

            [ListDrawerSettings (ListElementLabelName = "Name", NumberOfItemsPerPage = 30)]
            public List<BulletMove> WindBullets;

            private void OnEnable ()
            {
                Shot ();
            }

            public void Shot ()
            {
                if (BulletPrefab == null || BossTrans == null || WindBullets == null)
                    return;

                foreach (var bullet in WindBullets)
                {
                    bullet.Bullet = BulletPool.Instance.GetGameObject (
                        BulletPrefab, BossTrans.position, Quaternion.identity).transform;
                    bullet.Shot ();
                }

                CenterShot.m_bulletPrefab = BulletPrefab;
                CenterShot.Shot ();
            }

            private void OnDrawGizmosSelected ()
            {
                if (BossTrans == null || WindBullets == null)
                    return;

                foreach (var bullet in WindBullets)
                {
                    bullet.Start = BossTrans.position;
                    bullet.DrawGizmos ();
                }
            }

            [Serializable]
            public class BulletMove
            {
                [HideInInspector]
                public Transform Bullet;

                public string Name;

                [ListDrawerSettings (Expanded = false)]
                public BulletMoveEffect[] Effects;

                [HideInInspector] public Vector3 Start;

                public void Shot ()
                {
                    if (Effects == null || Effects.Length == 0)
                        return;

                    if (Bullet == null)
                        return;

                    var sequence = new Sequence<Transform, Vector3> ();

                    var start = Start;
                    foreach (var effect in Effects)
                    {
                        effect.StartPoint = start;
                        start = effect.EndPoint;
                        sequence.Add (effect.GetEffect ());
                    }
                    sequence.Reference = Bullet;

                    Bullet.position = Start;

                    Movement.Run (sequence);
                }

                public void DrawGizmos ()
                {
                    if (Effects == null || Effects.Length == 0)
                        return;

                    Vector3 start = Start;
                    foreach (var effect in Effects)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawCube (effect.EndPoint, Vector3.one * 0.1f);
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine (start, effect.EndPoint);
                        start = effect.EndPoint;
                        Debug.Log ("Here!");
                    }
                }
            }

            [Serializable]
            public class BulletMoveEffect
            {
                [HideInInspector] public Vector3 StartPoint;

                public Vector3 EndPoint;

                public float TimeBeforeStart;

                public float Duration;

                public Easing.EaseType Ease;

                public Effect<Transform, Vector3> GetEffect ()
                {
                    var effect = new Effect<Transform, Vector3> ();

                    effect.Duration = Duration;
                    effect.RetrieveStart = (bullet, lastValue) => StartPoint;
                    effect.RetrieveEnd = (bullet) => EndPoint;
                    effect.OnUpdate = (bullet, value) => bullet.position = value;
                    effect.HoldEffectUntil = (bullet, time) => time > TimeBeforeStart;
                    effect.CalculatePercentDone = Easing.GetEase (Ease);

                    return effect;
                }
            }
        }
    }
}