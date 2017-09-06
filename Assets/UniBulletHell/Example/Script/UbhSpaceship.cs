using UnityEngine;
using System.Collections;

public class UbhSpaceship : UbhMonoBehaviour
{
    public float _Speed;
    [SerializeField]
    GameObject _ExplosionPrefab;
    Animator _Animator;

    void Start ()
    {
        _Animator = GetComponent<Animator>();
    }

    public void Explosion ()
    {
        if (_ExplosionPrefab != null) {
            Instantiate(_ExplosionPrefab, transform.position, transform.rotation);
        }
    }

    public Animator GetAnimator ()
    {
        return _Animator;
    }
}
