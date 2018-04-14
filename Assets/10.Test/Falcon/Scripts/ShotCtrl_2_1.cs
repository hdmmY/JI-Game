using System;
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
        using Sirenix.OdinInspector;

        public class ShotCtrl_2_1 : MonoBehaviour
        {
            public ShotCtrl_2_2 NextShotReference;

            [Space]

            [SerializeField] private List<SectorShot> SectorShots;

            private void OnEnable ()
            {
                for (int i = 0; i < SectorShots.Count; i++)
                {
                    SectorShots[i].MoveType = Easing.Pow4Out;
                }
            }

            public void Shot ()
            {
                foreach (var secShot in SectorShots)
                {
                    secShot.Shot ();
                }

                if (NextShotReference.BulletsToSpeedUp == null)
                    NextShotReference.BulletsToSpeedUp = new List<GameObject> ();

                foreach (var secShot in SectorShots)
                {
                    foreach (var bullet in secShot.Bullet)
                    {
                        NextShotReference.BulletsToSpeedUp.Add (bullet);
                    }
                }
            }

            private void OnDrawGizmosSelected ()
            {
                foreach (var secShot in SectorShots)
                {
                    secShot.DrawGizmos ();
                }
            }

            [Serializable]
            internal class SectorShot
            {
                /// <summary>
                /// Prefab bullet gameobject to instantiate.
                /// </summary>
                public GameObject BulletPrefab;

                /// <summary>
                /// Bullet that of this shot
                /// </summary>
                public GameObject[] Bullet { get; private set; }

                public Transform BossTrans;

                /// <summary>
                /// Sector center angle. Start from x-positive-axis, anticlockwise 
                /// </summary>
                [Range (0, 300)]
                public float CenterAngle;

                /// <summary>
                /// Sector angle range, must be positive
                /// </summary>
                [Range (0, 300)]
                public float AngleRange;

                /// <summary>
                /// Line number in sector
                /// </summary>
                public int LineNumber;

                /// <summary>
                /// Line length, must be positive
                /// </summary>
                [Range (0, 5)]
                public float LineLength;

                /// <summary>
                /// Bullet Move Time
                /// </summary>
                public float MoveTime;

                public Func<float, float> MoveType;

                public void Shot ()
                {
                    Bullet = new GameObject[LineNumber];

                    float angleDelt = AngleRange / LineNumber;
                    float startAngle = CenterAngle - AngleRange / 2 + angleDelt / 2;;

                    for (int i = 0; i < LineNumber; i++)
                    {
                        float angle = angleDelt * i + startAngle;
                        angle *= Mathf.Deg2Rad;
                        Vector2 center = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
                        center *= LineLength;
                        center += (Vector2) BossTrans.position;

                        Bullet[i] = BulletPool.Instance.GetGameObject (
                            BulletPrefab, BossTrans.position, Quaternion.identity);

                        var effect = new Effect<Transform, Vector3> ();
                        effect.Duration = MoveTime;
                        effect.OnUpdate = (trans, value) => trans.position = value;
                        effect.RetrieveStart = (trans, lastValue) => trans.position;
                        effect.RetrieveEnd = (trans) => center;
                        effect.CalculatePercentDone = MoveType;

                        var seq = new Sequence<Transform, Vector3> ();
                        seq.Reference = Bullet[i].transform;
                        seq.Add (effect);

                        Movement.Run (seq);
                    }
                }

                public void DrawGizmos ()
                {
                    Gizmos.color = Color.yellow;

                    float angleDelt = AngleRange / LineNumber;
                    float startAngle = CenterAngle - AngleRange / 2 + angleDelt / 2;

                    for (int i = 0; i < LineNumber; i++)
                    {
                        float angle = angleDelt * i + startAngle;
                        angle *= Mathf.Deg2Rad;
                        Vector2 center = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
                        center *= LineLength;
                        center += (Vector2) BossTrans.position;
                        Gizmos.DrawCube (center, Vector3.one * 0.15f);
                    }
                }
            }

        }
    }
}