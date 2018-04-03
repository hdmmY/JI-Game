using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// /////////////////////////////////////////////////////////////////////////////////////////
//                             MOVEMENT DEMO #2 "TAG WITH CUBES"
// Created by Teal Rogers
// Trinary Software
// All rights preserved
// For questions or support contact trinaryllc@gmail.com
//
// In order to demonstrate the functionality of the movement control I've put together this 
// simple game. Try to touch the 15 cubes as quickly as possible, but the faster you click
// the more difficult things become. This demo is not about cluttering your project with a 
// bunch of art assets. It's about movement, so please ignore the use of primitive shapes.
// 
// There's a bunch of code related to setting up the game at the top of the file. To understand
// more about the movement control just skip to the next comment block. The only important thing
// here to remember is the line just below this box: "using MovementEffects;"
// /////////////////////////////////////////////////////////////////////////////////////////
using MovementEffects;

namespace MovementDemos
{
    public class Demo2Controller : MonoBehaviour
    {
        public Button FirstButton;
        public Rigidbody Ball;
        public Transform BelowTransform;
        public Material RedMaterial;
        public Material GreenMaterial;
        public Text TimerText;
        public GameObject WinPrompt;
        public GameObject FallPrompt;
        public GameObject LosePrompt;
        public float Angriness = -0.8f;
        public float ColorChangeSpeed = 0.25f;
        public float AverageAutoClickTime = 10f;
        public RectTransform ButtonArea;
        public float ButtonMovementInertia = 1f;

        public class ButtonContainer
        {
            public Button Button;
            public Image Image;
            public bool Moving = false;
            public float PerlinOffset;
        }

        private List<ButtonContainer> buttons = new List<ButtonContainer>();
        private float _nextClickTime;
        private int _cubesHit = 0;
        private const int TotalCubes = 15;
        private Vector3 _cameraHome;
        private Vector3 _cameraHomeDirection;

        void Start()
        {
            Random.InitState(Mathf.FloorToInt(Time.realtimeSinceStartup*50000f));
            _cameraHome = Camera.main.transform.position;
            _cameraHomeDirection = Camera.main.transform.rotation.eulerAngles;

            CreateNewButton(null);

            StartCoroutine(CheckForWin());
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit raycastResult;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastResult))
                {
                    if (raycastResult.transform != BelowTransform)
                    {
                        MoveBall((raycastResult.point - Ball.transform.position).normalized);
                    }
                }
            }

            if (_nextClickTime <= Time.time)
            {
                _nextClickTime = Random.Range(Time.time, Time.time + (AverageAutoClickTime * 2f));
                ButtonActivated(buttons[Mathf.FloorToInt(Random.Range(0f, buttons.Count - 0.01f))]);
            }

            for(int i=0;i < buttons.Count;i++)
            {
                float buttonEvilness = (Mathf.PerlinNoise(Time.time * ColorChangeSpeed, buttons[i].PerlinOffset) - 0.2f) * Angriness;
                if (buttonEvilness >= 0f)
                {
                    buttons[i].Image.color = new Color(1f, 1f - buttonEvilness, 1f - buttonEvilness);
                }
                else
                {
                    buttons[i].Image.color = new Color(1f + buttonEvilness, 1f, 1f + buttonEvilness);
                }
            }
        }

        private IEnumerator CheckForWin()
        {
            while (_cubesHit < TotalCubes)
            {
                TimerText.text = Time.time.ToString("0.00");

                yield return new WaitForSeconds(0.1f);
            }

            WinPrompt.SetActive(true);
        }

        private ButtonContainer CreateNewButton(ButtonContainer buttonToCopy)
        {
            ButtonContainer newContainer;
            if (buttonToCopy == null)
            {
                newContainer = new ButtonContainer
                {
                    Button = FirstButton,
                    Image = FirstButton.GetComponent<Image>(),
                    PerlinOffset = Random.Range(-100f, 100f)
                };
            }
            else
            {
                GameObject newButton = Instantiate(buttonToCopy.Button.gameObject) as GameObject;
                newButton.transform.SetParent(buttonToCopy.Button.transform.parent);
                newButton.transform.localScale = buttonToCopy.Button.transform.localScale;
                newButton.transform.localPosition = buttonToCopy.Button.transform.localPosition;
                newButton.transform.rotation = buttonToCopy.Button.transform.rotation;
                newButton.name = buttonToCopy.Button.name;

                newContainer = new ButtonContainer
                {
                    Button = newButton.GetComponent<Button>(),
                    Image = newButton.GetComponent<Image>(),
                    PerlinOffset = Random.Range(-100f, 100f)
                };
            }
            newContainer.Button.onClick.AddListener(delegate { ButtonActivated(newContainer); });

            buttons.Add(newContainer);

            return newContainer;
        }

        private void DeleteButton(ButtonContainer buttonContainer)
        {
            if (buttons.Count > 1)
            {
                buttons.Remove(buttonContainer);
                Destroy(buttonContainer.Button.gameObject, 5f);
                buttonContainer.Image = null;
            }
        }

        public void CubeHit(GameObject cube, Collider otherObjectCollider)
        {
            var meshRenderer = cube.GetComponent<MeshRenderer>();
            if (meshRenderer.sharedMaterial == RedMaterial)
            {
                meshRenderer.sharedMaterial = GreenMaterial;
                _cubesHit++;
            }
        }

        public void FloorHit(Collider otherObjectCollider)
        {
            FallPrompt.SetActive(true);
            LosePrompt.SetActive(!WinPrompt.activeSelf);
        }

