using UnityEngine;
using TMPro;

public class SandDollar : MonoBehaviour
{
    public static TMP_Text counterText; // Static reference to the UI ELEMENT FOR COUNTER

    [Header("Attributes")]
    [SerializeField] public int worth = 1;
    [SerializeField] private float despawnDelay = 5f; // Delay before despawning if not clicked

    private bool isClicked = false;

    void Start()
    {
        if (counterText == null)
        {
            counterText = GameObject.Find("SandDollar_Counter").GetComponent<TMP_Text>();
            if (counterText == null)
            {
                Debug.LogError("No TMP_Text component found in the scene.");
            }
        }

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
        if (counterText == null)
        {
            Debug.LogError("CounterText reference not set!");
            return;
        }

        SandDollarSpawning.sand_dollar_total += worth;
        counterText.text = SandDollarSpawning.sand_dollar_total.ToString();
    }

    void DespawnOnDelay()
    {
        if (!isClicked)
        {
            // Debug.Log("Despawning sand dollar due to timeout.");
            Destroy(gameObject);
        }
    }
}
