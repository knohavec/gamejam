using UnityEngine;
using TMPro;

public class SandDollar : MonoBehaviour
{
    public static TMP_Text counterText; // Static reference to the UI ELEMENT FOR COUNTER

    [Header("Attributes")]
    [SerializeField] private int changeAmount = 1;

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
    }

    void OnMouseDown()
    {
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

        

        SandDollarSpawning.sand_dollar_total += changeAmount;
        counterText.text = SandDollarSpawning.sand_dollar_total.ToString();

    }
}
