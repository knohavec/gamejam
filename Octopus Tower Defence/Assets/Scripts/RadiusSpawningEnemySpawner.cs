using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RadiusSpawningEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform baseTransform;
    public float enemiesPerSecond = 1f;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f;
    public float spawnRadius = 5f;
    public float spawnDelay = 0.5f;

    private LineRenderer lineRenderer;
    private float timer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;

        UpdateCirclePosition();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / enemiesPerSecond)
        {
            timer = 0f;
            StartCoroutine(SpawnEnemies());
        }
    }

    void UpdateCirclePosition()
{
    int numPoints = 100;
    lineRenderer.positionCount = numPoints;

    Vector3 basePosition = baseTransform.position;

    for (int i = 0; i < numPoints; i++)
    {
        float angle = i * 2 * Mathf.PI / numPoints;
        Vector3 point = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * spawnRadius;
        lineRenderer.SetPosition(i, point + basePosition);
    }

    lineRenderer.startColor = Color.clear;
    lineRenderer.endColor = Color.clear;
}



    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector3 spawnPosition = baseTransform.position + Quaternion.Euler(0f, 0f, randomAngle) * (Vector3.right * spawnRadius);

            if (Vector3.Distance(spawnPosition, baseTransform.position) < 2f)
            {
                continue;
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            Vector3 moveDirection = (baseTransform.position - enemy.transform.position).normalized;
            enemy.GetComponent<Movement>().SetMoveDirection(moveDirection);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
