using UnityEngine;
using System.Collections;

/// <summary>
/// Ubh coroutine.
/// </summary>
public class JICoroutine : JISingletonMonoBehavior<JICoroutine>
{
    protected override void Start ()
    {
        base.Start();
    }

    /// <summary>
    /// Start continue coroutine of object in non-active.
    /// Example : UbhCoroutine.Start (CoroutineMethod());
    /// </summary>
    public static Coroutine StartIE (IEnumerator routine)
    {
        return Instance.StartCoroutine(routine);
    }
}
