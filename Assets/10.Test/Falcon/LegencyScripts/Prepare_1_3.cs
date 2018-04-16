using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        public class Prepare_1_3 : MonoBehaviour
        {
            /// <summary>
            /// True boss reference
            /// </summary>
            public GameObject BossRefer;

            /// <summary>
            /// Fake boss prefab
            /// </summary>
            public GameObject FakeBoss;

            /// <summary>
            /// True 1_3 controller
            /// </summary>
            public GameObject True13;

            /// <summary>
            /// Fake 1_3 controller 
            /// </summary>
            public GameObject False13;

            private void OnEnable ()
            {
                var fakeBoss =
                    Instantiate (FakeBoss, BossRefer.transform.position, BossRefer.transform.rotation);
                fakeBoss.GetComponent<EnemyProperty> ().m_health = 1000000;

                False13.GetComponent<MoveCtrl_1_3_False> ().BossTrans = fakeBoss.transform;
                False13.transform.SetParent (fakeBoss.transform);
                False13.transform.localPosition = Vector3.zero;
                False13.GetComponent<UbhBaseShot> ().OnShotFinish =
                    (shot) => Destroy (fakeBoss);

                False13.SetActive (true);
                True13.SetActive (true);
            }
        }
    }
}