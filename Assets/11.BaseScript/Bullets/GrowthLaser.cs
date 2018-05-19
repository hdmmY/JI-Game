using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Sirenix.OdinInspector;

public class GrowthLaser : Laser
{
    public override LaserType LaserType => LaserType.Growth;

    public float GrowthSpeed;

    [ReadOnly]
    public List<Transform> Colliders = new List<Transform> ();

    protected override void Update ()
    {
        base.Update ();

        if (Colliders.Count == 0)
        {
            LaserLength += JITimer.Instance.DeltTime * GrowthSpeed;
        }
        else
        {
            Colliders = new List<Transform> (Colliders.Where (x => x != null));

            foreach (var col in Colliders)
            {
                float length = col.position.y - transform.position.y;
                LaserLength = Mathf.Min (length, LaserLength);
            }

            if (LaserLength <= 0)
            {
                Colliders.Clear ();
                LaserLength = 0f;
            }
        }

        UpdateLaserAppear ();
    }

    private void OnDisable ()
    {
        Colliders.Clear ();
    }
}