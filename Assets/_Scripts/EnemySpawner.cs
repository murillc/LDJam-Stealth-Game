using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    public bool spawn = true;
    [System.NonSerialized] public float spawnRate;
    public GameObject enemyPrefab;
    public Coroutine coroutine;

    void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;

        spawnRate = -0.12f * PlayerStats.instance.heat + 15;
        Debug.Log(spawnRate);
    }

    void Update()
    {
    }

    public void StartSpawningEnemies()
    {
        Debug.Log("START SPAWNING");
        coroutine = StartCoroutine(IE_SpawnEnemy());
    }

    public void StopSpawningEnemies()
    {
        Debug.Log("STOP SPAWNING");
        StopCoroutine(coroutine);
    }

    void SpawnEnemy()
    {
        if (EntryPointsManager.instance.entryPoints.Count <= 0)
        {
            Debug.Log("NO AVAILABLE ENTRYPOINTS");
            return;
        }

        Debug.Log("SPAWN ENEMY");
        GameObject enemy = Instantiate(enemyPrefab, transform);
        Vector3 spawnPoint = EntryPointsManager.instance.GetEntryPoint();
        enemy.GetComponent<EnemyMovement>().spawnPoint = new Vector3(spawnPoint.x + 0.5f, spawnPoint.y + 0.5f, 0);
    }

    IEnumerator IE_SpawnEnemy()
    {
        Debug.Log("ENUMERATOR");

        while (spawn)
        {
            yield return new WaitForSeconds(spawnRate);

            SpawnEnemy();
        }
    }
}
