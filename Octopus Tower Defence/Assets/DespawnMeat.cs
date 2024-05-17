using UnityEngine;

public class DespawnMeat : MonoBehaviour
{
    public float despawnTime = 5f; // Default despawn time of 5 seconds, can be set in the Inspector

    void Start()
    {
        Invoke("Despawn", despawnTime); // Invoke the Despawn method after despawnTime seconds
    }

    void Despawn()
    {
        Destroy(gameObject); // Destroy the GameObject this script is attached to
    }
}
