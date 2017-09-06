using UnityEngine;
using System.Collections;

public class UbhSetting : UbhMonoBehaviour
{
    [Range(0, 2)]
    public int _VsyncCount = 1;
    [Range(0, 60)]
    public int _FrameRate = 60;

    void Start ()
    {
        SetValue();
    }

    void OnValidate ()
    {
        SetValue();
    }

    void Update ()
    {
        if (UbhUtil.IsMobilePlatform() && Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    void SetValue ()
    {
        _VsyncCount = Mathf.Clamp(_VsyncCount, 0, 2);
        QualitySettings.vSyncCount = _VsyncCount;

        _FrameRate = Mathf.Clamp(_FrameRate, 1, 120);
        Application.targetFrameRate = _FrameRate;
    }
}