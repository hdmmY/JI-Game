using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitEnemy : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public int MinEnemyNum;

    public int MaxEnemyNum;

    public Rect SpawnArea;

    private List<GameObject> _spawnedEnemies;

    // Use this for initialization
    void Start ()
    {
        _spawnedEnemies = new List<GameObject> ();

        Spawn ();
    }

    // Update is called once per frame
    void Update ()
    {
        bool allDead = true;

        for (int i = 0; i < _spawnedEnemies.Count; i++)
        {
            allDead &= _spawnedEnemies[i] == null;
        }

        if (allDead)
        {
            Spawn ();
        }
    }

    private void Spawn ()
    {
        int num = Random.Range (MinEnemyNum, MaxEnemyNum);

        Vector3 pos;

        for (int i = 0; i < num; i++)
        {
            pos.x = Random.Range (SpawnArea.left, SpawnArea.right);
            pos.y = Random.Range (SpawnArea.bottom, SpawnArea.top);
            pos.z = EnemyPrefab.transform.position.z;

            _spawnedEnemies.Add (Instantiate (EnemyPrefab, pos, EnemyPrefab.transform.rotation));
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube (SpawnArea.center, SpawnArea.size);
    }
}