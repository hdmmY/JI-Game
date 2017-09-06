using UnityEngine;
using System.Collections;

public class UbhDestroyArea : UbhMonoBehaviour
{
    [SerializeField]
    bool _UseCenterCollider;
    [SerializeField]
    BoxCollider2D _ColCenter;
    [SerializeField]
    BoxCollider2D _ColTop;
    [SerializeField]
    BoxCollider2D _ColBottom;
    [SerializeField]
    BoxCollider2D _ColRight;
    [SerializeField]
    BoxCollider2D _ColLeft;

    void Start ()
    {
        if (_ColCenter == null || _ColTop == null || _ColBottom == null || _ColRight == null || _ColLeft == null) {
            return;
        }

        UbhManager manager = FindObjectOfType<UbhManager>();
        if (manager != null && manager._ScaleToFit) {
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1f, 1f));
            Vector2 size = max * 2f;
            size.x += 0.5f;
            size.y += 0.5f;
            Vector2 center = Vector2.zero;

            _ColCenter.size = size;

            _ColTop.size = size;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            center.x = _ColTop.center.x;
            center.y = size.y;
            _ColTop.center = center;
#else
            center.x = _ColTop.offset.x;
            center.y = size.y;
            _ColTop.offset = center;
#endif

            _ColBottom.size = size;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            center.x = _ColBottom.center.x;
            center.y = -size.y;
            _ColBottom.center = center;
#else
            center.x = _ColBottom.offset.x;
            center.y = -size.y;
            _ColBottom.offset = center;
#endif

            Vector2 horizontalSize = Vector2.zero;
            horizontalSize.x = size.y;
            horizontalSize.y = size.x;

            _ColRight.size = horizontalSize;
            center.x = (size.x / 2f) + (horizontalSize.x / 2f);
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            center.y = _ColRight.center.y;
            _ColRight.center = center;
#else
            center.y = _ColRight.offset.y;
            _ColRight.offset = center;
#endif

            _ColLeft.size = horizontalSize;
            center.x = -(size.x / 2f) - (horizontalSize.x / 2f);
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            center.y = _ColLeft.center.y;
            _ColLeft.center = center;
#else
            center.y = _ColLeft.offset.y;
            _ColLeft.offset = center;
#endif
        }

        _ColCenter.enabled = _UseCenterCollider;
        _ColTop.enabled = !_UseCenterCollider;
        _ColBottom.enabled = !_UseCenterCollider;
        _ColRight.enabled = !_UseCenterCollider;
        _ColLeft.enabled = !_UseCenterCollider;
    }

    void OnTriggerEnter2D (Collider2D c)
    {
        if (_UseCenterCollider) {
            return;
        }
        HitCheck(c.transform);
    }

    void OnTriggerExit2D (Collider2D c)
    {
        if (_UseCenterCollider == false) {
            return;
        }
        HitCheck(c.transform);
    }

    void OnTriggerEnter (Collider c)
    {
        if (_UseCenterCollider) {
            return;
        }
        HitCheck(c.transform);
    }

    void OnTriggerExit (Collider c)
    {
        if (_UseCenterCollider == false) {
            return;
        }
        HitCheck(c.transform);
    }

    void HitCheck (Transform colTrans)
    {
        // *It is compared with name in order to separate as Asset from project settings.
        //  However, it is recommended to use Layer or Tag.
        string goName = colTrans.name;
        if (goName.Contains(UbhPlayer.NAME_ENEMY_BULLET) ||
            goName.Contains(UbhEnemy.NAME_PLAYER_BULLET)) {
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.parent.gameObject);

        } else if (goName.Contains(UbhEnemy.NAME_PLAYER) == false) {
            Destroy(colTrans.gameObject);
        }
    }
}