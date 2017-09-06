using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject m_BulletPrefab;

    public Transform m_ParentTransform;

    public int m_AddedNumberWhenReset = 40;

    [Range(5, 1000)]
    public int m_Capacity;

    public int m_activedObjects;

    private List<GameObject> _pool;
    
    private LinkedList<GameObject> _disactivedObjects;

    private int _bulletIndex;

    private void OnEnable()
    {
        _pool = new List<GameObject>();
        _disactivedObjects = new LinkedList<GameObject>();

        _bulletIndex = 0;

        ResetPool(m_Capacity);
    }

    private void Update()
    {
        m_activedObjects = m_Capacity - _disactivedObjects.Count;
    }



    public GameObject create()
    {
        if (_disactivedObjects.Count == 0)
        {                         
            ResetPool(m_AddedNumberWhenReset);
        }

        if (_pool == null)
        {
            Debug.LogWarning(transform.parent + ": the bullet pool is null!");
            return null;
        }

        GameObject result = _disactivedObjects.Last.Value;

        //result.SetActive(true);
        ResetBulletProperty(result.GetComponent<Bullet_Property>());

        _disactivedObjects.RemoveLast();


        return result;
    }

    public GameObject create(Vector3 position)
    {
        GameObject go = create();

        go.transform.position = position;

        return go;
    }


    public void delete(GameObject bullet)
    {
        bullet.SetActive(false);

        _disactivedObjects.AddFirst(bullet);             

        //bullet.GetComponent<Bullet_Property>().CopyProperty(m_BulletPrefab.GetComponent<Bullet_Property>());

        return;
    }


    public void clear()
    {
        _disactivedObjects.Clear();

        _pool.Clear();
    }


    private void ResetPool(int addedNumber)
    {
        GameObject go;

        for (int i = 0; i < addedNumber; i++)
        {
            go = Instantiate(m_BulletPrefab, m_ParentTransform);
            go.SetActive(false);

            go.name += _bulletIndex.ToString();
            _bulletIndex++;

            _pool.Add(go);
            _disactivedObjects.AddFirst(go);
        }
      

        m_Capacity = _pool.Count;
    }


    // reset the new bullet's property to the default property
    private void ResetBulletProperty(Bullet_Property property)
    {
        property.m_LifeTime = BulletPropertyDefault.LifeTime;

        property.m_BulletColor = BulletPropertyDefault.Color;

        property.m_Alpha = BulletPropertyDefault.Alpha;

        property.m_SpriteDirection = BulletPropertyDefault.SpriteDirection;

        property.m_AlignWithVelocity = BulletPropertyDefault.AlignWithVelocity;

        property.m_BulletSpeed = BulletPropertyDefault.BulletSpeed;

        property.m_Accelerate = BulletPropertyDefault.Accelerator;

        property.m_AcceleratDir = BulletPropertyDefault.AcceleratorDirection;

        property.m_HorizontalVelocityFactor = BulletPropertyDefault.HorizontalVelocityFactor;

        property.m_VerticalVelocityFactor = BulletPropertyDefault.VerticalVelocityFactor;

        property.m_useBulletAttrack = BulletPropertyDefault.UseBulletAttrack;

        property.m_useBulletReject = BulletPropertyDefault.UseBulletReject;

        property.m_attrackFactor = BulletPropertyDefault.AttrackFactor;

        property.m_rejectFactor = BulletPropertyDefault.RejectFactor;

        property.m_targetTrans = BulletPropertyDefault.TargetTrans;

        property.m_BulletDamage = BulletPropertyDefault.BulletDamage;
    }

    
}
