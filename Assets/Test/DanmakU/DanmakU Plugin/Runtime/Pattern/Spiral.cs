// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace DanmakU.Fireables
// {
//     [Serializable]
//     public class Spiral : Fireable
//     {
//         public Range Count;

//         [Radians] public Range StartAngle;

//         [Radians]
//         public Range ShiftAngle;

//         public Range Delay;

//         public override void Fire (DanmakuConfig config)
//         {
//             var emitter = new GameObject ("SpireEmitter", typeof (SpiralEmitter)).GetComponent<SpiralEmitter> ();
//             emitter.SetEmitterParams (Count, StartAngle, ShiftAngle, Delay);
//             emitter.Emit (config, Child);
//         }

//         public class SpiralEmitter : MonoBehaviour
//         {
//             private int _count;

//             private Range _startAngle;

//             private float _shiftAngle;

//             private float _delay;

//             private DanmakuConfig _config;

//             private IFireable _subFire;

//             private bool _emit = false;

//             public void SetEmitterParams (int count, float StartAngle, float ShiftAngle, float Delay)
//             {
//                 _count = count;
//                 _startAngle = StartAngle;
//                 _shiftAngle = ShiftAngle;
//                 _delay = Delay;
//             }

//             public void Emit (DanmakuConfig config, IFireable subFire)
//             {
//                 _config = config;
//                 _subFire = subFire;

//                 if (!_emit)
//                 {
//                     _emit = true;
//                     StartCoroutine (EmitCoroutine ());
//                 }
//             }

//             private IEnumerator EmitCoroutine ()
//             {
//                 if (_count <= 0) yield break;

//                 float timer = 0f;

//                 for (int i = 0; i < _count; i++)
//                 {
//                     timer = 0f;
//                     while (timer < _delay)
//                     {
//                         timer += Time.deltaTime;
//                         yield return null;
//                     }

//                     if (_subFire != null)
//                     {
//                         _subFire.Fire (new DanmakuConfig
//                         {
//                             Position = _config.Position,
//                                 Rotation = _startAngle + _shiftAngle * i,
//                                 Speed = _config.Speed,
//                                 AngularSpeed = _config.AngularSpeed,
//                                 Color = _config.Color
//                         });
//                     }
//                 }

//                 Destroy (this.gameObject);
//             }
//         }
//     }
// }