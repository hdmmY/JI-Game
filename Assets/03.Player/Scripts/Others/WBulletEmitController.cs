using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBulletEmitController : MonoBehaviour
{
    #region Public 

    public float BulletSpeed;

    public GameObject BulletPrefab;

    public List<Transform> NormalEmitters;

    public List<Transform> ExtraEmitters;

    public void NormalShot ()
    {
        if (NormalEmitters.Count == 0) return;

        foreach (var emitter in NormalEmitters)
        {
            var bullet = BulletUtils.GetBullet (BulletPrefab, null, emitter.position,
                emitter.rotation);

            float angle = bullet.transform.rotation.eulerAngles.z + 90f;

            bullet.Shot (BulletSpeed, angle, 0, 0,
                false, null, 0, 0,
                false, 0, 0,
                false, 0, 0);
        }
    }

    public void OverloadShot ()
    {
        NormalShot ();

        if (ExtraEmitters.Count == 0) return;

        foreach (var emitter in ExtraEmitters)
        {
            var bullet = BulletUtils.GetBullet (BulletPrefab, null, emitter.position,
                emitter.rotation);

            float angle = bullet.transform.rotation.eulerAngles.z + 90f;

            bullet.Shot (BulletSpeed, angle, 0, 0,
                false, null, 0, 0,
                false, 0, 0,
                false, 0, 0);
        }
    }

    #endregion

    [SerializeField]
    private bool _alwaysShowGizmos;

    private void OnDrawGizmos ()
    {
        if (_alwaysShowGizmos) DrawHelper ();
    }

    private void OnDrawGizmosSelected ()
    {
        DrawHelper ();
    }

    private void DrawHelper ()
    {
        Vector3 start, end;

        Gizmos.color = Color.green;
        foreach (var emitter in NormalEmitters)
        {
            start = emitter.position;
            end = start + emitter.up * 1f;
            Gizmos.DrawLine (start, end);
            Gizmos.DrawCube (start, Vector3.one * 0.1f);
        }

        Gizmos.color = Color.red;
        foreach (var emitter in ExtraEmitters)
        {
            start = emitter.position;
            end = start + emitter.up * 1f;
            Gizmos.DrawLine (start, end);
            Gizmos.DrawCube (start, Vector3.one * 0.1f);
        }
    }

}