using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineController : MonoBehaviour
{
    /// <summary>
    /// whether the distroy target is a game object
    /// </summary>
    public bool m_targetGameObject; 

    public float m_lastTime;

    [SerializeField]
    private float _timer;

	private void OnEnable()
    {
        _timer = 0f;
    }


    private void Update()
    {
        _timer += Time.deltaTime;


        if(_timer >= m_lastTime)
        {
            if (m_targetGameObject)
                DestroyImmediate(gameObject);
            else
                DestroyImmediate(this);
            return;
        }

    }     
}
