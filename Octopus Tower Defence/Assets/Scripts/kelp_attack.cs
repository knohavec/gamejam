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

    private void Awake()
    {
        tower = GetComponent<Tower>(); // Get the Tower component
        animator = GetComponent<Animator>(); // Get the Animator component

        if (animator == null)
        {
            Debug.LogError("Animator component is missing.");
            return;
        }

        UpdateAnimClipTimes();
        GetEnemiesInRange();
    }

    private void Update()
    {
        if (tower == null || animator == null)
        {
            Debug.Log("Tower or Animator component is missing.");
            return;
        }

        if (tower.isDestroyed) return; // Do not attack if the tower is destroyed

        List<Transform> targets = GetEnemiesInRange();

        if (targets.Count > 0)
        {
            if (!animator.GetBool("IsAttacking"))
            {
                StartCoroutine(Attack(targets));
            }
        }
        else
        {
            // If there are no enemies in range, set the animator to idle
            animator.SetBool("IsAttacking", false);
        }
    }

    private void UpdateAnimClipTimes()
    {
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, tower.tower_attack_range, LayerMask.GetMask("Enemy"));
        List<Transform> targets = new List<Transform>();

        foreach (var hit in hits)
        {
            targets.Add(hit.transform);
        }

        return targets;
    }

    private IEnumerator Attack(List<Transform> targets)
    {
        Debug.Log("Starting Attack");
        // Trigger the attack animation
        animator.SetBool("IsAttacking", true);

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(tower.tower_attack_speed);

        // Damage all enemies found
        foreach (var target in targets)
        {
            Enemy_Stats enemyStats = target.GetComponent<Enemy_Stats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(tower.towerdamage);
            }
        }

        // Reset the attack animation parameter
        animator.SetBool("IsAttacking", false);
        Debug.Log("Attack Finished");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f); // Cyan color with 50% transparency
        Gizmos.DrawWireSphere(transform.position, tower.tower_attack_range);
    }
}
