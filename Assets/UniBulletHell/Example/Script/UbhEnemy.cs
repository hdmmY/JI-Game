using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UbhSpaceship))]
public class UbhEnemy : UbhMonoBehaviour
{
    public const string NAME_PLAYER = "Player";
    public const string NAME_PLAYER_BULLET = "PlayerBullet";
    const string ANIM_DAMAGE_TRIGGER = "Damage";
    [SerializeField]
    int _Hp = 1;
    [SerializeField]
    int _Point = 100;
    [SerializeField]
    bool _UseStop = false;
    [SerializeField]
    float _StopPoint = 2f;
    UbhSpaceship _Spaceship;

    void Start ()
    {
        _Spaceship = GetComponent<UbhSpaceship>();

        Move(transform.up.normalized * -1);
    }

    void FixedUpdate ()
    {
        if (_UseStop) {
            if (transform.position.y < _StopPoint) {
                rigidbody2D.velocity = Vector2.zero;
                _UseStop = false;
            }
        }
    }

    public void Move (Vector2 direction)
    {
        rigidbody2D.velocity = direction * _Spaceship._Speed;
    }

    void OnTriggerEnter2D (Collider2D c)
    {
        // *It is compared with name in order to separate as Asset from project settings.
        //  However, it is recommended to use Layer or Tag.
        if (c.name.Contains(NAME_PLAYER_BULLET)) {
            UbhSimpleBullet bullet = c.transform.parent.GetComponent<UbhSimpleBullet>();

            UbhObjectPool.Instance.ReleaseGameObject(c.transform.parent.gameObject);

            _Hp = _Hp - bullet._Power;

            if (_Hp <= 0) {
                FindObjectOfType<UbhScore>().AddPoint(_Point);

                _Spaceship.Explosion();

                Destroy(gameObject);
            } else {
                _Spaceship.GetAnimator().SetTrigger(ANIM_DAMAGE_TRIGGER);
            }
        }
    }
}