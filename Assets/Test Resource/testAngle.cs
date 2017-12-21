using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAngle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        for(int angle = 0; angle < 360; angle += 5)
        {
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            Debug.LogFormat("angle = {0}, vec = {1}, result = {2}", angle, dir.normalized, UbhUtil.GetAngleFromTwoPosition(Vector2.zero, dir));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
