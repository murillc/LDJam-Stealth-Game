using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;

        SpawnEnemy();
    }

    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform);

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();

        enemy.GetComponent<EnemyMovement>().spawnPoint = EntryPoints.instance.entryPoints[Random.Range(0, EntryPoints.instance.entryPoints.Count)];
    }
}
