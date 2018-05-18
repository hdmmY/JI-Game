using TMPro;
using UnityEngine;

[RequireComponent (typeof (TMP_Text))]
public class ShowTime : MonoBehaviour
{
    private TimeManager _timeManager;

    private TMP_Text _textField;

    private static readonly string NullText = "Null";

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake ()
    {
        _textField = GetComponent<TMP_Text> ();
        _timeManager = FindObjectOfType<TimeManager> ();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update ()
    {
        if (_timeManager)
        {
            _textField.text = NumberUtil.
            NumberFrom0to600[Mathf.Clamp ((int) _timeManager.m_timer, 0, 300)];
        }
        else
        {
            _textField.text = NullText;
        }
    }
}