using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool spawn = true;
    public float spawnRate = 2f;
    public GameObject enemyPrefab;

    void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;

        StartCoroutine(IE_SpawnEnemy());
    }

    void Update()
    {

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
        while (spawn)
        {
            yield return new WaitForSeconds(spawnRate);

            SpawnEnemy();
        }
    }
}
