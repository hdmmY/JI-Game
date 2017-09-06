using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Ubh paint shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Paint Shot")]
public class UbhPaintShot : UbhBaseShot
{
    static readonly string[] SPLIT_VAL = { "\n", "\r", "\r\n" };

    // "Set a paint data text file. (ex.[UniBulletHell] > [Example] > [PaintShotData] in Project view)"
    // "BulletNum is ignored."
    public TextAsset _PaintDataText;
    // "Set a center angle of shot. (0 to 360) (center of first line)"
    [Range(0f, 360f)]
    public float _PaintCenterAngle = 180f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f)]
    public float _BetweenAngle = 3f;
    // "Set a delay time between shot and next line shot. (sec)"
    public float _NextLineDelay = 0.1f;

    protected override void Awake ()
    {
        base.Awake();
    }

    public override void Shot ()
    {
        StartCoroutine(ShotCoroutine());
    }

    IEnumerator ShotCoroutine ()
    {
        if (_BulletSpeed <= 0f || _PaintDataText == null) {
            Debug.LogWarning("Cannot shot because BulletSpeed or PaintDataText is not set.");
            yield break;
        }
        if (_Shooting) {
            yield break;
        }
        _Shooting = true;

        var paintData = LoadPaintData();

        float paintStartAngle = _PaintCenterAngle;
        if (0 < paintData.Count) {
            paintStartAngle -= paintData[0].Count % 2 == 0 ?
                (_BetweenAngle * paintData[0].Count / 2f) + (_BetweenAngle / 2f) :
                 _BetweenAngle * Mathf.Floor(paintData[0].Count / 2f);
        }

        for (int lineCnt = 0; lineCnt < paintData.Count; lineCnt++) {
            var line = paintData[lineCnt];
            if (0 < lineCnt && 0 < _NextLineDelay) {
                yield return StartCoroutine(UbhUtil.WaitForSeconds(_NextLineDelay));
            }
            for (int i = 0; i < line.Count; i++) {
                if (line[i] == 1) {
                    var bullet = GetBullet(transform.position, transform.rotation);
                    if (bullet == null) {
                        break;
                    }

                    float angle = paintStartAngle + (_BetweenAngle * i);

                    ShotBullet(bullet, _BulletSpeed, angle);

                    AutoReleaseBulletGameObject(bullet.gameObject);
                }
            }
        }

        FinishedShot();
    }

    List<List<int>> LoadPaintData ()
    {
        var paintData = new List<List<int>>();

        if (string.IsNullOrEmpty(_PaintDataText.text)) {
            Debug.LogWarning("Cannot load paint data because PaintDataText file is empty.");
            return paintData;
        }

        string[] lines = _PaintDataText.text.Split(SPLIT_VAL, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++) {
            // lines beginning with "#" are ignored as comments.
            if (lines[i].StartsWith("#")) {
                continue;
            }
            // add line
            paintData.Add(new List<int>());

            for (int j = 0; j < lines[i].Length; j++) {
                // bullet is fired into position of "*".
                paintData[paintData.Count - 1].Add(lines[i][j] == '*' ? 1 : 0);
            }
        }

        // reverse because fire from bottom left.
        paintData.Reverse();

        return paintData;
    }
}