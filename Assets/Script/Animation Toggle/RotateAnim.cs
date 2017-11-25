using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationToggle
{
    public class RotateAnim : MonoBehaviour
    {
        public Transform m_target;

        public bool m_clockwise;

        public float m_rotateSpeed;

        private void Update()
        {
            float rotateAngle = m_rotateSpeed * UbhTimer.Instance.DeltaTime;
            rotateAngle = m_clockwise ? rotateAngle : -rotateAngle;

            if(m_target == null)
            {
                transform.Rotate(new Vector3(0, 0, rotateAngle));
            }
            else
            {
                m_target.Rotate(new Vector3(0, 0, rotateAngle));
            }
        }

    }
}


