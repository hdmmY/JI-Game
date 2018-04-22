using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Falcon
{
    public class WindBulletMove
    {
        public static IEnumerator LinearWindMove (Transform bullet, Vector3 speed, Vector3 accel, WindArea[] winds)
        {
            if (bullet == null)
            {
                Debug.LogWarning ("The shooting bullet is not exist!");
                yield break;
            }

            while (true)
            {
                var curAccel = accel;

                if (winds != null && winds.Length > 0)
                {
                    foreach (var wind in winds)
                    {
                        curAccel += wind.AffectByWind (bullet.position) ? wind.WindForce : Vector3.zero;
                    }
                }

                speed += curAccel * JITimer.Instance.DeltTime;
                bullet.position += speed * JITimer.Instance.DeltTime;

                yield return null;
            }
        }

        /// <summary>
        /// Pause when the bullet speed is less zero
        /// </summary>
        /// <returns></returns>
        public static IEnumerator PauseLinearMove (Transform bullet, Vector3 speed, float slowTime, WindArea[] winds)
        {
            if (bullet == null)
            {
                Debug.LogWarning ("The shooting bullet is not exist!");
                yield break;
            }

            Vector3 initSpeed = speed;
            Vector3 deltSpeed = speed / slowTime;
            Vector3 accelSpeed = Vector3.zero;
            Vector3 accel = Vector3.zero;

            while (true)
            {
                slowTime -= JITimer.Instance.DeltTime;

                if (winds != null && winds.Length > 0)
                {
                    foreach (var wind in winds)
                    {
                        accel += wind.AffectByWind (bullet.position) ? wind.WindForce : Vector3.zero;
                    }
                }

                accelSpeed += accel * JITimer.Instance.DeltTime;
                initSpeed = slowTime < 0 ? Vector3.zero : slowTime * deltSpeed;
 
                bullet.position += (accelSpeed + initSpeed) * JITimer.Instance.DeltTime;

                yield return null;
            }
        }
    }
}