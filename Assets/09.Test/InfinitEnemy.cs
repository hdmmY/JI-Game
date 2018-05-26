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

    private int _index = 0;

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
            pos.x = Random.Range (SpawnArea.xMin, SpawnArea.xMax);
            pos.y = Random.Range (SpawnArea.yMin, SpawnArea.yMax);
            pos.z = EnemyPrefab.transform.position.z;

            var enemy = Instantiate (EnemyPrefab, pos, EnemyPrefab.transform.rotation);
            enemy.name = string.Format ("{0} + {1}", enemy.name, _index++);

            if (enemy.GetComponent<EnemyProperty> () == null)
            {
                Debug.LogError ("EnemyProperty component is not instantiate!");
            }

            _spawnedEnemies.Add (enemy);
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube (SpawnArea.center, SpawnArea.size);
    }
}