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
        SpawnEnemy();
        SpawnEnemy();
    }

    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform);

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();

        Vector3 spawnPoint = EntryPoints.instance.entryPoints[Random.Range(0, EntryPoints.instance.entryPoints.Count)];

        enemy.GetComponent<EnemyMovement>().spawnPoint = new Vector3(spawnPoint.x + 0.5f, spawnPoint.y + 0.5f, 0);
    }
}
