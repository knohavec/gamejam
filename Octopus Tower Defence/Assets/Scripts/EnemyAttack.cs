using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Enemy_Stats enemyStats;
    private Movement movement;
    public Animator animator;

    private float attackCooldown;
    private float attackTimer;
    private bool canAttack = true;
    private GameObject currentTarget;

    private void Start()
    {
        enemyStats = GetComponent<Enemy_Stats>();
        movement = GetComponent<Movement>();
        attackCooldown = 1f / enemyStats.AttackSpeed;
        attackTimer = attackCooldown;
    }

    private void Update()
    {
        if (!enemyStats.isDestroyed)
        {
            GameObject target = FindTarget();
            if (target != currentTarget)
            {
                if (currentTarget != null)
                {
                    StopAttacking(currentTarget);
                }
                currentTarget = target;
            }

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
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0)
                    {
                        canAttack = true;
                        attackTimer = attackCooldown;
                        ResetAttackAnimation();
                        StopAttacking(target);
                    }
                }
            }
            else
            {
                movement.targetPosition = Vector2.zero;
            }
        }
        else
        {
            canAttack = false;
            if (currentTarget != null)
            {
                StopAttacking(currentTarget);
                currentTarget = null;
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
            return false;
        }

        float distance = Vector2.Distance(transform.position, movement.targetPosition);
        return distance <= enemyStats.AttackRange;
    }

    private IEnumerator Attack(GameObject target)
    {
        animator.SetTrigger("Attack");
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
                    tileComponent.StopFlashDamage();
                }
            }
        }
    }

    public void EnemyDestroyed()
    {
        if (currentTarget != null)
        {
            StopAttacking(currentTarget);
            currentTarget = null;
        }
    }
}
