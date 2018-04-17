using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        public class MoveCtrl_1_1 : MonoBehaviour
        {
            /// <summary>
            /// Boss's target position y value
            /// </summary>
            public float TargetY;

            /// <summary>
            /// Limit boss's horizontal movement at [-MaxX, MaxX]
            /// </summary>
            public float MaxX = 3;

            public Transform BossTrans;

            private PlayerProperty _player;

            private void OnEnable ()
            {
                _player = GameObject.FindGameObjectWithTag ("Player")?.GetComponent<PlayerProperty> ();
            }

            private void LateUpdate ()
            {
                if (_player == null || BossTrans == null)
                {
                    return;
                }

                float xSpeed = _player.m_horizontalSpeed;
                float ySpeed = _player.m_verticalSpeed;

                Vector3 addPos = Vector3.zero;

                addPos.y = BossTrans.position.y - TargetY;
                if (Mathf.Abs (addPos.y) > 0.01f)
                {
                    addPos.y = addPos.y < 0 ?
                        Mathf.Clamp (ySpeed * JITimer.Instance.DeltTime, 0, -addPos.y) :
                        Mathf.Clamp (-ySpeed * JITimer.Instance.DeltTime, -addPos.y, 0);
                }

                addPos.x = BossTrans.position.x - _player.transform.position.x;
                if (Mathf.Abs (addPos.x) > 0.01f)
                {
                    addPos.x = addPos.x < 0 ?
                        Mathf.Clamp (xSpeed * JITimer.Instance.DeltTime, 0, -addPos.x) :
                        Mathf.Clamp (-xSpeed * JITimer.Instance.DeltTime, -addPos.x, 0);
                }

                Vector3 finalPos = addPos + BossTrans.position;
                finalPos.x = Mathf.Clamp (finalPos.x, -MaxX, MaxX);

                BossTrans.position = finalPos;
            }

            private void OnDrawGizmosSelected ()
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine (
                    new Vector3 (-MaxX, TargetY, 0),
                    new Vector3 (MaxX, TargetY, 0));
            }
        }
    }
}