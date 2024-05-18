using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat_Manager : MonoBehaviour
{
    public GameObject enemyMeatPrefab;
    public int amountOfMeat = 1; // Number of meat pieces to drop
    public float spawnRadius = 1.0f; // Radius within which to randomize spawn positions

    private void OnDestroy()
    {
        for (int i = 0; i < amountOfMeat; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius), // Random x offset
                Random.Range(-spawnRadius, spawnRadius), // Random y offset
                0 // Keep z offset to 0 for 2D game
            );
            
            Instantiate(enemyMeatPrefab, transform.position + randomOffset, Quaternion.identity);
        }
    }
}
