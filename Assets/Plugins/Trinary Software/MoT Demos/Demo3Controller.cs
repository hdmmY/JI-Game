using System.Collections.Generic;
using UnityEngine;
using MovementEffects;
using MEC;

// /////////////////////////////////////////////////////////////////////////////////////////
//                          MOVEMENT DEMO #3 - "THE ROOM"
// Created by Teal Rogers
// Trinary Software
// All rights preserved
// For questions or support contact trinaryllc@gmail.com
// /////////////////////////////////////////////////////////////////////////////////////////


namespace MovementDemos
{
    public class Demo3Controller : MonoBehaviour
    {
        public Transform MyLeaf;
        public Transform BackFace;
        public Material MyMaterial;
        public Material OpaqueMaterial;

        private bool AtRest;

        void Start()
        {
            MyMaterial = MyLeaf.GetComponent<MeshRenderer>().material;
            BackFace.GetComponent<MeshRenderer>().material = MyMaterial;

            FadeIn();
        }

        public void OnTriggerEnter(Collider otherCollider)
        {
            AtRest = true;
        }

        public void OnTriggerExit(Collider otherCollider)
        {
            AtRest = false;
        }

        private void FadeIn()
        {
            var fadeInEffect = new Effect<Demo3Controller, Color>
            {
                Duration = Random.Range(0.3f, 2f),
                RetrieveStart = (me, lastEnd) => me.MyMaterial.color,
                RetrieveEnd = (me) => Color.white,
                OnUpdate = (me, value) => me.MyMaterial.color = value,
                OnDone = (me) =>
                {
                    me.MyLeaf.GetComponent<MeshRenderer>().material = me.OpaqueMaterial;
                    me.BackFace.GetComponent<MeshRenderer>().material = me.OpaqueMaterial;
                    me.WakeUp(me);
                }
            };

            var fadeInSequence = new Sequence<Demo3Controller, Color>(fadeInEffect);

            // Delay based on the position's distance from zero, then fade in.
            Timing.CallDelayed(transform.position.magnitude / 2f, delegate { Movement.Run(this, fadeInSequence); });
        }

        public void WakeUp(Demo3Controller instance)
        {
            var lurch = new Effect<Demo3Controller, Quaternion> 
            {
                Duration = Random.Range(0.12f, 1.2f),
                RetrieveStart = (leaf, lastEnd) => leaf.MyLeaf.localRotation,
                RetrieveEnd = (leaf) => Quaternion.Euler(Random.onUnitSphere * 6.5f),
                OnUpdate = (leaf, value) => leaf.MyLeaf.localRotation = value,
                HoldEffectUntil = (leaf, holdTimeSoFar) => !leaf.AtRest
            };


            Sequence<Demo3Controller, Quaternion> shake = lurch * 6;

            lurch.Duration = 0.15f;
            lurch.RetrieveEnd = (leaf) => Quaternion.Euler(Random.onUnitSphere * 2.8f);
            shake += lurch * 16;

            lurch.Duration = 0.05f;
            shake += lurch * 3;

            shake.Reference = this;
            shake.OnComplete = Fly;

            Movement.Run(shake);
        }

