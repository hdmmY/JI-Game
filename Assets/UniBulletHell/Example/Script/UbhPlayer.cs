using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UbhSpaceship))]
public class UbhPlayer : UbhMonoBehaviour
{
    public const string NAME_ENEMY_BULLET = "EnemyBullet";
    public const string NAME_ENEMY = "Enemy";
    const string AXIS_HORIZONTAL = "Horizontal";
    const string AXIS_VERTICAL = "Vertical";
    readonly Vector2 VIEW_PORT_LEFT_BOTTOM = new Vector2(0, 0);
    readonly Vector2 VIEW_PORT_RIGHT_TOP = new Vector2(1, 1);
    [SerializeField]
    GameObject _BulletPrefab;
    [SerializeField]
    float _ShotDelay;
    [SerializeField]
    UbhUtil.AXIS _UseAxis = UbhUtil.AXIS.X_AND_Y;
    UbhSpaceship _Spaceship;
    UbhManager _Manager;
    Transform _BackgroundTransform;
    bool _IsTouch;
    float _LastXpos;
    float _LastYpos;
    Vector2 _TempVector2 = Vector2.zero;
    AudioSource _AudioShot;

    IEnumerator Start ()
    {
        _Spaceship = GetComponent<UbhSpaceship>();
        _Manager = FindObjectOfType<UbhManager>();
        _BackgroundTransform = FindObjectOfType<UbhBackground>().transform;
        _AudioShot = GetComponent<AudioSource>();

        while (true) {
            Shot();

            yield return new WaitForSeconds(_ShotDelay);
        }
    }

    void Update ()
    {
        if (UbhUtil.IsMobilePlatform()) {
            TouchMove();
#if UNITY_EDITOR
            KeyMove();
#endif
        } else {
            KeyMove();
        }
    }

    void KeyMove ()
    {
        _TempVector2.x = Input.GetAxisRaw(AXIS_HORIZONTAL);
        _TempVector2.y = Input.GetAxisRaw(AXIS_VERTICAL);
        Move(_TempVector2.normalized);
    }

    void TouchMove ()
    {
        float xPos = 0f;
        float yPos = 0f;
        if (Input.GetMouseButtonDown(0)) {
            _IsTouch = true;
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            xPos = vec.x;
            yPos = vec.y;
        } else if (Input.GetMouseButton(0)) {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            xPos = vec.x;
            yPos = vec.y;
            if (_IsTouch) {
                _TempVector2.x = (xPos - _LastXpos) * 10f;
                _TempVector2.y = (yPos - _LastYpos) * 10f;
                Move(_TempVector2.normalized);
            }
        } else if (Input.GetMouseButtonUp(0)) {
            _IsTouch = false;
        }
        _LastXpos = xPos;
        _LastYpos = yPos;
    }

    void Move (Vector2 direction)
    {
        Vector2 min;
        Vector2 max;
        if (_Manager != null && _Manager._ScaleToFit) {
            min = Camera.main.ViewportToWorldPoint(VIEW_PORT_LEFT_BOTTOM);
            max = Camera.main.ViewportToWorldPoint(VIEW_PORT_RIGHT_TOP);
        } else {
            Vector2 scale = _BackgroundTransform.localScale;
            min = scale * -0.5f;
            max = scale * 0.5f;
        }

        Vector2 pos = transform.position;
        if (_UseAxis == UbhUtil.AXIS.X_AND_Z) {
            pos.y = transform.position.z;
        }

        pos += direction * _Spaceship._Speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        if (_UseAxis == UbhUtil.AXIS.X_AND_Z) {
            transform.SetPosition(pos.x, transform.position.y, pos.y);
        } else {
            transform.position = pos;
        }
    }

    void Shot ()
    {
        if (_BulletPrefab != null) {
            UbhObjectPool.Instance.GetGameObject(_BulletPrefab, transform.position, transform.rotation);

            if (_AudioShot != null) {
                _AudioShot.Play();
            }
        }
    }

    void OnTriggerEnter2D (Collider2D c)
    {
        HitCheck(c.transform);
    }

    void OnTriggerEnter (Collider c)
    {
        HitCheck(c.transform);
    }

    void HitCheck (Transform colTrans)
    {
        // *It is compared with name in order to separate as Asset from project settings.
        //  However, it is recommended to use Layer or Tag.
        string goName = colTrans.name;
        if (goName.Contains(NAME_ENEMY_BULLET)) {
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.parent.gameObject);
        }

        if (goName.Contains(NAME_ENEMY)) {
            UbhManager manager = FindObjectOfType<UbhManager>();
            if (manager != null) {
                manager.GameOver();
            }

            _Spaceship.Explosion();

            Destroy(gameObject);
        }
    }
}
