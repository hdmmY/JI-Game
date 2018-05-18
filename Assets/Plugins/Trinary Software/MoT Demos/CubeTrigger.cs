using UnityEngine;

// /////////////////////////////////////////////////////////////////////////////////////////
//                          HELPER SCRIPT FOR MOVEMENT DEMO #2
// Created by Teal Rogers
// Trinary Software
// All rights preserved
// For questions or support contact trinaryllc@gmail.com
// /////////////////////////////////////////////////////////////////////////////////////////

namespace MovementDemos
{
    public class CubeTrigger : MonoBehaviour
    {
        public Demo2Controller Controller;
        public bool FloorTrigger = false;

        public void OnTriggerEnter(Collider otherCollider)
        {
            if (Controller != null)
            {
                if (FloorTrigger)
                    Controller.FloorHit(otherCollider);
                else
                    Controller.CubeHit(transform.parent.gameObject, otherCollider);
            }
        }
    }
}