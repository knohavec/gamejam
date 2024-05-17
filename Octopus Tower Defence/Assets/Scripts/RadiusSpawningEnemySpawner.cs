using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSpawningEnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public Transform baseTransform;

    [Header("Spawn Settings")]
    public float timeBetweenWaves = 10f; // Changed default value to 10
    public float outerSpawnRange = 5f;
    public float innerNoSpawnRange = 2f;

    public float difficultyScaling = 0.75f;

    private int currentWave = 1;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator SpawnEnemies()
    {
        isSpawning = true;
        int enemiesToSpawn = EnemiesPerWave();
        Debug.Log("Wave " + currentWave + ": Spawning " + enemiesToSpawn + " enemies."); // Debug message
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            spawnPosition.z -= 1f;
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.5f); // Adjust spawn delay as needed
        }
        isSpawning = false;
    }

    IEnumerator StartWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWave++;
            StartCoroutine(SpawnEnemies());
        }
    }

    private int EnemiesPerWave()
    {
        int enemies = Mathf.RoundToInt(currentWave * difficultyScaling);
        return Mathf.Max(enemies, 1);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 basePosition = baseTransform.position;
        Vector2 randomOffset = new Vector2(Random.Range(-outerSpawnRange, outerSpawnRange), Random.Range(-outerSpawnRange, outerSpawnRange));
        Vector3 spawnPosition = basePosition + new Vector3(randomOffset.x, randomOffset.y, 0);

        // Check if the spawn position is within the inner no-spawn range, if so, reposition
        while (Mathf.Abs(randomOffset.x) < innerNoSpawnRange && Mathf.Abs(randomOffset.y) < innerNoSpawnRange)
        {
            randomOffset = new Vector2(Random.Range(-outerSpawnRange, outerSpawnRange), Random.Range(-outerSpawnRange, outerSpawnRange));
            spawnPosition = basePosition + new Vector3(randomOffset.x, randomOffset.y, 0);
        }

        return spawnPosition;
    }

    public void EnemyDestroyed()
    {
        if (isSpawning) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            StartCoroutine(StartWave());
        }
    }
}
