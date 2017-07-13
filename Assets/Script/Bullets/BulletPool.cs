using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject m_BulletPrefab;

    public Transform m_ParentTransform;

    [Range(5, 1000)]
    public int m_Capacity;

    private List<GameObject> _pool;

    private LinkedList<GameObject> _disactivedObjects;

    private void OnEnable()
    {
        _pool = new List<GameObject>();
        _disactivedObjects = new LinkedList<GameObject>();

        GameObject go;

        for(int i = 0; i < m_Capacity; i++)
        {
            go = Instantiate(m_BulletPrefab, m_ParentTransform);
            go.SetActive(false);

            _pool.Add(go);
            _disactivedObjects.AddLast(go);
        }

        return;
    }


     public GameObject create()
    {
        if(_disactivedObjects.Count == 0)
        {
            ResetPool();

            return Instantiate(m_BulletPrefab, m_ParentTransform);
        }

        if(_pool == null)
        {
            Debug.LogWarning(transform.parent + ": the bullet pool is null!");
            return null;
        }

        GameObject result = _disactivedObjects.Last.Value;
        result.SetActive(true);

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
        _disactivedObjects.AddLast(bullet);

        bullet.SetActive(false);

        return;
    }


    public void clear()
    {
        _disactivedObjects.Clear();

        _pool.Clear();
    }


    private void ResetPool()
    {
        GameObject go;

        for (int i = 0; i < m_Capacity; i++)
        {
            go = Instantiate(m_BulletPrefab, m_ParentTransform);
            go.SetActive(false);

            _pool.Add(go);
            _disactivedObjects.AddLast(go);
        }

        m_Capacity += 40;
    }
}
