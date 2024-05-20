using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDollarSpawning : MonoBehaviour
{
    public static SandDollarSpawning Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject sand_dollar_prefab;

    [Header("Attributes")]
    [SerializeField] private float spawn_rate = 1.0f;
    [SerializeField] private float spawn_range = 10.0f; // Half the side length of the square spawn area
    private float timeSinceLastSpawn = 0.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of SandDollarSpawning already exists. Deleting this one.");
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 center = transform.position;
        Vector3 halfExtents = Vector3.one * spawn_range;
        Gizmos.DrawWireCube(center, 2 * halfExtents);
    }

    void SpawnSandDollars()
    {
        if (timeSinceLastSpawn >= spawn_rate)
        {
            Vector2 position = RandomPositionOnMap();
            if (!IsPositionOccupied(position))
            {
                Instantiate(sand_dollar_prefab, position, Quaternion.identity);
                timeSinceLastSpawn = 0;
            }
        }
    }

    Vector2 RandomPositionOnMap()
    {
        float randomX = Random.Range(-spawn_range, spawn_range);
        float randomY = Random.Range(-spawn_range, spawn_range);
        return new Vector2(randomX, randomY);
    }

    private bool IsPositionOccupied(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tile") || collider.CompareTag("Tower"))
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        SpawnSandDollars();
    }
}
