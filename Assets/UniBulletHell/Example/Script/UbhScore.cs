using UnityEngine;
using System.Collections;

public class UbhScore : UbhMonoBehaviour
{
    const string HIGH_SCORE_KEY = "highScoreKey";
    const string HIGH_SCORE_TITLE = "HighScore : ";
    [SerializeField]
    bool _DeleteScore;
    [SerializeField]
    GUIText _ScoreGUIText;
    [SerializeField]
    GUIText _HighScoreGUIText;
    int _Score;
    int _HighScore;

    void Start ()
    {
        Initialize();
    }

    void Update ()
    {
        if (_HighScore < _Score) {
            _HighScore = _Score;
        }

        _ScoreGUIText.text = _Score.ToString();
        _HighScoreGUIText.text = HIGH_SCORE_TITLE + _HighScore.ToString();
    }

    public void Initialize ()
    {
        if (_DeleteScore) {
            PlayerPrefs.DeleteAll();
        }
        _Score = 0;
        _HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    public void AddPoint (int point)
    {
        _Score = _Score + point;
    }

    public void Save ()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, _HighScore);
        PlayerPrefs.Save();

        Initialize();
    }
}
