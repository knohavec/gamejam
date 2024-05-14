using UnityEngine;
using System.Collections;

public class kelp_attack : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int damage = 5;

    private Transform target;
    private float searchCooldown = 0f;
    private float searchCooldownTime = 0.5f; // Search for a target every 0.5 seconds
    private Animator animator; // Reference to the Animator component
    private float idleTime;
    private float attackTime;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        UpdateAnimClipTimes();
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

    private void UpdateAnimClipTimes()
{
    animator = GetComponent<Animator>();
    if (animator == null)
    {
        Debug.Log("Error: Did not find Animator component!");
        return;
    }

    AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
    foreach (AnimationClip clip in clips)
    {
        switch (clip.name)
        {
            case "kelp_attack":
                attackTime = clip.length;
                break;
            case "kelp_idle":
                idleTime = clip.length;
                break;
        }
    }

    Debug.Log("Attack Time: " + attackTime);
    Debug.Log("Idle Time: " + idleTime);
}


  private IEnumerator Attack()
{
    if (target != null && CheckTargetIsInRange())
    {
        // Trigger the attack animation
        animator.SetBool("IsAttacking", true);

        
        
        
        
        // Wait for the attack animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        target.gameObject.GetComponent<Enemy_Stats>().TakeDamage(damage);

        // Reset the attack animation parameter
        animator.SetBool("IsAttacking", false);

        
        // Reset the target after attacking
        target = null;
    }

    
    yield break;
}





    private void SearchForTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
            // Debug.Log("Target found: " + target.name);
        }
        else
        {
            // Debug.Log("No target found");
        }
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
