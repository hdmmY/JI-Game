using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Boss.Falcon
{
    public class WindArea : MonoBehaviour
    {
        [SerializeField] private Vector3 _windForce;

        public Vector3 WindForce
        {
            get
            {
                return _windForce;
            }
        }

        public Vector3 AffectAreaCenter;

        public Vector3 AffectAreaSize;

        public bool WindActive;

        /// <summary>
        /// Whether the item is affected by the wind area
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool AffectByWind (Vector3 itemPos)
        {
            if (!WindActive) return false;

            Vector3 distance = itemPos - AffectAreaCenter;
            distance.x = Mathf.Abs (distance.x);
            distance.y = Mathf.Abs (distance.y);

            return (distance.x <= AffectAreaSize.x) && (distance.y <= AffectAreaSize.y);
        }

        private void OnDrawGizmosSelected ()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireCube (AffectAreaCenter, AffectAreaSize * 2);
        }
    }
}