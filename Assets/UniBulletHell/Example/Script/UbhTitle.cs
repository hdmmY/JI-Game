using UnityEngine;
using System.Collections;

public class UbhTitle : UbhMonoBehaviour
{
    const string TITLE_PC = "Press X";
    const string TITLE_MOBILE = "Tap To Start";
    [SerializeField]
    GUIText _StartGUIText;

    void Start ()
    {
        _StartGUIText.text = UbhUtil.IsMobilePlatform() ? TITLE_MOBILE : TITLE_PC;
    }
}