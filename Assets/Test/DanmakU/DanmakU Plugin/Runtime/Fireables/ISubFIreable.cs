using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU
{

    public interface ISubFireable : IFireable
    {
        void Subfire (DanmakuConfig state);

        ISubFireable GetChild ();

        void SetChild (ISubFireable child);
    }

}