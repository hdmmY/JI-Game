using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent (typeof (TMP_Text))]
public class ShowBulletNum : MonoBehaviour
{
    private TMP_Text _textField;

    private BulletPool _bulletPool;

    private DanmakU.DanmakuManager _danmakuManager;

    private void Start ()
    {
        _textField = GetComponent<TMP_Text> ();

        _danmakuManager = DanmakU.DanmakuManager.Instance;

        _bulletPool = BulletPool.Instance;
    }

    private void LateUpdate ()
    {
        if (_danmakuManager == null)
        {
            _textField.text = _bulletPool.ActiveGameObject.ToString ();
        }
        else
        {
            _textField.text = _danmakuManager.ActiveCount.ToString ();
        }
    }

}