        private void Fly(Demo3Controller instance)
        {
            // Three sequences that will run so long as AtRest == false.
            // #1 faces a random direction.
            Sequence<Demo3Controller, Quaternion> faceRandom = new Sequence<Demo3Controller, Quaternion>
            {
                RetrieveSequenceStart = (me) => me.MyLeaf.localRotation,
                ContinueIf = (me) => !me.AtRest,
                Reference = this,
            };

            faceRandom.Effects = new Effect<Demo3Controller, Quaternion>[]
            {
                new Effect<Demo3Controller, Quaternion>
                {
                    Duration = 3.5f,
                    RetrieveEnd = (me) => Quaternion.Euler(Random.insideUnitSphere * 500f),
                    OnUpdate = (me, value) => me.MyLeaf.localRotation = value
                }
            };

            Movement.Run(faceRandom).Loop = true;

            //#2 spins around in a circle.
            var semiCircle = new Effect<Demo3Controller, Vector2>
            {
                Duration = 3f,
                RetrieveStart = (me, lastEnd) => lastEnd,
                RetrieveEnd = (me) =>
                {   // Funny little math trick to rotate 90 degrees just by swapping numbers around:
                    Vector2 dest = new Vector2(me.MyLeaf.position.z, -me.MyLeaf.position.x).normalized
                                                * Mathf.Pow(me.MyLeaf.position.y / 8f, 2.6f);
                    // If the leaf was sitting at (0, 0) then the .normalized in the above line will 
                    // leave the vector holding (NaN, NaN). NaN means "Not-a-Number". The magnitude of 
                    // NaN is NaN, which evaluates to false in any compairson check, this is why I'm 
                    // checking for "not greater than" rather than "less than" here.
                    if(!(dest.sqrMagnitude > 0.009f))
                        dest = Random.insideUnitCircle * (me.MyLeaf.position.y + 25f);

                    return dest;
                },
                OnUpdate = (me, value) =>
                {
                    Vector3 newPos = me.MyLeaf.position;
                    newPos.x = value.x;
                    newPos.z = value.y;
                    me.MyLeaf.position = newPos;
                },
                MovementSpace = MovementOverTime.MovementSpace.Spherical
            };

            var circle = semiCircle * 4;
            circle.ContinueIf = (me) => !me.AtRest;
            circle.RetrieveSequenceStart = me => new Vector2(me.MyLeaf.position.x, me.MyLeaf.position.z);
            circle.Inertia = 0.5f;
            circle.Elasticity = 0.5f;

            Movement.Run(this, circle).Loop = true;

            // #3 makes the leaf rise into the air.
            Sequence<Demo3Controller, float> rise = new Sequence<Demo3Controller, float>
            {
                Effects = new Effect<Demo3Controller, float>[]
                {
                    new Effect<Demo3Controller, float>
                    {
                        Duration = 30f,
                        // This is the default behavior when you leave RetrieveStart null:
                        //RetrieveStart = (me, lastEnd) => lastEnd,
                        RetrieveEnd = (me) => Random.Range(6.15f, 35f),
                        OnUpdate = (me, value) =>
                        {
                            Vector3 tempPos = me.MyLeaf.position;
                            tempPos.y = value;
                            me.MyLeaf.position = tempPos;
                        }
                    }
                },
                Reference = this,
                RetrieveSequenceStart = me => me.MyLeaf.position.y,
                ContinueIf = (me) => !me.AtRest,
            };

            Movement.Run(rise);

            // Two sequences that will return to home as soon as AtRest == true
            // #1 moves the position
            var approach = new Effect<Demo3Controller, Vector3>
            {
                Duration = 1f,
                RetrieveStart = (me, lastEnd) => me.MyLeaf.localPosition,
                RetrieveEnd = (me) => new Vector3(me.MyLeaf.localPosition.z,
                                        me.MyLeaf.localPosition.y, -me.MyLeaf.localPosition.x) * 0.001f,
                OnUpdate = (me, value) => me.MyLeaf.localPosition = value,
                MovementSpace = MovementOverTime.MovementSpace.Spherical,
                HoldEffectUntil = (me, timeSoFar) => me.AtRest,
            };

            var dock = new Effect<Demo3Controller, Vector3>
            {
                Duration = 0.2f,
                RetrieveStart = (me, lastEnd) => me.MyLeaf.localPosition,
                RetrieveEnd = (me) => Vector3.zero,
                OnUpdate = (me, value) => me.MyLeaf.localPosition = value
            };


            Sequence<Demo3Controller, Vector3> landingSequence = approach + dock;
            landingSequence.Reference = this;
            landingSequence.OnComplete = WakeUp;

            Movement.Run(landingSequence);

            // #2 points forward in local space
            var pointForward = new Sequence<Demo3Controller, Quaternion>(new Effect<Demo3Controller, Quaternion>
            { 
                Duration = 1.1f,
                RetrieveStart = (me, lastEnd) => me.MyLeaf.localRotation,
                RetrieveEnd = (me) => Quaternion.identity,
                OnUpdate = (me, value) => me.MyLeaf.localRotation = value,
                HoldEffectUntil = (me, timeSoFar) => me.AtRest,
            });

            Movement.Run(this, pointForward);
        }
    }
}