using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public delegate bool ImmFunc ();

    /// <summary>
    /// Behavior tree datas
    /// </summary>
    public class BTDate : ScriptableObject
    {
        public readonly List<ShapeToken> ShapeTokens;

        public readonly List<ImmFunc> ImmediateFuncs;

        public BTDate (ShapeToken[] tokens, ImmFunc[] immFuncs)
        {
            ShapeTokens = new List<ShapeToken> (tokens);
            ImmediateFuncs = new List<ImmFunc> (immFuncs);
        }
    }

    [Serializable]
    public struct ShapeToken
    {

    }
}