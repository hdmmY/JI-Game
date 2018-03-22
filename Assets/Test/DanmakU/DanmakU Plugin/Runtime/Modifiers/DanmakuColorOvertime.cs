using DanmakU.Modifiers;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace DanmakU
{
    public class DanmakuColorOvertime : MonoBehaviour, IDanmakuModifier
    {
        /// <summary>
        /// Color Change Speed
        /// </summary>
        public float Speed;

        public JobHandle UpdateDannmaku (DanmakuPool pool, JobHandle dependency)
        {
            var deltTime = Speed * Time.deltaTime;

            return new ChangeColorOvertime (pool, deltTime).
                Schedule (pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
        }

        struct ChangeColorOvertime : IJobParallelFor
        {
            NativeArray<Vector4> _colors;

            float _deltTime;

            public ChangeColorOvertime (DanmakuPool Pool, float deltTime)
            {
                _deltTime = deltTime;
                _colors = Pool.Colors;
            }

            public unsafe void Execute (int index)
            {
                var colorPtr = (float * ) _colors.GetUnsafePtr () + index * 4;

                * colorPtr++ = Mathf.Repeat ( * colorPtr + _deltTime, 1);
                * colorPtr++ = Mathf.Repeat ( * colorPtr + _deltTime, 1);
                * colorPtr++ = Mathf.Repeat ( * colorPtr + _deltTime, 1);
                * colorPtr = Mathf.Repeat ( * colorPtr + _deltTime, 1);
            }
        }
    }

}