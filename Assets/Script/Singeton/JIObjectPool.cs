using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class JIObjectPool : MonoBehaviour
{
    public Action<GameObject> OnSpawnedEvent;

    public Action<GameObject> OnDespawnedEvent;

    public GameObject m_prefab;

    public int m_instancesToPreallocate = 5;

    /// <summary>
	/// Total number of instances to allocate if one is requested when the bin is empty
	/// </summary>
    public static int InstanceToPreAllocateIfEmpty = 1;

    /// <summary>
	/// If true, the object pool will not create any more instances when m_maxLimit is reached and will instead return null for any spanws
	/// </summary>
    public bool m_enbleMaxLimit = false;

    /// <summary>
    /// If m_enbleMaxLimit is true, this will be the maximum number of instances to create
    /// </summary>
    public int m_maxLimit = 5;

    public bool m_cullExcessPrefabs = false;

    /// <summary>
	/// Total instances to keep in the pool. All excess will be culled if cullExcessPrefabs is true
	/// </summary>
    public int m_instantesToMaintainInPool = 5;

    /// <summary>
	/// How often in seconds should culling occur
	/// </summary>
    public float m_cullInterval = 10f;

    /// <summary>
	/// Stores all of our GameObjects
	/// </summary>
    private Stack<GameObject> _gameObjectPool;

    /// <summary>
	/// Last time culling happened
	/// </summary
    private float _timeOfLastCull = float.MinValue;

    /// <summary>
	/// Keeps track of the total number of instances spawned
	/// </summary>
    private int _spawnedInstanceCount = 0;


    public int SpawnedInstanceCount
    {
        get
        {
            return _spawnedInstanceCount;
        }
    }

    public int InPoolObjectCount
    {
        get
        {
            if (_gameObjectPool == null)
                return 0;
            else
                return _gameObjectPool.Count;
        }
    }



    #region Private

    private void AllocateGameObject(int count)
    {
        if (m_enbleMaxLimit)
        {
            count = Mathf.Min(count, m_maxLimit - _gameObjectPool.Count - _spawnedInstanceCount);
        }

        for (int n = 0; n < count; n++)
        {
            GameObject go = Instantiate(m_prefab, JIObjectPoolManager.Instance.transform);
            go.name = m_prefab.name;
            go.SetActive(false);

            _gameObjectPool.Push(go);
        }
    }

    /// <summary>
	/// Pops an object off the stack. Returns null if we hit the m_maxLimited.
	/// </summary>
    private GameObject Pop()
    {
        if (m_enbleMaxLimit && _spawnedInstanceCount >= m_maxLimit)
            return null;

        if (_gameObjectPool.Count > 0)
        {
            _spawnedInstanceCount++;
            return _gameObjectPool.Pop();
        }

        AllocateGameObject(InstanceToPreAllocateIfEmpty);
        return Pop();
    }

    #endregion


    #region Public

    /// <summary>
	/// Preps the Stack and does preallocation
	/// </summary>
    public void Initialize()
    {
        Clear();
        _gameObjectPool = new Stack<GameObject>(m_instancesToPreallocate);
        AllocateGameObject(m_instancesToPreallocate);
    }

    /// <summary>
	/// Culls any excess objects if necessary
	/// </summary>
    public void CullExcessObjects()
    {
        if (!m_cullExcessPrefabs || _gameObjectPool.Count <= m_instantesToMaintainInPool)
        {
            return;
        }

        if (Time.time > (_timeOfLastCull + m_cullInterval))
        {
            _timeOfLastCull = Time.time;
            for (int i = _gameObjectPool.Count; i > m_instantesToMaintainInPool; i--)
            {
                Destroy(_gameObjectPool.Pop());
            }
        }
    }


    /// <summary>
	/// Fetches a new instance from the pool. Returns null if reached the m_maxLimits.
	/// </summary>
    public GameObject Spawn()
    {
        var go = Pop();

        if (go != null)
        {
            if (OnSpawnedEvent != null)
            {
                OnSpawnedEvent(go);
            }
        }

        return go;
    }

    /// <summary>
	/// Returns an instance to the pool
	/// </summary>
	/// <param name="go">Go.</param>
    public void Despawn(GameObject go)
    {
        go.SetActive(false);

        _spawnedInstanceCount--;
        _gameObjectPool.Push(go);

        if (OnDespawnedEvent != null)
        {
            OnDespawnedEvent(go);
        }
    }


    /// <summary>
	/// Clears out the pool by calling Destroy on all objects in it. Note that any spawned objects are not touched by this operation!
	/// </summary>
    public void Clear()
    {
        if (_gameObjectPool == null)
            return;

        while (_gameObjectPool.Count > 0)
        {
            var go = _gameObjectPool.Pop();

            Destroy(go);
        }
    }

    #endregion


}
