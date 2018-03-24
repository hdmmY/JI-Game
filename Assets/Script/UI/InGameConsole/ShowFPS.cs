using TMPro;
using UnityEngine;

[RequireComponent (typeof (TMP_Text))]
public class ShowFPS : MonoBehaviour
{
	private TMP_Text _textField;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	private void Start ()
	{
		_textField = GetComponent<TMP_Text> ();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	private void Update ()
	{
		_textField.text = NumberUtil.
			NumberFrom1to300[Mathf.Clamp (FPSCounter.Instance.LowestFPS, 0, 300)];
	}
}