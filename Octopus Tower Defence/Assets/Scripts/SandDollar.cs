using UnityEngine;

public class SandDollar : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public int worth = 1;
    [SerializeField] private float despawnDelay = 5f; // Delay before despawning if not clicked

    private bool isClicked = false;

    void Start()
    {
        Invoke(nameof(DespawnOnDelay), despawnDelay); // Start the despawn delay
    }

    void OnMouseDown()
    {
        if (isClicked) return; // Prevent double clicking
        isClicked = true;
        UpdateCounter();
        Destroy(gameObject); // Destroy the sand dollar object
    }

    void UpdateCounter()
    {
        SandDollarManager.instance.AddSandDollars(worth);
    }

    void DespawnOnDelay()
    {
        if (!isClicked)
        {
            Destroy(gameObject);
        }
    }
}
