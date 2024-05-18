using UnityEngine;
using UnityEngine.Events;

public class Enemy_Stats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int hitPoints = 2;
    [SerializeField] public int Damage = 2;
    
    [SerializeField] public float AttackSpeed = 1f;
    [SerializeField] public float AttackRange = 1f;


    public bool isDestroyed = false;

    public void TakeDamage(int dmg)
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
