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

    private float attackSpeed;

    private void Start()
    {
        enemyStats = GetComponent<Enemy_Stats>();
        movement = GetComponent<Movement>();
        attackCooldown = 1f / enemyStats.AttackSpeed; // Calculate the initial attack cooldown
        attackTimer = attackCooldown; // Start with the attack cooldown
        attackSpeed = enemyStats.AttackSpeed;
    }

    private void Update()
{
    GameObject target = FindTarget();
    if (target != null)
    {
        movement.targetPosition = target.transform.position;
        if (IsInRange() && canAttack)
        {
            StartCoroutine(Attack());
            canAttack = false;
            attackTimer = attackCooldown; // Reset the attack timer
        }
        else
        {
            attackTimer -= Time.deltaTime; // Decrement the attack timer
            if (attackTimer <= 0)
            {
                canAttack = true;
                ResetAttackAnimation(); // Reset the attack trigger
            }
        }
    }
    else
    {
        Debug.Log("No valid target found.");
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
    bool inRange = distance <= enemyStats.AttackRange;
    if (!inRange)
    {
        // Debug.Log($"Target out of range. Distance: {distance}, Attack Range: {enemyStats.AttackRange}");
    }
    return inRange;
}

    private IEnumerator Attack()
{
    // Debug.Log("Attacking");

     // Get the target object (tower or tile)
    GameObject targetObject = movement.FindTarget();

    // Set the attack trigger
    animator.SetTrigger("Attack");

    yield return new WaitForSeconds(attackSpeed);


   

    if (targetObject != null && targetObject.activeSelf)
    {
        // Check if the target object is destroyed
        bool targetIsDestroyed = false;
        if (targetObject.CompareTag("Tower"))
        {
            Tower towerComponent = targetObject.GetComponent<Tower>();
            if (towerComponent != null && towerComponent.isDestroyed)
            {
                targetIsDestroyed = true;
            }
        }
        else if (targetObject.CompareTag("Tile"))
        {
            Tile tileComponent = targetObject.GetComponent<Tile>();
            if (tileComponent != null && tileComponent.isDestroyed)
            {
                targetIsDestroyed = true;
            }
        }

        // If the target is destroyed, find a new target
        if (targetIsDestroyed)
        {
            targetObject = movement.FindTarget();
            if (targetObject != null && targetObject.activeSelf)
            {
                // Debug.Log($"New target: {targetObject.name}");
            }
            else
            {
                // Debug.Log("No valid target found");
                yield break;
            }
        }

        // Debug.Log($"Target: {targetObject.name}");

        if (targetObject.CompareTag("Tower"))
        {
            Tower towerComponent = targetObject.GetComponent<Tower>();
            if (towerComponent != null)
            {
                towerComponent.TakeDamage(enemyStats.Damage);
                // Debug.Log($"Dealt {enemyStats.Damage} damage to {targetObject.name}");
            }
        }
        else if (targetObject.CompareTag("Tile"))
        {
            Tile tileComponent = targetObject.GetComponent<Tile>();
            if (tileComponent != null && !tileComponent.isDestroyed)
            {
                tileComponent.TakeDamage(enemyStats.Damage);
                // Debug.Log($"Dealt {enemyStats.Damage} damage to {targetObject.name}");
            }
        }
    }
    else
    {
        Debug.Log("No valid target found");
    }
}



}
