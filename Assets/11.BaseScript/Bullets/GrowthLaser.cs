using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GrowthLaser : Laser
{
    public override LaserType LaserType => LaserType.Growth;

    [Range (0, 1)]
    public float GrowthSpeed;

    [HideInInspector]
    public List<Transform> Colliders = new List<Transform> ();

    protected override void Update ()
    {
        base.Update ();

        if (Colliders.Count == 0)
        {
            LaserLength += JITimer.Instance.DeltTime * GrowthSpeed * 5f;
        }
        else
        {
            Colliders = new List<Transform> (Colliders.Where (x => x != null));

            foreach (var col in Colliders)
            {
                float length = col.position.y - transform.position.y;
                LaserLength = Mathf.Min (length, LaserLength);
            }
        }

        UpdateLaserAppear ();
    }
}