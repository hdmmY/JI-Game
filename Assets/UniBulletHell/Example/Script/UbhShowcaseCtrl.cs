using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UbhShowcaseCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject _InitialPoolBulletPrefab;
    [SerializeField]
    int _InitialPoolCount = 1000;
    [SerializeField]
    GameObject[] _GoShotCtrlList;
    Rect _RectArea = new Rect(0, 0, 0, 0);
    int _NowIndex = 0;
    string _NowGoName;

    void Start ()
    {
        if (_InitialPoolBulletPrefab == null) {
            return;
        }
        // pooling bullet
        List<GameObject> goBulletList = new List<GameObject>();
        for (int i = 0; i < _InitialPoolCount; i++) {
            GameObject goBullet = UbhObjectPool.Instance.GetGameObject(_InitialPoolBulletPrefab, Vector3.zero, Quaternion.identity, true);
            if (goBullet == null) {
                break;
            }

            // add UbhBullet
            if (goBullet.GetComponent<UbhBullet>() == null) {
                goBullet.AddComponent<UbhBullet>();
            }

            goBulletList.Add(goBullet);
        }
        for (int i = 0; i < goBulletList.Count; i++) {
            UbhObjectPool.Instance.ReleaseGameObject(goBulletList[i]);
        }

        if (_GoShotCtrlList != null) {
            for (int i = 0; i < _GoShotCtrlList.Length; i++) {
                _GoShotCtrlList[i].SetActive(false);
            }
        }

        _NowIndex = -1;
        ChangeShot(true);
    }

    void Update ()
    {
        /*
        if (Input.GetKeyUp (KeyCode.LeftArrow)) {
            ChangeShot (false);
            return;
        }
        if (Input.GetKeyUp (KeyCode.RightArrow)) {
            ChangeShot (true);
            return;
        }
        */
    }

    void OnGUI ()
    {
        _RectArea.x = 0f;
        _RectArea.y = 0f;
        _RectArea.width = Screen.width;
        _RectArea.height = Screen.height;
        GUILayout.BeginArea(_RectArea);
        {
            float screenScaleX = (float) Screen.width / (float) UbhManager.BASE_SCREEN_WIDTH;
            float screenScaleY = (float) Screen.height / (float) UbhManager.BASE_SCREEN_HEIGHT;
            float screenScale = Screen.height < Screen.width ? screenScaleY : screenScaleX;

            GUIStyle guiStyle = GUIStyle.none;
            guiStyle.fontSize = (int) (22f * screenScale);
            guiStyle.normal.textColor = Color.white;
            guiStyle.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                // Shot Name
                GUILayout.Label("No." + (_NowIndex + 1).ToString() + " : " + _NowGoName, guiStyle, GUILayout.Width((float) Screen.width), GUILayout.Height((float) Screen.height * 0.15f));

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                // Prev Button
                if (GUILayout.Button("<< PREV", GUILayout.Width((float) Screen.width / 4f), GUILayout.Height((float) Screen.height * 0.1f))) {
                    ChangeShot(false);
                }

                GUILayout.FlexibleSpace();
                GUILayout.FlexibleSpace();

                // Next Button
                if (GUILayout.Button("NEXT >>", GUILayout.Width((float) Screen.width / 4f), GUILayout.Height((float) Screen.height * 0.1f))) {
                    ChangeShot(true);
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
        }
        GUILayout.EndArea();
    }

    void ChangeShot (bool toNext)
    {
        if (_GoShotCtrlList == null) {
            return;
        }

        StopAllCoroutines();

        if (0 <= _NowIndex && _NowIndex < _GoShotCtrlList.Length) {
            _GoShotCtrlList[_NowIndex].SetActive(false);
        }

        if (toNext) {
            _NowIndex = (int) Mathf.Repeat(_NowIndex + 1f, _GoShotCtrlList.Length);
        } else {
            _NowIndex--;
            if (_NowIndex < 0) {
                _NowIndex = _GoShotCtrlList.Length - 1;
            }
        }

        if (0 <= _NowIndex && _NowIndex < _GoShotCtrlList.Length) {
            _GoShotCtrlList[_NowIndex].SetActive(true);

            _NowGoName = _GoShotCtrlList[_NowIndex].name;

            StartCoroutine(StartShot());
        }
    }

    IEnumerator StartShot ()
    {
        float cntTimer = 0f;
        while (cntTimer < 1f) {
            cntTimer += UbhTimer.Instance.DeltaTime;
            yield return 0;
        }

        yield return 0;

        UbhShotCtrl shotCtrl = _GoShotCtrlList[_NowIndex].GetComponent<UbhShotCtrl>();
        if (shotCtrl != null) {
            shotCtrl.StartShotRoutine();
        }
    }
}