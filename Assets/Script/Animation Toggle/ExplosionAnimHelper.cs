using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationToggle
{
    [RequireComponent(typeof(Animator))]
    public class ExplosionAnimHelper : MonoBehaviour
    {
        public void PlayAnimation(string name)
        {
            GetComponent<Animator>().Play(name);
        }

        public void DestroyExplosion()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }


    }

}

