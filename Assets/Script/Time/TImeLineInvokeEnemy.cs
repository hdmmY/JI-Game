using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TimeLineManager))]
public class TImeLineInvokeEnemy : MonoBehaviour
{
    private List<TimeLineManager.TimeLineGameObject> _timelineGO;


    [SerializeField]
    private float _timer;

    private void OnEnable()
    {
        InitReference();

        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        LinkedList<TimeLineManager.TimeLineGameObject> _removeTimeLineObjects = new LinkedList<TimeLineManager.TimeLineGameObject>();


        // invoke game object
        foreach (var tlGo in _timelineGO)
        {
            if (_timer >= tlGo.m_activeTime)
            {
                var timeLineControllerScript = tlGo.m_gameObject.AddComponent<TimeLineController>();

                timeLineControllerScript.m_lastTime = tlGo.m_disactiveTime - tlGo.m_activeTime;
                timeLineControllerScript.m_targetGameObject = true;

                tlGo.m_gameObject.SetActive(true);

                _removeTimeLineObjects.AddLast(tlGo);
            }
        }


        // remove invoked game object
        if(_removeTimeLineObjects.Count != 0)
        {
            foreach(var removedTimeLineGo in _removeTimeLineObjects)
            {
                _timelineGO.Remove(removedTimeLineGo);
            }
        }

    }





    private void InitReference()
    {
        _timelineGO = new List<TimeLineManager.TimeLineGameObject>();
        foreach (var go in GetComponent<TimeLineManager>().m_timeLineGameObject)
        {
            _timelineGO.Add(go);
        }
    }
}
