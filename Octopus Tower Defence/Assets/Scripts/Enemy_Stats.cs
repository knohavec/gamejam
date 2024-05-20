using System.Collections;
using UnityEngine;

public class Enemy_Stats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float hitPoints = 2f;
    [SerializeField] public int Damage = 2;
    [SerializeField] public float AttackSpeed = 1f;
    [SerializeField] public float AttackRange = 1f;
    [SerializeField] public float TargetingRadius = 5f;

    public bool isDestroyed = false;
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f);
    public float flashDuration = 2.0f; // Duration to stop flashing if no damage is taken

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashDamageCoroutine;
    private Coroutine damageStopCoroutine;


    
    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on Enemy.");
        }
    }

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
        else if (!isDestroyed)
        {
            if (flashDamageCoroutine == null)
            {
                flashDamageCoroutine = StartCoroutine(FlashDamage(AttackSpeed));
            }

            // Reset the stop coroutine to ensure the flashing continues until the delay elapses
            if (damageStopCoroutine != null)
            {
                StopCoroutine(damageStopCoroutine);
            }
            damageStopCoroutine = StartCoroutine(StopFlashingAfterDelay());
        }
    }

    private IEnumerator FlashDamage(float attackSpeed)
    {
        while (!isDestroyed)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = damageColor;
            }
            yield return new WaitForSeconds(attackSpeed * 0.5f);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
            yield return new WaitForSeconds(attackSpeed * 0.5f);
        }
    }

    private IEnumerator StopFlashingAfterDelay()
    {
        yield return new WaitForSeconds(flashDuration);

        if (flashDamageCoroutine != null)
        {
            StopCoroutine(flashDamageCoroutine);
            flashDamageCoroutine = null;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    public void StopFlashDamage()
    {
        if (flashDamageCoroutine != null)
        {
            StopCoroutine(flashDamageCoroutine);
            flashDamageCoroutine = null;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }

        if (damageStopCoroutine != null)
        {
            StopCoroutine(damageStopCoroutine);
            damageStopCoroutine = null;
        }
    }
}
