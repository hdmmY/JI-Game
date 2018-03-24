using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU
{

    internal class MutableDanmakuCollisionList
    {

        const int kDefaultCapacity = 16;
        const int kGrowthFactor = 2;

        Dictionary<Danmaku, DanmakuCollision> Set;

        public int Count => Set?.Count ?? kDefaultCapacity;

        public DanmakuCollisionList AsReadOnly ()
        {
            int count = Set.Count;
            DanmakuCollision[] readonlyData = new DanmakuCollision[count];
            Set.Values.CopyTo (readonlyData, 0);
            return new DanmakuCollisionList (readonlyData, count);
        }

        public MutableDanmakuCollisionList ()
        {
            Set = new Dictionary<Danmaku, DanmakuCollision>
                (kDefaultCapacity, new DanmakUComparer ());
        }

        public void Add (DanmakuCollision obj)
        {
            if (!Set.ContainsKey (obj.Danmaku))
            {
                Set[obj.Danmaku] = obj;
            }
        }

        public void Clear () => Set.Clear ();

        internal class DanmakUComparer : IEqualityComparer<Danmaku>
        {
            bool IEqualityComparer<Danmaku>.Equals (Danmaku x, Danmaku y)
            {
                return x == y;
            }

            int IEqualityComparer<Danmaku>.GetHashCode (Danmaku obj)
            {
                return obj.Id + obj.Pool.Capacity * 30;
            }
        }

    }

}