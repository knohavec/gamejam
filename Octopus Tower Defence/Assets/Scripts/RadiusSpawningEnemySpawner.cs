using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SquareSpawningEnemySpawner : MonoBehaviour
{
    public List<Level> levels; // List of levels
    public Transform baseTransform;

    [Header("Spawn Settings")]
    public float timeBetweenWaves = 10f; // Changed default value to 10
    public float outerSpawnRange = 5f;
    public float innerNoSpawnRange = 2f;

    public float difficultyScaling = 0.75f;

    private int currentWave = 1;
    private int currentLevel = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator SpawnEnemies()
{
    isSpawning = true;
    int enemiesToSpawn = EnemiesPerWave();
    Debug.Log("Level " + (currentLevel + 1) + " Wave " + currentWave + ": Spawning " + enemiesToSpawn + " enemies."); // Debug message

    for (int i = 0; i < enemiesToSpawn; i++)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(levels[currentLevel].enemyPrefabs[Random.Range(0, levels[currentLevel].enemyPrefabs.Count)], spawnPosition, Quaternion.identity);

        // Ensure the enemy has the correct sorting layer and order
        SpriteRenderer enemyRenderer = enemy.GetComponent<SpriteRenderer>();
        if (enemyRenderer != null)
        {
            enemyRenderer.sortingLayerName = "Enemy"; // Ensure the sorting layer is set correctly
            enemyRenderer.sortingOrder = 1; // Adjust as needed
        }
        else
        {
            Debug.LogWarning("Enemy does not have a SpriteRenderer component.");
        }

        // Debug log to check where the enemy is instantiated
        // Debug.Log($"Spawned enemy at position: {spawnPosition}");

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
            if (currentWave > levels[currentLevel].waveCap)
            {
                currentWave = 1;
                currentLevel++;
                if (currentLevel >= levels.Count)
                {
                    Debug.Log("All levels completed!");
                    yield break; // Exit the coroutine when all levels are completed
                }
                Debug.Log("Proceeding to Level " + (currentLevel + 1));
            }
            // Wait until all enemies are destroyed before starting the next wave
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return new WaitForSeconds(1f);
            }
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
        while (Vector2.Distance(Vector2.zero, randomOffset) < innerNoSpawnRange)
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
