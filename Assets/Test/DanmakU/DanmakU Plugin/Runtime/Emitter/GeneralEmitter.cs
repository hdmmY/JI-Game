using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DanmakU
{
    using DanmakU.Fireables;
    using DanmakU.Modifiers;

    public class GeneralEmitter : DanmakuBehaviour
    {

        public DanmakuPrefab DanmakuType;

        public Range Speed = 5f;

        public Range AngularSpeed;

        public Color Color = Color.white;

        /// <summary>
        /// Emitter will fire <see cref = "DanmakuEmitter.FireRate"/> times per second 
        /// </summary>
        public Range FireRate = 5;

        /// <summary>
        /// If FrameRate is zero, then the emitter's frameRate is relatived to Time.deltTime;
        /// Else the emitter's frameRate is this value.
        /// </summary>
        public float FrameRate;

        /// <summary>
        /// Descirption to create a new fire
        /// </summary>
        [Space, OnValueChanged ("UpdateFireables")]
        [DisableInPlayMode]
        public string FireDescirption;

        /// <summary>
        /// All Fireables that this emitter will fire. The fire order is the same as this list order.
        /// </summary>
        [ShowInInspector]
        [ListDrawerSettings (HideAddButton = true, IsReadOnly = true)]
        [InlineEditor (InlineEditorModes.GUIOnly)]
        public List<Fireable> Fireables;

        /// <summary>
        /// Timer for emitter interval
        /// </summary>
        private float _timer;

        private Fireable _fireable;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start ()
        {
            if (DanmakuType == null)
            {
                Debug.LogWarning ($"Emitter doesn't have a valid DanmakuPrefab", this);
                return;
            }

            var set = CreateSet (DanmakuType);
            set.AddModifiers (GetComponents<IDanmakuModifier> ());

            if (Fireables == null || Fireables.Count == 0)
            {
                Debug.LogWarning ("Emitter doesn't have a valid Fireables", this);
                return;
            }

            _fireable = Fireables[0];
            for (int i = 1; i < Fireables.Count; i++)
            {
                _fireable = _fireable.Of (Fireables[i]);
            }
            _fireable.Of (set);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update ()
        {
            if (_fireable == null) return;

            var deltaTime = Time.deltaTime;
            if (FrameRate > 0)
            {
                deltaTime = 1f / FrameRate;
            }
            _timer -= deltaTime;

            if (_timer < 0)
            {
                _fireable.Fire (new DanmakuConfig
                {
                    Position = transform.position,
                        Rotation = transform.eulerAngles.z * Mathf.Deg2Rad,
                        Speed = Speed,
                        AngularSpeed = AngularSpeed,
                        Color = Color
                });

                _timer = 1f / FireRate.GetValue ();
            }
        }

        /// <summary>
        /// Use fire description to change fireables
        /// </summary>
        private void UpdateFireables ()
        {
            var fires = FireDescirption.ToLower ().Split (
                new char[] { ' ' },
                System.StringSplitOptions.RemoveEmptyEntries);

            Fireables = Fireables ?? new List<Fireable> ();
            Fireables.Clear ();

            foreach (var fire in fires)
            {
                switch (fire)
                {
                    case "arc":
                        Fireables.Add (ScriptableObject.CreateInstance<Arc> ());
                        break;
                    case "circle":
                        Fireables.Add (ScriptableObject.CreateInstance<Circle> ());
                        break;
                    case "line":
                        Fireables.Add (ScriptableObject.CreateInstance<Line> ());
                        break;
                    case "ring":
                        Fireables.Add (ScriptableObject.CreateInstance<Ring> ());
                        break;
                }
            }

            Debug.Log ("Change!");
        }
    }

}