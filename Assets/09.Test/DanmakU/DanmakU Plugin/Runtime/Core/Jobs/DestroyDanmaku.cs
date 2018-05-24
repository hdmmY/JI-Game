using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace DanmakU
{

    internal struct DestroyDanmaku : IJob
    {

        NativeArray<int> ActiveCountArray;
        NativeArray<float> Times;
        NativeArray<Vector2> Positions;
        NativeArray<Vector2> OldPositions;
        NativeArray<float> Rotations;
        NativeArray<float> Speeds;
        NativeArray<float> AngularSpeeds;
        NativeArray<Vector4> Colors;

        public DestroyDanmaku (DanmakuPool pool)
        {
            ActiveCountArray = pool.activeCountArray;
            Times = pool.Times;
            Positions = pool.Positions;
            OldPositions = pool.OldPositions;
            Rotations = pool.Rotations;
            Times = pool.Times;
            Speeds = pool.Speeds;
            AngularSpeeds = pool.AngularSpeeds;
            Colors = pool.Colors;
        }

        public unsafe void Execute ()
        {
            var activeCount = Mathf.Max (0, ActiveCountArray[0]);
            var timePtr = (float * ) Times.GetUnsafeReadOnlyPtr ();
            for (var i = 0; i < activeCount; i++)
            {
                if ( * (timePtr++) >= 0) continue;
                activeCount--;
                Times[i] = Times[activeCount];
                Positions[i] = Positions[activeCount];
                OldPositions[i] = OldPositions[activeCount];
                Rotations[i] = Rotations[activeCount];
                Speeds[i] = Speeds[activeCount];
                AngularSpeeds[i] = AngularSpeeds[activeCount];
                Colors[i] = Colors[activeCount];
            }
            ActiveCountArray[0] = activeCount;
        }

    }

}