using UnityEngine;
using UnityEngine.UI;

// /////////////////////////////////////////////////////////////////////////////////////////
//                           MOVEMENT DEMO #1 - "SOMETHING SIMPLE"
// Created by Teal Rogers
// Trinary Software
// All rights preserved
// For questions or support contact trinaryllc@gmail.com
//
// This demo creates two sequences which each move a particle system from one point to the next.
// There are sliders in the scene which allow you to change the inertia and springiness values
// for the smoothed (green) line while the other (yellow) line remains unaffected.
// /////////////////////////////////////////////////////////////////////////////////////////
using MovementEffects;
using MovementEffects.Extensions;

namespace MovementDemos
{
    public class Demo1Controller : MonoBehaviour
    {
        public ParticleSystem LinearItem;
        public ParticleSystem SmoothedItem;
        public Text InertiaLabel;
        public Text SpringinessLabel;
        public Text TimescaleLabel;
        public Transform StartPosition;
        public Transform[] Waypoints;
        
        private Sequence<ParticleSystem, Vector3> _linearSequence;
        private Sequence<ParticleSystem, Vector3> _smoothedSequence;
        private float _inertia;
        private float _elasticity;
        private float _timescale = 1f;
        private bool _loop;
        private int _rotatingIndex1;
        private int _rotatingIndex2;

        private SequenceInstance<Vector3> _smoothedInstance;

// /////////////////////////////////////////////////////////////////////////////////////////
// Here is the Start function, in which I initalize the two sequences. An effect is a single
// process (with options), and a sequence is a list of effects (with a few more options).
// 
// Both structures combine to define just about any process you might need to perform. The 
// three most important variables in any effect are the three function delegates: RetrieveStart
// RetrieveEnd, and OnUpdate. In the code below I leave RetrieveEnd as null when defining the
// effect. I only do that because I'm overwriting each RetrieveEnd to retrieve the position from
// a different waypoint in the for loop.
// 
// A third structure is the SequenceInstance, which is used to change values on a sequence while it is 
// running. I'm using the SequenceInstance below to change the values of Inertia and Springiness
// whenever the slider values change. 
// 
// So the sequence below can be read as: Create a moveToWaypoint effect that lerps "LinearItem" 
// from the last effect's end value "lastEnd" to <null> (it's set later in the for loop). 
// Every frame it will set the variable the the effect is working on's transform.postion to a 
// new Vector3 value. Setting the position each frame is how we create movement, and the Movement
// Control takes care of calculating excatly where the new position should be each frame and
// passing that in to the "value" variable.
// 
// I then use moveToWaypoint times <number> to create a sequence that repeats the effect 
// moveToWaypoint <number> times. I set the StartingValue for the sequence to the initial waypoint's 
// position. The starting value is the value that will be passed into "lastEnd" for the first 
// effect that is run on the sequence.
//
// Then I do that for loop I was talking about where I set each effect in the sequence to have
// a different RetrieveEnd that grabs the position of a different index in the Waypoints array.
// You might be asking yourself at this point "what's with the localI variable?" Well, the
// "system => Waypoints[localI].position" is defining a function with retrieves the value at
// a personalized index in an array. I can't use the i varable because the compiler won't store
// the current value for an itterator of a for loop like it would for any other variable. It's
// a b.u.g in the Mono compiler, and storing a local version of the itterator is a workaround.
// 
// I finish up by changing the refrence variable in moveToWaypoint from LinearItem to SmoothedItem
// and constructing the smoothedSequence the same way.
// /////////////////////////////////////////////////////////////////////////////////////////
        private void Start()
        {
            Effect<ParticleSystem, Vector3> moveToWaypoint = new Effect<ParticleSystem, Vector3>();
            moveToWaypoint.Duration = 1f;
            moveToWaypoint.OnUpdate = OnUpdate;
            moveToWaypoint.RetrieveEnd = RetrieveEndLinear;

            _linearSequence = moveToWaypoint * Waypoints.Length;
            _linearSequence.Reference = LinearItem;
            _linearSequence.RetrieveSequenceStart = RetrieveSequenceStart;

            moveToWaypoint.RetrieveEnd = RetrieveEndSmoothed;

            _smoothedSequence = moveToWaypoint * Waypoints.Length;
            _smoothedSequence.Reference = SmoothedItem;
            _smoothedSequence.RetrieveSequenceStart = RetrieveSequenceStart;

            RunTest();

            
        }

