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
        target = null;
        return;
    }

    // If the target is in range, trigger the attack animation and attack
    Attack();
}


private void SearchForTarget()
{
     RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
}






    private void Attack()
    {
        // Trigger the attack animation
        animator.SetBool("IsAttacking", true);

        // Implement your attack logic here
        Debug.Log("Attacking!");

        // Reset the target after attacking
        target = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f); // Cyan color with 50% transparency
        Gizmos.DrawSphere(transform.position, targetingRange);
    }
    private bool CheckTargetIsInRange(){
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
}
