using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimationToggle
{
    public class AimingTarget : MonoBehaviour
    {
        public Transform m_target;

        public bool m_aiming;

        private void Update()
        {
            if (m_aiming)
            {
                float angle = UbhUtil.GetAngleFromTwoPosition(transform, m_target);
                transform.SetEulerAnglesZ(angle - 90f);
            }

        }
    }
}

