using UnityEngine;

public class AttractBulletMoveCtrl : BaseBulletMoveCtrl
{
    public float AttractFactor;

    public float SmoothFactor = 0.5f;

    public Transform GravityCenter;

    public override void Init ()
    {

    }

    private void Update ()
    {
        if (JITimer.Instance.DeltTime == 0) return;
        if (GravityCenter == null)
        {
            this.enabled = false;
            Destroy (this);
            return;
        }

        Vector2 current = _bullet.transform.position;
        Vector2 target = GravityCenter.position;

        float rad = Mathf.Deg2Rad * (_bullet.transform.eulerAngles.z + 90f);
        Vector2 velocity = _bulletMove.Speed * new Vector2 (Mathf.Cos (rad), Mathf.Sin (rad));

        float distance = (target - current).magnitude;

        Vector2 newPos = Vector2.SmoothDamp (current, target, ref velocity,
            SmoothFactor * distance, 10f, AttractFactor * JITimer.Instance.DeltTime);

        _bulletMove.Speed = (newPos - current).magnitude / JITimer.Instance.DeltTime;
        _bulletMove.AngleSpeed = 0f;

        Quaternion rot = Quaternion.Euler (0, 0,
            UbhUtil.GetAngleFromTwoPosition (current, newPos) - 90f);
        _bullet.transform.rotation = rot;
    }
}