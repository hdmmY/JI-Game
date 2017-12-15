using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimController : MonoBehaviour {

	public void DestroyExplosion()
    {
        Destroy(this.gameObject);
    }
}
