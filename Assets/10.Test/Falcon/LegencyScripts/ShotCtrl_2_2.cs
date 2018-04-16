using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        using MovementEffects.Extensions;
        using MovementEffects;
        using Sirenix.OdinInspector;

        public class ShotCtrl_2_2 : MonoBehaviour
        {
            [ReadOnly]
            public List<GameObject> BulletsToSpeedUp;

            public Vector2 WindDirction;

            [Range (3, 10)]
            public float WindForce;

            public Transform BossTrans;

            private void OnEnable ()
            {
                var bullets = GameObject.FindGameObjectsWithTag ("EnemyBullet")
                    .Where (x => x.GetComponent<JIBulletProperty> () != null);

                BulletsToSpeedUp = new List<GameObject> (bullets);

                Shot ();
            }

            private void Shot ()
            {
                if (BulletsToSpeedUp == null || BulletsToSpeedUp.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < BulletsToSpeedUp.Count; i++)
                {
                    if (BulletsToSpeedUp[i] == null || !BulletsToSpeedUp[i].activeInHierarchy)
                    {
                        continue;
                    }

                    var controller = BulletsToSpeedUp[i].GetComponent<JIBulletController> () ??
                        BulletsToSpeedUp[i].AddComponent<JIBulletController> ();
                    float forceAngle = UbhUtil.GetAngleFromTwoPosition (Vector3.zero, WindDirction);
                    float distance = 1 + (controller.transform.position - BossTrans.position).magnitude;
                    float force = WindForce / distance;
                    controller.Shot (0, forceAngle, 0, force,
                        false, null, 0, 0,
                        false, 0, 0,
                        false, 0, 0);
                }
            }
        }
    }
}