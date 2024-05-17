using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSpongePull : MonoBehaviour
{
    [SerializeField] private float pullSpeed = 2f; // Speed at which sand dollars are pulled towards the sea sponge
    public Tower tower; // Reference to the Tower instance
    private float targetingRange;

    private void Start()
    {
        if (tower != null)
        {
            targetingRange = tower.tower_attack_range;
        }
        else
        {
            Debug.LogError("Tower reference is not set in SeaSpongePull.");
        }
    }

    private void Update()
    {
        if (tower != null && !tower.isDestroyed)
        {
            PullSandDollars();
        }
    }

    private void PullSandDollars()
    {
        // Find all SandDollar objects within the attack range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, targetingRange);
        foreach (var hitCollider in hitColliders)
        {
            SandDollar sandDollar = hitCollider.GetComponent<SandDollar>();
            if (sandDollar != null)
            {
                // Move the sand dollar towards the sea sponge
                Vector3 direction = (transform.position - sandDollar.transform.position).normalized;
                sandDollar.transform.position += direction * pullSpeed * Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
{
    if (transform == null)
    {
        Debug.LogError("Transform is null!");
        return;
    }

    // Assuming targetingRange is a field in your class
    if (targetingRange <= 0)
    {
        Debug.LogError("Invalid targeting range!");
        return;
    }

    // Draw the attack range in the scene view for debugging
    Gizmos.color = new Color(0, 1, 1, 0.5f); // Cyan color with 50% transparency
    Gizmos.DrawSphere(transform.position, targetingRange);
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        SandDollar sandDollar = other.GetComponent<SandDollar>();
        if (sandDollar != null)
        {
            // Update the sand dollar counter and destroy the sand dollar
            SandDollarSpawning.sand_dollar_total += sandDollar.worth;
            if (SandDollar.counterText != null)
            {
                SandDollar.counterText.text = SandDollarSpawning.sand_dollar_total.ToString();
            }
            Destroy(sandDollar.gameObject);
        }
    }
}
