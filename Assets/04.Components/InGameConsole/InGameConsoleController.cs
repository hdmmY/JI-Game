using System;
using System.Linq;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                m_consoleViewContainer.SetActive (false);
                FindObjectOfType<PlayerMove> ().enabled = true;
            }
            else
            {
                _prevTimeScale = JITimer.Instance.TimeScale;
                JITimer.Instance.TimeScale = 0f;
                m_consoleViewContainer.SetActive (true);
                m_inputField.text = "";
                m_inputField.ActivateInputField ();
                FindObjectOfType<PlayerMove> ().enabled = false;
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
        m_logOutput.text += string.Format ("\n[{0,2}:{1,2}:{2,2}] : {3}\n",
            curTime.Hour, curTime.Minute, curTime.Second, command);

        m_inputField.text = string.Empty;

        var args = command.ToLower ().Split (new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        switch (args[0])
        {
            case "tgm":
                TrunGodMode (args);
                break;

            case "timescale":
            case "ts":
                ChangeTimeScale (args);
                break;

            case "cls":
                m_logOutput.text = string.Empty;
                break;

            case "fps":
                ShowFPSAndBulletNum ();
                break;

            case "time":
                ShowTime ();
                break;

            case "status":
                ShowPlayerStatus ();
                break;

            case "loadscene":
                LoadSceneByName (args);
                break;

            case "recover":
                Recover ();
                break;

            default:
                m_logOutput.text += InvalidCommand;
                break;
        }

        m_inputField.ActivateInputField ();

        m_logOutput.rectTransform.offsetMin = new Vector2 (0, 0);
    }

    private void TrunGodMode (string[] args)
    {
        var player = FindObjectOfType<PlayerProperty> ();

        if (player.m_god)
        {
            m_logOutput.text += "\tGod Mode Off\n";
            player.m_god = false;
        }
        else
        {
            m_logOutput.text += "\tGod Mode On\n";
            player.m_god = true;
        }
    }

    private void ChangeTimeScale (string[] args)
    {
        if (args.Length == 1)
        {
            m_logOutput.text += InvalidCommand;
            return;
        }

        try
        {
            float timescale = float.Parse (args[1]);
            timescale = Mathf.Clamp (timescale, 0, 10);
            _prevTimeScale = timescale;
            m_logOutput.text += string.Format ("\tCurrent Time Scale : {0,3}\n", _prevTimeScale.ToString ());
        }
        catch (Exception e)
        {
            m_logOutput.text += InvalidCommand;
        }
    }

    private void ShowFPSAndBulletNum ()
    {
        var fpss = gameObject.Children ().Where (x => x.name == "FPS Canvas");

        var bullets = gameObject.Children ().Where (x => x.name == "BulletNum Canvas");

        foreach (var fps in fpss)
        {
            fps.SetActive (!fps.activeInHierarchy);
        }
        foreach (var bullet in bullets)
        {
            bullet.SetActive (!bullet.activeInHierarchy);
        }
    }

    private void ShowTime ()
    {
        var times = gameObject.Children ().Where (x => x.name == "Time Canvas");

        foreach (var time in times)
        {
            time.SetActive (!time.activeInHierarchy);
        }
    }

    private void ShowPlayerStatus ()
    {
        var status = gameObject.Children ().Where (x => x.name == "PlayerStatus Canvas");

        foreach (var statu in status)
        {
            statu.SetActive (!statu.activeInHierarchy);
        }
    }

    private void LoadSceneByName (string[] args)
    {
        if (args.Length == 1)
        {
            m_logOutput.text += InvalidCommand;
            return;
        }

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string sceneName = SceneUtility.GetScenePathByBuildIndex (i);
            int start = sceneName.LastIndexOf ('/') + 1;
            int end = sceneName.LastIndexOf ('.');
            if (sceneName.Substring (start, end - start).ToLower () == args[1])
            {
                EventManager.Instance.Raise (new BeforeChangeToNextStageEvent (
                    SceneManager.GetActiveScene ().buildIndex, i));

                SceneManager.LoadSceneAsync (i, LoadSceneMode.Single);
                return;
            }
        }

        m_logOutput.text += InvalidCommand;
    }

    private void Recover ()
    {
        var player = GameObject.FindGameObjectWithTag ("Player")
            .GetComponent<PlayerProperty> ();

        if (player == null) return;

        player.m_playerLife = 5;
        player.m_playerHealth = 5;

        m_logOutput.text += "Recovered!`\n";
    }
}