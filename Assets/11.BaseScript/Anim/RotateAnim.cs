using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationToggle
{
    public class RotateAnim : MonoBehaviour
    {
        public Transform m_target;

        public Vector3 m_eularVelocity;

        public float m_rotateSpeed;

        private void Update()
        {
            Vector3 rotateAngle = m_eularVelocity * JITimer.Instance.RealDeltTime;

            if(m_target == null)
            {
                transform.Rotate(rotateAngle);
            }
            else
            {
                m_target.Rotate(rotateAngle);
            }
        }

    }
}