        private void OnUpdate(ParticleSystem system, Vector3 value)
        {
            system.transform.position = value;
        }

        private Vector3 RetrieveSequenceStart(ParticleSystem system)
        {
            return StartPosition.position;
        }

        private Vector3 RetrieveEndLinear(ParticleSystem system)
        {
            _rotatingIndex1 = (_rotatingIndex1 + 1) % Waypoints.Length;
            return Waypoints[_rotatingIndex1].position;
        }

        private Vector3 RetrieveEndSmoothed(ParticleSystem system)
        {
            _rotatingIndex2 = (_rotatingIndex2 + 1) % Waypoints.Length;
            return Waypoints[_rotatingIndex2].position;
        }

// /////////////////////////////////////////////////////////////////////////////////////////
// Here is where I run both sequences whenever the "Run Test" button is pressed and also at start.
// 
// Movement.Run starts everything in motion. Here I'm setting the inertia and springiness for
// the smoothed sequence, but leaving it alone on the linear sequence. I'm also saving the 
// SequenceInstance variable for the smoothed sequence that is returned by Sequence.Run. You
// don't need to save that variable unless you want to modify or kill sequences while they are
// running, but I want to modify the smoothed instance so we save it here. I then modify the
// inertia or springiness value in the instance every time I get a "value changed" event from
// one of the sliders.
//
// Feel free to consider this project a standbox. Play around with the positions, set variables,
// etc. I like an inertia of .72 and a springiness of .98. 
// 
// If you want to get a better feel for Slerp then try adding the following line in RunTest:
// _smoothedSequence.MovementSpace = Movement.MovementSpace.Spherical; 
// Slerp moves in spheres and circles around the zero point and is mostly used for rotations.
// /////////////////////////////////////////////////////////////////////////////////////////
        public void RunTest()
        {
            Movement.Run(_linearSequence);

            for (int i = 0; i < _smoothedSequence.Effects.Length; i++)
            {
                _smoothedSequence.Inertia = _inertia;
                _smoothedSequence.Elasticity = _elasticity;
            }

            _smoothedInstance = Movement.Run(_smoothedSequence);
            _smoothedInstance.Timescale = _timescale;
            _smoothedInstance.Loop = _loop;
        }

/// /////////////////////////////////////////////////////////////////////////////////////////
/// Think of inertia as "a bias twords being inert (not moving)." By itself inertia will always
/// lag behind the linear value, and will never overshoot. In this demo you have to set inertia
/// to near the limit before you notice much smoothing, that is because we are using coordinates
/// in the hundreds. If you were running an effect from 0 to 1 you would want to use very small
/// values for inertia.
/// 
/// Springiness is like having a rubber band between the current position and the raw value of the
/// lerp. Springiness will overshoot and then bounce back. The two effects combine well.
/// 
/// Both of these functions add extra computation to the lerp. It's not a huge difference, but
/// you will get slightly better performance when you leave both values at 0.
/// /////////////////////////////////////////////////////////////////////////////////////////
        public void SetInertia(float value)
        {
            _inertia = value;
            InertiaLabel.text = "Inertia: " + value.ToString("0.000");

            if (_smoothedInstance != null)
                _smoothedInstance.Inertia = value;
        }

        public void SetElasticity(float value)
        {
            _elasticity = value;
            SpringinessLabel.text = "Elasticity: " + value.ToString("0.000");

            if (_smoothedInstance != null)
                _smoothedInstance.Elasticity = value;
        }

        public void SetTimescale(float value)
        {
            _timescale = value;
            TimescaleLabel.text = "Timescale: " + value.ToString("0.000");

            if (_smoothedInstance != null)
                _smoothedInstance.Timescale = value;
        }

        public void SetLoop(bool value)
        {
            _loop = value;

            if (_smoothedInstance != null)
                _smoothedInstance.Loop = value;
        }
    }
}