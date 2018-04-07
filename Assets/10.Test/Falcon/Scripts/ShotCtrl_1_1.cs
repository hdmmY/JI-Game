using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    namespace Falcon
    {
        /// <summary>
        /// Boss will use the same bullet shot pattern like player
        /// </summary>
        public class ShotCtrl_1_1 : MonoBehaviour
        {
            /// <summary>
            /// Boss shot bullet prefab
            /// </summary>
            public GameObject BulletPrefab;

            /// <summary>
            /// Local shot point one
            /// </summary>
            public Vector3 ShotPoint1;

            /// <summary>
            /// Another local shot point
            /// </summary>
            public Vector3 ShotPoint2;

            private PlayerProperty _player;

            private float _shotInterval;

            private float _bulletSpeed;

            private float _timer;

            private void OnEnable ()
            {
                _player = GameObject.FindGameObjectWithTag ("Player")?.GetComponent<PlayerProperty> ();
                _timer = 0f;
            }

            private void Update ()
            {
                if (_player == null || BulletPrefab == null)
                {
                    Debug.LogError ("Cannot shot because the parameters is not set!", this);
                    return;
                }

                _shotInterval = _player.m_shootInterval;
                _bulletSpeed = _player.m_bulletSpeed;
                _timer += JITimer.Instance.DeltTime;

                if (_timer < _shotInterval)
                {
                    return;
                }
                else
                {
                    Shot (transform.TransformPoint (ShotPoint1));
                    Shot (transform.TransformPoint (ShotPoint2));
                    _timer = 0f;
                }
            }

            private void Shot (Vector3 shotPosition)
            {
                var bullet = BulletPool.Instance.GetGameObject (BulletPrefab, shotPosition, Quaternion.identity, false);
                var controller = bullet.GetComponent<JIBulletController> () ?? bullet.AddComponent<JIBulletController> ();
                controller.Shot (_bulletSpeed, -90, 0, 0,
                    false, null, 0, 0,
                    false, 0, 0,
                    false, 0, 0);
            }

            private void OnDrawGizmos ()
            {
                Gizmos.color = new Color (1, 0, 0, 0.5f);
                Gizmos.DrawSphere (transform.TransformPoint (ShotPoint1), 0.07f);
                Gizmos.DrawSphere (transform.TransformPoint (ShotPoint2), 0.07f);
            }
        }
    }
}