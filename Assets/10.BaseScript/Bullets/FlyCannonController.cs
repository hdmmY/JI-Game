using UnityEngine;
using Sirenix.OdinInspector;

public class FlyCannonController : MonoBehaviour
{
    #region Public Variables

    [BoxGroup ("Shoot")]
    public GameObject BulletPrefab;

    [BoxGroup ("Shoot")]
    public float ShotInterval;

    [BoxGroup ("Shoot")]
    public float BulletSpeed;

    [BoxGroup ("Shoot")]
    public int BulletDamage;

    [BoxGroup ("Move")]
    public PlayerProperty Player;

    [BoxGroup ("Move")]
    public Vector3 Offset;

    [BoxGroup ("Move")]
    public float SmoothTime;

    [HideInInspector] public float ExistTime;

    #endregion

    #region Private variables

    private Vector2 _velocity;

    private float _shotTimer;

    private float _existTimer;

    [BoxGroup ("Shoot"), SerializeField]
    private Vector2 _bulletSpawnOffset;

    private void OnEnable ()
    {
        _shotTimer = 0f;
        _existTimer = 0f;
        _velocity = Vector2.zero;
    }

    private void Update ()
    {
        _existTimer += JITimer.Instance.DeltTime;

        if (_existTimer >= ExistTime)
        {
            Destroy (this.gameObject);
        }

        Move ();
        Shot ();
    }

    private void Move ()
    {
        if (Player == null) return;
        if (Player.m_playerState == JIState.Black) return;

        Vector3 targetPos = Player.transform.position + Offset;

        Vector2 pos = Vector2.SmoothDamp (transform.position, targetPos,
            ref _velocity, SmoothTime, 10, JITimer.Instance.DeltTime);

        transform.position = new Vector3 (pos.x, pos.y, transform.position.z);
    }

    private void Shot ()
    {
        _shotTimer += JITimer.Instance.DeltTime;

        if (_shotTimer > ShotInterval)
        {
            var bullet = BulletUtils.GetBullet (BulletPrefab, null,
                    transform.position + (Vector3) _bulletSpawnOffset,
                    BulletPrefab.transform.rotation)
                .GetComponent<JIBulletProperty> ();
            bullet.Damage = BulletDamage;

            var moveCtrl = bullet.gameObject.AddComponent<GeneralBulletMoveCtrl> ();
            moveCtrl.Speed = BulletSpeed;
            moveCtrl.Init ();

            _shotTimer = 0f;
        }
    }

    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube (transform.position + (Vector3) _bulletSpawnOffset, Vector3.one * 0.05f);
    }

    #endregion

}