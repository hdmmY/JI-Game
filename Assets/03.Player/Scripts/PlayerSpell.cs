using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using MovementEffects;

[RequireComponent (typeof (PlayerProperty))]
public class PlayerSpell : MonoBehaviour
{
    #region Public Variables

    [BoxGroup ("Black")]
    public GameObject BombPrefab;

    [BoxGroup ("Black")]
    public float BombFlyDistance;

    [BoxGroup ("Black")]
    public float BombFlyTime;

    [BoxGroup ("Black")]
    public float BombExplodTime;

    #endregion

    #region Private Variables and Methods

    private PlayerProperty _player;

    private void OnEnable ()
    {
        _player = GetComponent<PlayerProperty> ();
    }

    private void Update ()
    {
        if (InputManager.Instance.InputCtrl.SpellButtonDown)
        {
            if (_player.m_playerState == JIState.Black)
            {
                ShotBlackBomb ();
            }
            else if (_player.m_playerState == JIState.White)
            {

            }
        }
    }

    private void ShotBlackBomb ()
    {
        Vector3 bombPos = transform.position + Vector3.up * 0.3f;
        Quaternion bombRot = Quaternion.identity;

        GameObject bombGo = Instantiate (BombPrefab, bombPos, bombRot);

        // Move
        var moveEffect = new Effect<Transform, Vector3> ();
        moveEffect.Duration = BombFlyTime;
        moveEffect.RetrieveStart = (bomb, lastValue) => bomb.position;
        moveEffect.RetrieveEnd = (bomb) => bombPos + Vector3.up * BombFlyDistance;
        moveEffect.OnUpdate = (bomb, newPos) => bomb.position = newPos;
        moveEffect.OnDone = (bomb) =>
        {
            var bombCtrl = bomb.GetComponent<BombController> ();
            bombCtrl.enabled = true;
            bombCtrl.AttractFieldRenderer.SetActive (true);
            bombCtrl.DestroyFieldRenderer.SetActive (true);
            bombCtrl.UnexplodeBombRenderer.SetActive (false);
        };

        // explode
        var explodeEffect = new Effect<Transform, Vector3> ();
        explodeEffect.Duration = BombExplodTime;
        explodeEffect.RetrieveStart = (bomb, lastValue) => lastValue;
        explodeEffect.OnUpdate = (bomb, newPos) => { };
        explodeEffect.OnDone = (bomb) => Destroy (bomb.gameObject, 0.1f);

        var seq = new Sequence<Transform, Vector3> ();
        seq.Reference = bombGo.transform;
        seq.Add (moveEffect);
        seq.Add (explodeEffect);
        Movement.Run (seq);
    }

    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.white;

        Vector3 start = transform.position + Vector3.up * 0.3f;
        Vector3 end = start + Vector3.up * BombFlyDistance;

        Gizmos.DrawLine (start, end);
    }

    #endregion

}