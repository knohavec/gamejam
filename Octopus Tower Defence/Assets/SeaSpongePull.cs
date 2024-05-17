using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSpongePull : MonoBehaviour
{
    [SerializeField] private float attackRange = 5f; // Adjustable range for detecting sand dollars
    [SerializeField] private float pullSpeed = 2f; // Speed at which sand dollars are pulled towards the sea sponge

    private void Update()
    {
        PullSandDollars();
    }

    private void PullSandDollars()
    {
        // Find all SandDollar objects within the attack range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
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
        // Draw the attack range in the scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
