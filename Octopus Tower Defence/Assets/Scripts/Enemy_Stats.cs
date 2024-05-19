using System.Collections;
using UnityEngine;

using System;
using System.Collections.Generic;

public class Enemy_Stats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float hitPoints = 2f;
    [SerializeField] public int Damage = 2;
    [SerializeField] public float AttackSpeed = 1f;
    [SerializeField] public float AttackRange = 1f;
    [SerializeField] public float TargetingRadius = 5f;

    public bool isDestroyed = false;

    public void TakeDamage(float dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isDestroyed)
        {
            FindObjectOfType<SquareSpawningEnemySpawner>().EnemyDestroyed();
            isDestroyed = true;

            // Notify the EnemyAttack script
            EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
            if (enemyAttack != null)
            {
                enemyAttack.EnemyDestroyed();
            }

            Destroy(gameObject);
        }
    }
}
