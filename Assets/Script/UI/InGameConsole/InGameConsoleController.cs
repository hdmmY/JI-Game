using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameConsoleController : MonoBehaviour
{
	public GameObject m_consoleViewContainer;

	public TMP_InputField m_inputField;

	public TMP_Text m_logOutput;

	private static readonly string InvalidCommand = "Invalid Command!\n";

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	private void OnEnable ()
	{
		m_consoleViewContainer.SetActive (false);

		m_inputField.onSubmit.AddListener (SubmitCommand);
	}

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	private void OnDisable ()
	{
		m_inputField.onSubmit.RemoveListener (SubmitCommand);
	}

	private float _prevTimeScale;

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	private void Update ()
	{
		if (Input.GetKeyDown (KeyCode.BackQuote))
		{
			if (m_consoleViewContainer.activeInHierarchy)
			{
				JITimer.Instance.TimeScale = _prevTimeScale;
				Debug.Log (JITimer.Instance.TimeScale);
				m_consoleViewContainer.SetActive (false);
			}
			else
			{
				_prevTimeScale = JITimer.Instance.TimeScale;
				JITimer.Instance.TimeScale = 0f;
				m_inputField.text = string.Empty;
				m_consoleViewContainer.SetActive (true);
				m_inputField.ActivateInputField ();
			}
		}

		if (m_consoleViewContainer.activeInHierarchy)
		{
			JITimer.Instance.TimeScale = 0f;
		}
	}

	private void SubmitCommand (string command)
	{
		if (string.IsNullOrEmpty (command))
		{
			return;
		}

		var curTime = System.DateTime.Now;
		m_logOutput.text += string.Format ("[{0,2}:{1,2}:{2,2}] : ", curTime.Hour, curTime.Minute, curTime.Second);

		m_inputField.text = string.Empty;

		var args = command.ToLower ().Split (new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		switch (args[0])
		{
			case "god":
				EnableGodMode (args);
				break;

			case "timescale":
			case "ts":
				ChangeTimeScale (args);
				break;

			case "cls":
				m_logOutput.text = string.Empty;
				break;

			default:
				m_logOutput.text += InvalidCommand;
				break;
		}

		m_inputField.ActivateInputField ();

		m_logOutput.rectTransform.offsetMin = new Vector2(0, 0); 
	}

	private void EnableGodMode (string[] args)
	{
		var player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();

		if (player.m_tgm)
		{
			m_logOutput.text += "God Mode Off\n";
			player.m_tgm = false;
		}
		else
		{
			m_logOutput.text += "God Mode On\n";
			player.m_tgm = true;
		}
	}

	private void ChangeTimeScale (string[] args)
	{
		if (args.Length == 1)
		{
			m_logOutput.text += InvalidCommand;
		}

		try
		{
			float timescale = float.Parse (args[1]);
			timescale = Mathf.Clamp (timescale, 0, 10);
			_prevTimeScale = timescale;
			m_logOutput.text += string.Format ("Current Time Scale : {0,3}\n", _prevTimeScale.ToString ());
		}
		catch (Exception e)
		{
			m_logOutput.text += InvalidCommand;
		}
	}
}