using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimationToggle
{
    public class ExplosionAnimController : MonoBehaviour
    {

        public void DestroyExplosion()
        {
            Destroy(this.gameObject);
        }
    }
}

