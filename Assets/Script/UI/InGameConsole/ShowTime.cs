using TMPro;
using UnityEngine;

[RequireComponent (typeof (TMP_Text))]
public class ShowTime : MonoBehaviour
{
	private TimeManager _timeManager;

	private TMP_Text _textField;

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
		_textField.text = NumberUtil.
			NumberFrom1to300[Mathf.Clamp ((int) _timeManager.m_timer, 0, 300)];
	}
}