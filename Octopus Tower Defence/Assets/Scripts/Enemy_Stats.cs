using UnityEngine;
using UnityEngine.Events;

public class Enemy_Stats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int damage = 2;

    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isDestroyed)
        {
            FindObjectOfType<SquareSpawningEnemySpawner>().EnemyDestroyed();
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