// ///////////////////////////////////////////////////////////////////////////////////
// Ok, at this point we're going to start using the movement control. Two simple helper
// functions are included in the movement control: Movement.DelayedCall and 
// Movement.CallContinuously. Below I show the second one. Here I continuously call 
// the IncreaseAnger and DecreaseAnger functions every frame for the amount of time 
// that is passed in (1 second in this case). Increase and Decrease anger just move 
// the Angriness value up and down by the amount of time since the last frame. The result 
// is that it goes up or down by 1 per second, and since we are calling it every frame 
// then each of these calls raise or lower it by approximately 1. Each call here is 
// independent, so 20 calls to CallContinously will stack on top of each other irrespective 
// of the timing.
// 
// So why not just add 1 to the value and be done with it? Well, the colors of the 
// buttons are linked to the Angriness value. If the value changed by 1 unit between 
// one frame and the next then the buttons would look like they were flashing. We 
// want a smoother color change.
// ///////////////////////////////////////////////////////////////////////////////////

        public void ButtonActivated(ButtonContainer buttonContainer)
        {
            float buttonEvilness = (Mathf.PerlinNoise(Time.time * ColorChangeSpeed, buttonContainer.PerlinOffset) - 0.2f) * Angriness;

            if (buttonEvilness > 0f)
            {
                DoSomethingEvil(buttonContainer, buttonEvilness);
                StartCoroutine(_DecreaseAnger(1f));
            }
            else
            {
                DoSomethingGood(buttonContainer, -buttonEvilness);
                StartCoroutine(_IncreaseAnger(1f));
            }
        }

        private IEnumerator _IncreaseAnger(float timespan)
        {
            float startTime = Time.time;
            while(Time.time + timespan < startTime)
            {
                Angriness += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator _DecreaseAnger(float timespan)
        {
            float startTime = Time.time;
            while(Time.time + timespan < startTime)
            {
                Angriness -= Time.deltaTime;
                yield return null;
            }
        }

        private void DoSomethingGood(ButtonContainer buttonContainer, float goodness)
        {
            if (buttonContainer.Image == null)
            {
                return;
            }

// ///////////////////////////////////////////////////////////////////////////////////
// At this point the buttons are green and happy and they want to do something nice,
// so they're going to shrink if they're large or disappear completely if they're small
// enough.
// 
// Here we create an effect called shrink. The Effect structure is the core of the 
// movement control. There are a lot of options in the Effect structure which, when
// combined, allow you to create practically any type of movement you can imagine on
// practically any type of object.
// 
// To describe the effect below: the effect starts with the button's width, and ends 
// with the width minus some amount. The third function is where you define what actually 
// happens each frame. This is like the glue that takes the value which is being changed 
// each frame and turns it into a nice change on some object. In this case it sets the 
// width and height of the button to "value".
// 
// I have a line in the delete function for a button that always sets buttonContainer.Image
// to null when deleting. I do that so I can check the reference and cancel all the effects
// on that button when it has been deleted. The ContinueIf action here does the actual checking.
// Just before calling the OnUpdate action it evaluates whether button.Image is not null. If
// that expression ever returns false then the rest of the effect is skipped.
//
// Once we have the effect defined, we do Movement.Run to put it into motion. I'm commenting
// out the "Run" line in this block so I can run the effect in the next block.
// ////////////////////////////////////////////////////////////////////////////////////
            if(buttonContainer.Image.rectTransform.sizeDelta.x - (goodness * 200f) > 22f)
            {
                Effect<ButtonContainer, Vector2> shrinkEffect = new Effect<ButtonContainer, Vector2>();
                shrinkEffect.Duration = 0.5f;
                shrinkEffect.RunEffectUntilTime = MovementOverTime.ContinueUntilTime;
                shrinkEffect.RetrieveStart = delegate(ButtonContainer container, Vector2 lastEndVal)
                {
                    return container.Image.rectTransform.sizeDelta;
                };
                shrinkEffect.RetrieveEnd = delegate(ButtonContainer container)
                {
                    return container.Image.rectTransform.sizeDelta - Vector2.one * Mathf.Max((goodness * 200f), 22f);
                };
                shrinkEffect.OnUpdate = delegate(ButtonContainer container, Vector2 value)
                {
                    container.Image.rectTransform.sizeDelta = value;
                };

                Sequence<ButtonContainer, Vector2> shrinkSequence = new Sequence<ButtonContainer, Vector2>();
                shrinkSequence.Reference = buttonContainer;
                shrinkSequence.Effects = new Effect<ButtonContainer, Vector2>[1];
                shrinkSequence.Effects[0] = shrinkEffect;
                shrinkSequence.ContinueIf = delegate(ButtonContainer container)
                {
                    return container.Image != null;
                };

                //Movement.Run(shrinkSequence);

// ///////////////////////////////////////////////////////////////////////////////////
// Ok, That's how to define and run a sequence using only 24 lines of code! However,
// we can make that a LOT better. Just below this block I'm going to redefine the exact 
// same effect in a much more compact way. 
// 
// First off I'm going to use the CreateEffect helper function, which initializes most 
// of the variables to default values and allows you to set the 5 essential variables 
// that create the effect.
// 
// The first variable is a reference to the object that you will be performing this effect
// on. This value is passed back to each of the actions. It's important to use this 
// variable properly! More on that in a second.
//
// The second variable is the length of time that this effect should last (in seconds).
// 
// The last four variables are each actions that you can do (actions are another name for
// function delegates). They are RetrieveStart, RetrieveEnd, OnUpdate, and CancelIf. 
// There is also an optional 5th delegate for OnDone, we'll use that in a later effect.
// 
// If you use lambdas or delegates like we do here then the compiler will let you access 
// variables which are scoped outside of the function you are working with at that moment. 
// However, if you access variables that have scope outside of the lambda expression you are
// using then this is called a full closure, which forces the compiler to save a snapshot
// of the function's context for *every call* of the lambda expression. For the OnUpdate
// function this can potentially be 60 context variables generated per second, per effect.
// This will slow down your app and cause the garbage collector to engage often.
// It's easy to avoid all this overhead though if you just remember to always pass the variable
// you are working on into the effect and use the locally scoped version of the variable
// in your accessor functions.
// 
// As an example, in this case the compiler would let me access the buttonContainer 
// variable directly in the three lambda expressions below, but I choose to pass the 
// reference into the first field of CreateEffect and then get it back as the 
// "button" variable. "button" and "buttonContainer" are actually the exact same variable,
// even though the compiler requires them to have different names so it knows which one 
// you are referring to. Doing this avoids full closures.. mostly. If you look closely
// you might notice that I'm using the "goodness" variable in RetrieveEnd, which creates
// a full closure in that function. I let this pass since RetireveEnd is only called once
// per effect. The third delegate, OnUpdate, should *never* be written with a closure.
// This is important enough that the Movement Effects libarary actually checks for closures
// in OnUpdate before running any effect and will produce a warning if you create one.
//
// Lastly, I can save 3 lines by passing the effect directly into Run without wrapping 
// it in a sequence first. 
//
// Down to 6 lines! (Or 2 if you're being technical.) That's much better.
// ///////////////////////////////////////////////////////////////////////////////////
                var shrink = new Sequence<ButtonContainer, Vector2>(new Effect<ButtonContainer, Vector2>(0.5f,
                    (container, lastEndVal) => container.Image.rectTransform.sizeDelta,
                    (container) => container.Image.rectTransform.sizeDelta - Vector2.one * Mathf.Max((goodness * 200f), 22f),
                    (container, value) => container.Image.rectTransform.sizeDelta = value))
                    { ContinueIf = (container) => container.Image != null };

                Movement.Run(buttonContainer, shrink);
            }
            

// ///////////////////////////////////////////////////////////////////////////////////
// This is almost the same effect, but with an added delete. The delete is achieved by 
// supplying a value for OnDone (I supply the DeleteButton function, which is defined above).
// 
// The last difference is that rather than querying the width of the button for StartVal
// I'm using the lastEndVal, which I'm supplying as an argument during Movement.Run. In
// this case that change has no effect, but soon we'll start chaining effects together,
// and in an effect chain you typically want to start with the end value of the last 
// effect. So it's a good idea to get in the practice now.
// ////////////////////////////////////////////////////////////////////////////////////
            else if (buttons.Count > 1)
            {
                const float newSizeInCanvasPixels = 2f;

                var delete = new Effect<ButtonContainer, Vector2>(0.5f,
                    (container, lastEndVal) => lastEndVal,
                    (container) => Vector2.one * newSizeInCanvasPixels,
                    (container, value) => container.Image.rectTransform.sizeDelta = value,
                    DeleteButton);

                var deleteSequence = new Sequence<ButtonContainer, Vector2>(buttonContainer, delete);
                deleteSequence.RetrieveSequenceStart = (container) => buttonContainer.Image.rectTransform.sizeDelta;
                deleteSequence.ContinueIf = (container) => container.Image != null;

                Movement.Run(deleteSequence);

            }
            else
            {
                Angriness = 1.8f;
            }
        }

        private void DoSomethingEvil(ButtonContainer buttonContainer, float evilness)
        {

// ///////////////////////////////////////////////////////////////////////////////////
// DoSomethingEvil gets called when the buttons are angry at the user for paying too 
// much attention to the ball and not enough attention to them.
// 
// I'm going to start by defining an effect that will move the button from its starting
// point to a random place on the screen and take two seconds to do it. I'm defining
// this effect fresh every time we enter the DoSomethingEvil function because this allows
// me to just change it at future points of this function and simply use the changed 
// version.
// 
// An effect is a very lightweight object. You would only start noticing an overhead
// for creating them if you were doing it by the thousands.
// ///////////////////////////////////////////////////////////////////////////////////
            var moveButton = new Effect<ButtonContainer, Vector2>(2f,
                (container, lastEffectEnd) => lastEffectEnd,
                (container) => new Vector2(Random.Range(ButtonArea.rect.xMin * 1.4f, ButtonArea.rect.xMax * 1.4f),
                    Random.Range(ButtonArea.rect.yMin * 1.4f, ButtonArea.rect.yMax * 1.4f)),
                (container, value) => container.Image.rectTransform.anchoredPosition = value);

// ///////////////////////////////////////////////////////////////////////////////////
// Now I'm going to add moveButton to a moveSequence so I can change a value which is 
// normally left at 0 by default: The sequence's Inertia. You can modify any of the fields 
// of an effect or a sequence at any time. When you call Run it will use the values 
// as they are at that moement.
// 
// The inertia value smooths the value that is returned to OnUpdate. Higher values
// mean more smoothing, and zero means no smoothing. Inertia will smooth out any rapid
// changes in direction, and by itself it will never overshoot.
// ///////////////////////////////////////////////////////////////////////////////////
            var moveSequence = new Sequence<ButtonContainer, Vector2>
                (buttonContainer, moveButton, (container) => container.Image.rectTransform.anchoredPosition);
            moveSequence.Inertia = ButtonMovementInertia;
            moveSequence.ContinueIf = (container) => container.Image != null;

// ///////////////////////////////////////////////////////////////////////////////////
// I could add something like a 1 second wait to give the button a chance to settle into 
// it's final position. However, if the button ends up moving really fast then a one 
// second wait may not be enough. In fact, it uasually wouldn't be enough. What I really 
// want to do is to stop running when the effect gets within some threshold of the end value. 
// 
// I can accomplish this behavior by setting the RunEffectUntilValue action. The effect
// will keep running so long as RunEffectUntilValue returns true. By default an effect
// will use RunEffectUnitlTime and checks to see if time is out of bounds, but if you 
// define RunEffectUntilValue then that action will be used instead. So lets make 
// it check to see if we're near the end location. In this case 4 canvas pixels should 
// be close enough. Always use sqrMagnitude whenever possible, so 4^2 = 16.
//
// I added the moveButton to a sequence in the block above, but when you add an effect 
// to a sequence it adds a copy of the effect. If I only updated the moveButton variable 
// with the new logic at this point then the version that was already in the sequence 
// would use the old logic that stops immedeately at stopTime. So in order to make sure 
// that all moveButton effects now use this new logic, I need to change the effect that's 
// in the sequence as well the independent one. Fortunately, I can do that: The sequence 
// has an array of effects, and the one effect that we have added so far will be at 
// element zero.
// ///////////////////////////////////////////////////////////////////////////////////
            moveButton.RunEffectUntilValue = 
                (curVal, startVal, endVal) => (endVal - curVal).sqrMagnitude > 16f;

            moveSequence.Effects[0].RunEffectUntilValue = moveButton.RunEffectUntilValue;

// ///////////////////////////////////////////////////////////////////////////////////
// We'll start with the most evil thing a button can do: duplicate itself. The CreateNewButton
// function handles the actual act of duplication but I want to also move the new button
// to a random location on the screen after it's created. Rather than creating a new
// move effect for the duplicate button I'm just going to retarget the effect that I 
// just defined above onto the new button. This is really simple to do, all I have to
// do is change the Reference variable to buttonCopy and the entire effect has been 
// retargeted.
// 
// If two different move effects start fighting over the position of the button then 
// things will start jumping around on the screen. That's a good thing to remember:
// If you start seeing things jump around on the screen it's probably because more
// than one effect is trying to move that object at the same time. 
// 
// I'm solving this problem by setting a "Moving" flag to true in the ButtonContainer
// that I'm moving. I'm then defining the OnDone function for the effect to set Moving 
// to false on whichever button container is passed in.
// //////////////////////////////////////////////////////////////////////////////////
            if (evilness > 0.8f)
            {
                ButtonContainer buttonCopy = CreateNewButton(buttonContainer);

                buttonCopy.Moving = true;
                moveSequence.Reference = buttonCopy;
                moveSequence.Effects[0].OnDone = button => button.Moving = false;

                Movement.Run(moveSequence);
            }

// ///////////////////////////////////////////////////////////////////////////////////
// Ok, time for something new. Here I'm going to start a sequence which I'll call "dance".
// I'm setting OnComplete for the sequence (rather then the OnDone for the effect like I did
// above) to set the Moving flag to false once the entire sequence is done. 
// 
// Btw, the OnComplete function for the sequence can also be used to make a repeating sequence
// (if that's what you want) by setting it to something like "delegate { Movement.Run(yourself); }". 
// Or you can use a lambda expression: 
//       () => { Movement.Run(yourself); } or () => Movement.Run(yourself)
//
// The {} brackets can usually be omitted for single line lambda expressions. Anything
// with more than one line requires brackets though.
//
// The OnDone and OnComplete functions have slightly different function signatures. The 
// OnDone function for an effect passes in the effect's Reference, but references 
// are attached to effects and not sequences, so the OnComplete function for the sequence
// passes in the reference to the instance that just ended. I'm not using instance 
// references until a later tutorial, so I'm just ignoring them for now. Since I don't 
// have access to a local version of the effect's reference in the sequence's OnDone, 
// I'll need to reference the buttonContainer variable directly here. This unfortunately 
// produces another full closure, but OnDone only gets called once so it isn't too bad. 
// Just make sure to never create a full closure in the OnUpdate function.
// 
// Here I'm just adding the moveButton effect 5 times in a row, but I'm doing it in 
// several different ways for demonstration. Any method will work, pick your favorite!
// //////////////////////////////////////////////////////////////////////////////////
            else if (evilness > 0.5f && buttonContainer.Moving == false)
            {
                buttonContainer.Moving = true;

                Sequence<ButtonContainer, Vector2> dance = new Sequence<ButtonContainer, Vector2>
                    (buttonContainer, moveButton, container => container.Image.rectTransform.anchoredPosition);
                dance.ContinueIf = (container) => container.Image != null;
                dance.OnComplete = (container) => container.Moving = false;
                dance.Inertia = ButtonMovementInertia;

                dance = dance + moveButton + moveButton;
                dance += moveButton;
                dance.Add(moveButton);

// ///////////////////////////////////////////////////////////////////////////////////
// You can also use the multiplication operator. Just for demonstration I'm throwing out
// the dance variable that I just defined and redefining it using moveButton * 5.
//
// moveButton is an effect, so moveButton * 5 creates a sequence of effects that looks
// like this: { moveButton, moveButton, moveButton, moveButton, moveButton } 
// 
// Since we're using actions the call to Random that we put in the RetrieveEnd field 
// will return a new random number each time moveButton is run.
// ///////////////////////////////////////////////////////////////////////////////////
                Sequence<ButtonContainer, Vector2> dance2 = moveButton * 5;
                dance2.Reference = buttonContainer;
                dance2.ContinueIf = (container) => container.Image != null;
                dance2.RetrieveSequenceStart = (container) => buttonContainer.Image.rectTransform.anchoredPosition;
                dance2.OnComplete = instanceRef => buttonContainer.Moving = false;
                dance2.Inertia = ButtonMovementInertia;

                Movement.Run(dance2);
            }

// ///////////////////////////////////////////////////////////////////////////////////
// Here we have a boring old "move once" effect. It seems too easy after all our fun with
// sequences, right? Well I'm going to make it just a little more interesting by changing
// the logic slightly. The dance sequence ignored a decision branch if buttonContainer.Moving
// was true, but now I'm going to set the effect running but tell this one to hold off 
// until the Moving flag gets set to false.
// 
// I'm doing this by defining the HoldEffectUntil action. HoldEffectUntil returns a bool. 
// All it does is run the action continuously until the action returns true. In this 
// case if I find it false then I want to set it back to true before moving on. (Because 
// this effect moves the button again.)
// ///////////////////////////////////////////////////////////////////////////////////
            else if (evilness < 0.2f)
            {
                moveButton.HoldEffectUntil = (container, waitTimeSoFar) =>
                {
                    if (container.Moving)
                    {
                        return true;
                    }

                    container.Moving = true;
                    return false;
                };

                moveButton.OnDone = button => button.Moving = false;
                moveButton.RetrieveStart = (container, lastEndValue) => container.Image.rectTransform.anchoredPosition;

                Movement.Run(buttonContainer, (Sequence<ButtonContainer, Vector2>)moveButton);
            }
            else
            {

// ///////////////////////////////////////////////////////////////////////////////////
// Ok, if the button can't think of anything more evil to do then it's going to default
// to growing bigger, which is nearly the same operation as the shrink function from
// above. I'm doing it all with one long line this time, since I don't need to save the variable.
// ///////////////////////////////////////////////////////////////////////////////////
                Movement.Run(new Sequence<ButtonContainer, Vector2>(buttonContainer, new Effect<ButtonContainer, Vector2>(0.5f,
                            (container, lastEndVal) => container.Image.rectTransform.sizeDelta,
                            (container) => container.Image.rectTransform.sizeDelta + Vector2.one * (evilness * 30f),
                            (container, value) => container.Image.rectTransform.sizeDelta = value))
                            { ContinueIf = (container) => container.Image != null });
            }
        }

// ///////////////////////////////////////////////////////////////////////////////////
// In this section I'm going to be adding a physics force to the ball for each mouse click.
// The direction that the ball should be pushed has already been calculated in the Update
// function above, so this function is primarily responsible for producing a more pleasing
// "push" effect than would be created by adding a large instantaneous force. I then go 
// on to further annoy the user if Angriness is really high.
// 
// I start out by defining the push effect, which handles a Vector3.
// Effects can run on floats, Vector2, Vector3, Vector4, Color, Rect, and Quaternions
// 
// The effect starts out at one unit and moves up to 15 units per second in the direction
// of the mouse click.
// ///////////////////////////////////////////////////////////////////////////////////
        private void MoveBall(Vector3 forceVector)
        {
            Effect<Rigidbody, Vector3> pushEffect = new Effect<Rigidbody, Vector3>(0.5f,
                (ball, endOfLast) => endOfLast,
                (ball) => forceVector*15f,
                (ball, value) => ball.AddForce(value));

            Sequence<Rigidbody, Vector3> pushBall = new Sequence<Rigidbody, Vector3>(Ball, pushEffect);

// ///////////////////////////////////////////////////////////////////////////////////
// Remember: When running effects on physics objects, (basically any time an object has 
// a rigidbody component and you use AddForce) you need to change the timing from Update 
// to FixedUpdate. This is done on the sequence object.
// 
// I'm also causing Angriness to go up by 0.7 at the end of the push ball sequence.
// ///////////////////////////////////////////////////////////////////////////////////
            pushBall.Segment = MovementOverTime.Segment.FixedUpdate;
            pushBall.OnComplete = ball => StartCoroutine(_IncreaseAnger(0.7f));
            pushBall.RetrieveSequenceStart = ball => forceVector;

// ///////////////////////////////////////////////////////////////////////////////////
// Now I'm appending some number of random movements to the end of the sequence.
// The more angry things are the more wobbly the ball gets!
// 
// Like I did earlier, I'm just changing the RetrieveEnd function of the pushEffect 
// variable to go in a random direction and then appending the effect to the list.
// ///////////////////////////////////////////////////////////////////////////////////
            pushEffect.RetrieveEnd = ball => Random.onUnitSphere * 5f;
            for (float disruption = Angriness; disruption > 2f; disruption -= 1f)
                pushBall += pushEffect;

            Movement.Run(pushBall);

// ///////////////////////////////////////////////////////////////////////////////////
// As a flimsy excuse to show some camera effects, I'm going to create a "camera shake"
// effect and apply it if anger is over 10.
// 
// The shake works by moving the camera one world space unit in a random direction and 
// applying that 6 times quickly. In order to avoid camera drift I'm adding a final 
// jerk that sends the camera back to cameraHome, which is the position of the camera
// that I saved during the Start function.
//
// If you add inertia to the shake effect then it will feel "too real" and make the
// user a lot more uncomfortable, (i.e. nauseous) so I would advise against setting
// inertia for camera shake effects.
// ///////////////////////////////////////////////////////////////////////////////////
            if (Angriness > 10f)
            {
                var jerkCamera = new Effect<Transform, Vector3>(0.08f,
                    (cam, endOfLast) => endOfLast,
                    (cam) => cam.position + Random.onUnitSphere,
                    (cam, value) => cam.position = value);

                var shakeCamera = jerkCamera * 6;
                shakeCamera.Reference = Camera.main.transform;
                shakeCamera.RetrieveSequenceStart = cam => cam.position;

                jerkCamera.RetrieveEnd = cam => _cameraHome;
                shakeCamera += jerkCamera;

                Movement.Run(shakeCamera);
            }

// ///////////////////////////////////////////////////////////////////////////////////
// Just to be extra annoying I'll also change the rotation of the camera if angriness 
// is over 14. This might cause the user to feel vertigo... but the idea is to discourage
// the user from clicking so fast, so it's ok to be a little unpleasant.
// ///////////////////////////////////////////////////////////////////////////////////
            if (Angriness >= 14f)
            {
                var tiltCamera = new Effect<Transform, Quaternion>(Angriness / 100f,
                    (cam, endOfLast) => endOfLast,
                    (cam) => Quaternion.Euler(_cameraHomeDirection + (Random.onUnitSphere * (Angriness - 13f))),
                    (cam, value) => cam.rotation = value);

// ///////////////////////////////////////////////////////////////////////////////////
// Spherical interpolation (Slerp) works better than linear (Lerp) for rotation values. 
// Whereas Lerp works great for normal movement.
//
// Quaternion sequences use spherical interpolation by default, however if you opt to 
// use Vector3s for rotations or direction vectors then you will usually want to set 
// movement space to spherical manually, since Vectors will interpret default as linear. 
// Spherical interpolation will work on all input types except floats and doubles 
// (circles don't exist in 1-demensional space).
//
// I'm setting it to Lerp here because when it comes to shaking the camera (or any
// unexpected/uncontrolled camera movement) a less realistic tilting movement actually
// decreases the feelings of nausia and vertigo. Lineally calculated interpolations also 
// use fewer processor cycles, so in this case they are a better pick. 90% of the time 
// you'll want to spherically interpolate anything that is a 3D rotation or direction 
// vector in your own project. If you spherically interpolate position vectors then you
// will end up orbiting the zero point of your coordinate system.
// ///////////////////////////////////////////////////////////////////////////////////
                tiltCamera.MovementSpace = MovementOverTime.MovementSpace.Linear;

                var twistCamera = tiltCamera * 7;
                twistCamera.Reference = Camera.main.transform;
                twistCamera.RetrieveSequenceStart = cam => cam.rotation;

// ///////////////////////////////////////////////////////////////////////////////////
// Finally, append a "return to home" rotation and run it.
// ///////////////////////////////////////////////////////////////////////////////////
                tiltCamera.RetrieveEnd = cam => Quaternion.Euler(_cameraHomeDirection);
                twistCamera += tiltCamera;

                Movement.Run(twistCamera);
            }
        }
    }
}