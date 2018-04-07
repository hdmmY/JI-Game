using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Sequenced awake gameobject. 
/// When awake a new gameobject, it will destroy last awaked gameobject
/// </summary>
public class SeqAwakeMananger : MonoBehaviour
{
    /// <summary>
    /// A list of sequence gameobjects.
    /// Use for editor
    /// </summary>
    [ListDrawerSettings (DraggableItems = true, Expanded = true, ShowIndexLabels = true, NumberOfItemsPerPage = 20)]
    public List<TimeManager.TimeGameobject> SeqGameobjects;

    [ReadOnly, ShowInInspector]
    public float Timer { get { return _timer; } }

    private Queue<TimeManager.TimeGameobject> _seqGOs;

    private float _timer;

    private GameObject _lastActivedGo;

    private void Awake ()
    {
        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        _timer = -0.5f;

        _seqGOs = new Queue<TimeManager.TimeGameobject> (SeqGameobjects);

        if (_seqGOs.Count > 0)
        {
            foreach (var go in _seqGOs)
            {
                go.Active = false;
            }
        }
    }

    private void Update ()
    {
        if (_seqGOs == null || _seqGOs.Count == 0) return;

        _timer += JITimer.Instance.DeltTime;

        if (_timer >= _seqGOs.Peek ().ActiveTime)
        {
            if (_lastActivedGo) Destroy (_lastActivedGo);
            _lastActivedGo = _seqGOs.Dequeue ().Go;
            _lastActivedGo.SetActive (true);
            _timer = 0f;
        }
    }
}