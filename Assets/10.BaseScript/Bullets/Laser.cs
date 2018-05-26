using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public enum LaserType
{
    Constant,
    Growth
}

[RequireComponent (typeof (JIBulletProperty))]
public abstract class Laser : MonoBehaviour
{
    #region Public variables

    public int Damage;

    public float Interval;

    [OnValueChanged ("UpdateLaserEditor")]
    public float LaserLength;

    [OnValueChanged ("UpdateLaserEditor")]
    public float LaserWidth;

    public RuntimeAnimatorController LaserAnim;

    [HideInInspector] public float DamageTimer;

    public abstract LaserType LaserType { get; }

    #endregion

    #region Monobehavior

    protected virtual void OnEnable ()
    {
        _bullet = GetComponent<JIBulletProperty> ();
        _boxcol2D = transform.GetChild (0).GetComponent<BoxCollider2D> ();
        _renderer = transform.GetChild (1).GetComponent<LineRenderer> ();

        _anim = GetComponent<Animator> () == null ?
            gameObject.AddComponent<Animator> () : GetComponent<Animator> ();
        _anim.runtimeAnimatorController = LaserAnim;

        var rb = GetComponent<Rigidbody> ();
    }

    protected virtual void Update ()
    {
        DamageTimer += JITimer.Instance.DeltTime;
    }

    #endregion

    #region Pirvate variables

    protected JIBulletProperty _bullet;

    private BoxCollider2D _boxcol2D;

    private LineRenderer _renderer;

    private Animator _anim;

    #endregion

    public void UpdateLaserAppear ()
    {
        _renderer.widthMultiplier = LaserWidth;
        _renderer.SetPosition (1, new Vector3 (0, LaserLength, 0));

        _boxcol2D.offset = new Vector2 (0, LaserLength / 2);
        _boxcol2D.size = new Vector2 (LaserWidth, LaserLength);
    }

#if UNITY_EDITOR

    private void UpdateLaserEditor ()
    {
        var boxcol2D = transform.GetChild (0).GetComponent<BoxCollider2D> ();
        var renderer = transform.GetChild (1).GetComponent<LineRenderer> ();

        renderer.widthMultiplier = LaserWidth;
        renderer.SetPosition (1, new Vector3 (0, LaserLength, 0));

        boxcol2D.size = new Vector2 (LaserWidth, LaserLength);
        boxcol2D.offset = new Vector2 (0, LaserLength / 2);
    }

#endif
}