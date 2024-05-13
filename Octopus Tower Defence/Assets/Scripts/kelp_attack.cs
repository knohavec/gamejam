using UnityEngine;

public class kelp_attack : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float attackSpeed = 1f;

    private Transform target;
    private float searchCooldown = 0f;
    private float searchCooldownTime = 0.5f; // Search for a target every 0.5 seconds
    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        if (target == null)
        {
            SearchForTarget();
            return;
        }

        if (!CheckTargetIsInRange())
        {
            target = null; // Reset the target if it's no longer in range
            return;
        }

        // If the target is in range, trigger the attack animation and attack
        StartCoroutine(Attack());
    }

    private void SearchForTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
            Debug.Log("Target found: " + target.name);
        }
        else
        {
            Debug.Log("No target found");
        }
    }


    
    private System.Collections.IEnumerator Attack()
{
    Debug.Log("Attacking");

    // Trigger the attack animation
    animator.SetBool("IsAttacking", true);

    while (target != null && CheckTargetIsInRange())
    {
        // Wait for a short time based on attack speed
        yield return new WaitForSeconds(1f / attackSpeed);
    }

    // Reset the attack animation parameter
    animator.SetBool("IsAttacking", false);
    Debug.Log("Done Attacking");

    // Reset the target after attacking
    target = null;
}



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f); // Cyan color with 50% transparency
        Gizmos.DrawSphere(transform.position, targetingRange);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
}
