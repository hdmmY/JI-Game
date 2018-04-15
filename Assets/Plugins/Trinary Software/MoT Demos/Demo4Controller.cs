using MovementEffects;
using MovementEffects.Extensions;
using UnityEngine;

// /////////////////////////////////////////////////////////////////////////////////////////
//                          MOVEMENT DEMO #4 - "GRAPHS"
//
// This demo utilizes some of the additional functionality that is contained in 
// MovementEffects.Extensions. Extensions contains helpers that allow you to construct a 
// sequence using functions like myGameObject.transform.MoTWorldSpaceMoveTo() and it also 
// contains Easing functions which allow you to perform simple movements more easily.
// 
// Created by Teal Rogers
// Trinary Software
// All rights preserved
// For questions or support contact trinaryllc@gmail.com
// /////////////////////////////////////////////////////////////////////////////////////////

namespace MovementDemos
{
    public class Demo4Controller : MonoBehaviour
    {
        public ParticleSystem[] GraphLines;
        public Transform[] Graphs;
        public float DistanceToMoveGraph = 200f;

        private const float GraphLeftSide = -50f;
        private const float GraphRightSide = 50f;
        private const float GraphTop = 46f;
        private const float GraphBottom = -49f;

        void Start ()
        {
            StartGraphsMoving ();
        }

        private void StartGraphsMoving ()
        {
            Effect<Transform, float> bottomTop = new Effect<Transform, float>
            {
                Duration = 2f,
                RetrieveStart = (tform, last) => GraphBottom,
                RetrieveEnd = tform => GraphTop,
                OnUpdate = (tform, val) => tform.localPosition = new Vector3 (tform.localPosition.x, val, tform.localPosition.z)
            };

            Effect<Transform, float> leftRight = new Effect<Transform, float>
            {
                Duration = 2f,
                RetrieveStart = (tform, last) => GraphLeftSide,
                RetrieveEnd = tform => GraphRightSide,
                OnUpdate = (tform, val) => tform.localPosition = new Vector3 (val, tform.localPosition.y, tform.localPosition.z)
            };

            for (int i = 0; i < GraphLines.Length; i++)
            {
                AssignEasingBasedOnIndex (i, ref bottomTop);

                Movement.Run (GraphLines[i].transform, bottomTop).Loop = true;
                Movement.Run (GraphLines[i].transform, leftRight).Loop = true;
            }
        }

        public void Button1 ()
        {
            for (int i = 0; i < Graphs.Length / 2; i += 3)
            {
                var moveThereAndBack = Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position + new Vector3 (DistanceToMoveGraph, 0f, 0f));
                moveThereAndBack += Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position);

                for (int j = 0; j < moveThereAndBack.Effects.Length; j++)
                    AssignEasingBasedOnIndex (i, ref moveThereAndBack.Effects[j]);

                Movement.Run (moveThereAndBack);
            }
        }

        public void Button2 ()
        {
            for (int i = 1; i < Graphs.Length / 2; i += 3)
            {
                var moveThereAndBack = Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position + new Vector3 (DistanceToMoveGraph, 0f, 0f));
                moveThereAndBack += Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position);

                for (int j = 0; j < moveThereAndBack.Effects.Length; j++)
                    AssignEasingBasedOnIndex (i, ref moveThereAndBack.Effects[j]);

                Movement.Run (moveThereAndBack);
            }
        }

        public void Button3 ()
        {
            for (int i = 2; i < Graphs.Length / 2; i += 3)
            {
                var moveThereAndBack = Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position + new Vector3 (DistanceToMoveGraph, 0f, 0f));
                moveThereAndBack += Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position);

                for (int j = 0; j < moveThereAndBack.Effects.Length; j++)
                    AssignEasingBasedOnIndex (i, ref moveThereAndBack.Effects[j]);

                Movement.Run (moveThereAndBack);
            }
        }

        public void Button4 ()
        {
            for (int i = 12; i < Graphs.Length; i += 3)
            {
                var moveThereAndBack = Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position - new Vector3 (DistanceToMoveGraph, 0f, 0f));
                moveThereAndBack += Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position);

                for (int j = 0; j < moveThereAndBack.Effects.Length; j++)
                    AssignEasingBasedOnIndex (i, ref moveThereAndBack.Effects[j]);

                Movement.Run (moveThereAndBack);
            }
        }

        public void Button5 ()
        {
            for (int i = 13; i < Graphs.Length; i += 3)
            {
                var moveThereAndBack = Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position - new Vector3 (DistanceToMoveGraph, 0f, 0f));
                moveThereAndBack += Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position);

                for (int j = 0; j < moveThereAndBack.Effects.Length; j++)
                    AssignEasingBasedOnIndex (i, ref moveThereAndBack.Effects[j]);

                Movement.Run (moveThereAndBack);
            }
        }

        public void Button6 ()
        {
            for (int i = 14; i < Graphs.Length; i += 3)
            {
                var moveThereAndBack = Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position - new Vector3 (DistanceToMoveGraph, 0f, 0f));
                moveThereAndBack += Graphs[i].MoTWorldSpaceMoveTo (4f, Graphs[i].transform.position);

                for (int j = 0; j < moveThereAndBack.Effects.Length; j++)
                    AssignEasingBasedOnIndex (i, ref moveThereAndBack.Effects[j]);

                Movement.Run (moveThereAndBack);
            }
        }

        private static void AssignEasingBasedOnIndex<T> (int index, ref Effect<Transform, T> effect)
        {
            switch (index)
            {
                default : effect.CalculatePercentDone = null;
                break;
                case 0:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.SineIn);
                    break;
                case 1:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.SineOut);
                    break;
                case 2:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.SineInOut);
                    break;
                case 3:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.ElasticIn);
                    break;
                case 4:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.ElasticOut);
                    break;
                case 5:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.ElasticInOut);
                    break;
                case 6:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.BounceIn);
                    break;
                case 7:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.BounceOut);
                    break;
                case 8:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.BounceInOut);
                    break;
                case 9:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.BackIn);
                    break;
                case 10:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.BackOut);
                    break;
                case 11:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.BackInOut);
                    break;
                case 12:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2In);
                    break;
                case 13:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);
                    break;
                case 14:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2InOut);
                    break;
                case 15:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow3In);
                    break;
                case 16:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow3Out);
                    break;
                case 17:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow3InOut);
                    break;
                case 18:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow4In);
                    break;
                case 19:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow4Out);
                    break;
                case 20:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow4InOut);
                    break;
                case 21:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow5In);
                    break;
                case 22:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow5Out);
                    break;
                case 23:
                        effect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow5InOut);
                    break;
            }

        }
    }
}