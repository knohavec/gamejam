using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class kelp_attack : MonoBehaviour
{
    [Header("Attributes")]
    private Tower tower; // Reference to the Tower component
    private Animator animator; // Reference to the Animator component
    private float idleTime;
    private float attackTime;

    private void Start()
    {
        tower = GetComponent<Tower>(); // Get the Tower component
        animator = GetComponent<Animator>(); // Get the Animator component
        UpdateAnimClipTimes();
    }

    private void Update()
    {
        if (tower.isDestroyed) return; // Do not attack if the tower is destroyed

        List<Transform> targets = GetEnemiesInRange();

        if (targets.Count > 0)
        {
            // If there are enemies in range, trigger the attack animation and attack
            StartCoroutine(Attack(targets));
        }
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
    }

    private List<Transform> GetEnemiesInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, tower.tower_attack_range, LayerMask.GetMask("Enemies"));
        List<Transform> targets = new List<Transform>();

        foreach (var hit in hits)
        {
            targets.Add(hit.transform);
        }

        return targets;
    }

    private IEnumerator Attack(List<Transform> targets)
    {
        // Trigger the attack animation
        animator.SetBool("IsAttacking", true);

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(attackTime);

        foreach (var target in targets)
        {
            target.GetComponent<Enemy_Stats>().TakeDamage(tower.towerdamage);
        }

        // Reset the attack animation parameter
        animator.SetBool("IsAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f); // Cyan color with 50% transparency
        Gizmos.DrawWireSphere(transform.position, tower.tower_attack_range);
    }
}
