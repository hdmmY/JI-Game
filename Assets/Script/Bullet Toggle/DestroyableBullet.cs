using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DestroyableBullet : MonoBehaviour
{
    public string m_bulletTag;

    public float m_destroyVelocity;

    [Range(0, 100)]
    public int m_bulletDestroyableHealth;

    public System.Action DestoryBullet;

    public System.Func<float> CurrentBulletVelocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(m_bulletTag))
        {
            if (Mathf.Approximately(CurrentBulletVelocity(), m_destroyVelocity))
            {
                if(DestoryBullet != null)
                {
                    if (m_bulletDestroyableHealth < 0)
                    {
                        DestoryBullet();
                    }
                    else
                    {
                        int bulletDamage = collision.transform.parent.GetComponent<UbhBullet>().m_damage;
                        m_bulletDestroyableHealth -= bulletDamage;
                    }
                }

                UbhObjectPool.Instance.ReleaseGameObject(collision.transform.parent.gameObject);
            }
        }               
    }

    private void OnDisable()
    {
        Destroy(this);
    }


}
