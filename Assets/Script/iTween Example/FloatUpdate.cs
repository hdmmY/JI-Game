using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace iTweenExample
{
    public class FloatUpdate : MonoBehaviour
    {

        private float _value;

        private void OnEnable()
        {
            _value = iTween.FloatUpdate(0, 50, 1);
        }

        private void Update()
        {
            _value = iTween.FloatUpdate(_value, 50, 1);

            //Debug.Log(_value);
        }
    }
}



