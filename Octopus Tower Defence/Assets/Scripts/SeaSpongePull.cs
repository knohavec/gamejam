using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSpongePull : MonoBehaviour
{
    [SerializeField] private float pullSpeed = 2f; // Speed at which sand dollars are pulled towards the sea sponge
    [SerializeField] private int increment_amount = 1;
    [SerializeField] private Animator animator;
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
            SandDollarManager.instance.AddSandDollars(increment_amount);

            // Set the animator parameter
            animator.SetBool("IsSucking", true);

            Destroy(sandDollar.gameObject);
            
            // Reset the animator parameter after a delay (if needed)
            StartCoroutine(ResetSuckingAnimation());
        }
    }

    private IEnumerator ResetSuckingAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
        animator.SetBool("IsSucking", false);
    }
}
