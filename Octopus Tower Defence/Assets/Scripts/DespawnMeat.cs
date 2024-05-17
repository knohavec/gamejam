// using UnityEngine;

// public class DespawnMeat : MonoBehaviour
// {
//     public float despawnTime = 5f; // Default despawn time of 5 seconds, can be set in the Inspector

//     public GameObject despawnPrefab; // The prefab to check for collisions with

//     private void Start()
//     {
//         Invoke("Despawn", despawnTime); // Invoke the Despawn method after despawnTime seconds
//     }

//     private void Despawn()
//     {
//         Destroy(gameObject); // Destroy the GameObject this script is attached to
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.gameObject == despawnPrefab)
//         {
//             Destroy(gameObject); // Destroy the meat prefab
//         }
//     }
// }
