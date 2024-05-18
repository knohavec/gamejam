using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    private Enemy_Stats enemyStats;
    private Movement movement;
    public Animator animator;

    private float attackCooldown;
    private float attackTimer;
    private bool canAttack = true;

    private void Start()
    {
        enemyStats = GetComponent<Enemy_Stats>();
        movement = GetComponent<Movement>();
        attackCooldown = 1f / enemyStats.AttackSpeed; // Calculate the initial attack cooldown
        attackTimer = attackCooldown; // Start with the attack cooldown
    }

    private void Update()
    {
        // Check if the enemy is alive
        if (!enemyStats.isDestroyed)
        {
            GameObject target = FindTarget();
            if (target != null)
            {
                movement.targetPosition = target.transform.position;
                if (IsInRange() && canAttack)
                {
                    StartCoroutine(Attack(target));
                    canAttack = false;
                }
                else
                {
                    attackTimer -= Time.deltaTime; // Decrement the attack timer
                    if (attackTimer <= 0)
                    {
                        canAttack = true;
                        attackTimer = attackCooldown; // Reset the attack timer
                        ResetAttackAnimation(); // Reset the attack trigger

                        // Ensure target stops flashing when not being attacked
                        StopAttacking(target);
                    }
                }
            }
            else
            {
                movement.targetPosition = Vector2.zero; // Move towards (0, 0) if no target is found
            }
        }
        else
        {
            // Stop attacking if the enemy is no longer alive
            canAttack = false;

            // Ensure the tile stops flashing when the enemy dies
            GameObject target = movement.FindTarget();
            if (target != null)
            {
                StopAttacking(target);
            }
        }
    }

    private GameObject FindTarget()
    {
        if (movement == null)
        {
            Debug.LogError("Movement component not found.");
            return null;
        }

        return movement.FindTarget();
    }

    private void ResetAttackAnimation()
    {
        animator.ResetTrigger("Attack");
    }

    private bool IsInRange()
    {
        if (movement.targetPosition == Vector2.positiveInfinity)
        {
            return false; // No valid target set
        }

        float distance = Vector2.Distance(transform.position, movement.targetPosition);
        return distance <= enemyStats.AttackRange;
    }

    private IEnumerator Attack(GameObject target)
    {
        // Set the attack trigger
        animator.SetTrigger("Attack");

        // Wait for the attack animation to play
        yield return new WaitForSeconds(attackCooldown);

        if (target != null && target.activeSelf)
        {
            if (target.CompareTag("Tower"))
            {
                Tower towerComponent = target.GetComponent<Tower>();
                if (towerComponent != null)
                {
                    towerComponent.TakeDamage(enemyStats.Damage, attackCooldown);
                }
            }
            else if (target.CompareTag("Tile"))
            {
                Tile tileComponent = target.GetComponent<Tile>();
                if (tileComponent != null && !tileComponent.isDestroyed)
                {
                    if (tileComponent.hasTower)
                    {
                        // Attack the tower on the tile
                        Collider2D[] colliders = Physics2D.OverlapPointAll(tileComponent.transform.position);
                        foreach (var collider in colliders)
                        {
                            if (collider.CompareTag("Tower"))
                            {
                                Tower tower = collider.GetComponent<Tower>();
                                if (tower != null)
                                {
                                    tower.TakeDamage(enemyStats.Damage, attackCooldown);
                                    yield break;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Attack the tile itself
                        tileComponent.TakeDamage(enemyStats.Damage, attackCooldown);
                    }
                }
            }
        }
        else
        {
            Debug.Log("No valid target found");
        }
    }

    private void StopAttacking(GameObject target)
    {
        if (target != null)
        {
            if (target.CompareTag("Tower"))
            {
                Tower towerComponent = target.GetComponent<Tower>();
                if (towerComponent != null)
                {
                    towerComponent.StopAttack();
                }
            }
            else if (target.CompareTag("Tile"))
            {
                Tile tileComponent = target.GetComponent<Tile>();
                if (tileComponent != null)
                {
                    tileComponent.StopAttack();
                }
            }
        }
    }

    public void EnemyDestroyed()
    {
        GameObject target = movement.FindTarget();
        if (target != null)
        {
            StopAttacking(target);
        }
        StopAttacking(gameObject);
    }
}
