using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LaserController : MonoBehaviour {

    // The laser stay time in seconds. 
    // Don't contains laser appear animation.
    public float m_laserLife = 5f;

    // The laser appear and disappear animation.
    public AnimationClip m_LaserAppear;
    public AnimationClip m_LaserDisappear;

    private float _timer;

    // The laser appear/disappear animation length in seconds.
    private float _appearAnimLength;
    private float _disappearAnimLength;

    private Animator _animator;


    private void OnEnable()
    {
        _timer = 0f;

        _animator = GetComponent<Animator>();
        _animator.Play("LaserAppear");

        _appearAnimLength = m_LaserAppear.length;
        _disappearAnimLength = m_LaserDisappear.length;
    }


    private void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        if(_timer > (_appearAnimLength + m_laserLife))
        {
            StartCoroutine(LaserDisappear());
            _timer = float.MinValue;
        }
    }

    IEnumerator LaserDisappear()
    {
        _animator.Play("LaserDisappear");

        yield return new WaitForSeconds(_disappearAnimLength);

        transform.parent.gameObject.SetActive(false);
    }

}
