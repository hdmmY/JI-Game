using UnityEngine;

[RequireComponent (typeof (JIBulletMovement), typeof (JIBulletProperty))]
public abstract class BaseBulletMoveCtrl : MonoBehaviour
{
    protected JIBulletProperty _bullet;

    protected JIBulletMovement _bulletMove;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected void Awake ()
    {
        _bullet = GetComponent<JIBulletProperty> ();
        _bulletMove = GetComponent<JIBulletMovement> ();
    }

    public abstract void Init ();
}