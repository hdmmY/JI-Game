// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// *Please enable this define if you want to use the DarkTonic's CoreGameKit pooling system.
// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// #define USING_CORE_GAME_KIT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if USING_CORE_GAME_KIT
using DarkTonic.CoreGameKit;
#endif


/// <summary>
/// Ubh object pool.
/// </summary>
public class UbhObjectPool : UbhSingletonMonoBehavior<UbhObjectPool>
{
#if USING_CORE_GAME_KIT
    // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
    PoolBoss _PoolBoss = null;
#else
    List<int> _PooledKeyList = new List<int>();
    Dictionary<int, List<GameObject>> _PooledGoDic = new Dictionary<int, List<GameObject>>();
#endif

    protected override void Awake ()
    {
        base.Awake();
    }

    /// <summary>
    /// Get GameObject from object pool or instantiate.
    /// </summary>
    public GameObject GetGameObject (GameObject prefab, Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        if (prefab == null) {
            return null;
        }

#if USING_CORE_GAME_KIT
        // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
        if (_PoolBoss == null) {
            // PoolBoss Initialize
            _PoolBoss = FindObjectOfType<PoolBoss>();
            if (_PoolBoss == null) {
                _PoolBoss = new GameObject(typeof(PoolBoss).Name).AddComponent<PoolBoss>();
            }
            _PoolBoss.autoAddMissingPoolItems = true;
        }
        return PoolBoss.Spawn(prefab.transform, position, rotation, _Transform).gameObject;
#else
        int key = prefab.GetInstanceID();

        if (_PooledKeyList.Contains(key) == false && _PooledGoDic.ContainsKey(key) == false) {
            _PooledKeyList.Add(key);
            _PooledGoDic.Add(key, new List<GameObject>());
        }

        List<GameObject> goList = _PooledGoDic[key];
        GameObject go = null;

        if (forceInstantiate == false) {
            for (int i = goList.Count - 1; i >= 0; i--) {
                go = goList[i];
                if (go == null) {
                    goList.Remove(go);
                    continue;
                }
                if (go.activeSelf == false) {
                    // Found free GameObject in object pool.
                    Transform goTransform = go.transform;
                    goTransform.position = position;
                    goTransform.rotation = rotation;
                    go.SetActive(true);
                    return go;
                }
            }
        }

        // Instantiate because there is no free GameObject in object pool.
        go = (GameObject) Instantiate(prefab, position, rotation);
        go.transform.parent = _Transform;
        goList.Add(go);

        return go;
#endif
    }

    /// <summary>
    /// Releases game object (back to pool or destroy).
    /// </summary>
    public void ReleaseGameObject (GameObject go, bool destroy = false)
    {
#if USING_CORE_GAME_KIT
        // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
        PoolBoss.Despawn(go.transform);
#else
        if (destroy) {
            Destroy(go);
            return;
        }
        go.SetActive(false);
#endif
    }

    /// <summary>
    /// Get active bullets count.
    /// </summary>
    public int GetActivePooledObjectCount ()
    {
#if USING_CORE_GAME_KIT
        var bullets = GetComponentsInChildren<UbhBullet>();
        return bullets == null ? 0 : bullets.Length;
#else
        int cnt = 0;
        for (int i = 0; i < _PooledKeyList.Count; i++) {
            int key = _PooledKeyList[i];
            var goList = _PooledGoDic[key];
            for (int j = 0; j < goList.Count; j++) {
                var go = goList[j];
                if (go != null && go.activeInHierarchy) {
                    cnt++;
                }
            }
        }
        return cnt;
#endif
    }
}