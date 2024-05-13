using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RadiusSpawningEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform baseTransform; // Reference to the base's transform
    public float enemiesPerSecond = 1f; // Number of enemies to spawn per second
    public int enemiesPerWave = 5; // Number of enemies to spawn per wave
    public float timeBetweenWaves = 5f; // Time between each wave
    public float spawnRadius = 5f; // Spawn radius around the base
    public float spawnDelay = 0.5f; // Delay between each enemy spawn

    private LineRenderer lineRenderer; // Reference to the LineRenderer component
    private float timer; // Timer to keep track of spawning

    void Start()
    {
        // Initialize the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;

        // Set the circle points for the LineRenderer
        int numPoints = 100;
        lineRenderer.positionCount = numPoints;
        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * 2 * Mathf.PI / numPoints;
            Vector3 point = new Vector3(Mathf.Cos(angle) * spawnRadius, Mathf.Sin(angle) * spawnRadius, 0f);
            lineRenderer.SetPosition(i, point + baseTransform.position);
        }

        // Set the LineRenderer's color to fully transparent
        lineRenderer.startColor = Color.clear;
        lineRenderer.endColor = Color.clear;
    }

    void Update()
    {
        // Spawn enemies based on the given rate
        timer += Time.deltaTime;
        if (timer >= 1f / enemiesPerSecond)
        {
            timer = 0f;
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            // Calculate a random angle around the base
            float randomAngle = Random.Range(0f, 360f);
            Vector3 spawnPosition = baseTransform.position + Quaternion.Euler(0f, 0f, randomAngle) * (Vector3.right * spawnRadius);

            // Check if the spawn position is inside or underneath the base
            if (Vector3.Distance(spawnPosition, baseTransform.position) < 1f)
            {
                continue; // Skip this iteration and try again
            }

            // Instantiate the enemy at the spawn position
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Set the initial movement direction towards the base
            Vector3 moveDirection = (baseTransform.position - enemy.transform.position).normalized;
            enemy.GetComponent<Movement>().SetMoveDirection(moveDirection);

            // Wait for the spawn delay before spawning the next enemy
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
