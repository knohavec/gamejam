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

    public static int sand_dollar_total = 0;
    public int SandDollarTotal => sand_dollar_total;

    public delegate void SandDollarTotalChanged(int newTotal);
    public static event SandDollarTotalChanged OnSandDollarTotalChanged;

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
            Instantiate(sand_dollar_prefab, position, Quaternion.identity);
            timeSinceLastSpawn = 0;
        }
    }

    Vector2 RandomPositionOnMap()
    {
        float randomX = Random.Range(-spawn_range, spawn_range);
        float randomY = Random.Range(-spawn_range, spawn_range);
        return new Vector2(randomX, randomY);
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        SpawnSandDollars();
    }

    public static void SpendSandDollars(int cost)
{
    if (cost < 0)
    {
        Debug.LogError("Cost cannot be negative.");
        return;
    }

    sand_dollar_total = Mathf.Max(0, sand_dollar_total - cost);
    OnSandDollarTotalChanged?.Invoke(sand_dollar_total);
    SandDollar.counterText.text = SandDollarSpawning.sand_dollar_total.ToString();
}

}
