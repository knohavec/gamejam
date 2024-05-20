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
            Debug.Log("SpriteRenderer found, original color: " + originalColor);
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on Enemy.");
        }
    }

    public void TakeDamage(float dmg)
    {
        Debug.Log("TakeDamage called with dmg: " + dmg);
        hitPoints -= dmg;
        Debug.Log("Enemy took damage: " + dmg + ", remaining hitPoints: " + hitPoints);

        if (hitPoints <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            // Debug.Log("Enemy is destroyed");
            FindObjectOfType<SquareSpawningEnemySpawner>().EnemyDestroyed();

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
                flashDamageCoroutine = StartCoroutine(FlashDamage());
                // Debug.Log("Started FlashDamage Coroutine");
            }

            // Reset the stop coroutine to ensure the flashing continues until the delay elapses
            if (damageStopCoroutine != null)
            {
                StopCoroutine(damageStopCoroutine);
                // Debug.Log("Stopped existing StopFlashingAfterDelay Coroutine");
            }
            damageStopCoroutine = StartCoroutine(StopFlashingAfterDelay());
            // Debug.Log("Started StopFlashingAfterDelay Coroutine");
        }
    }

    private IEnumerator FlashDamage()
    {
        while (!isDestroyed)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = damageColor;
                // Debug.Log("Changed color to damageColor");
            }
            yield return new WaitForSeconds(0.1f); // Use a fixed short duration for flashing

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
                // Debug.Log("Reverted color to originalColor");
            }
            yield return new WaitForSeconds(0.1f); // Use a fixed short duration for flashing
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
                // Debug.Log("Stopped FlashDamage Coroutine and reset color");
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
                // Debug.Log("Stopped FlashDamage Coroutine in StopFlashDamage");
            }
        }

        if (damageStopCoroutine != null)
        {
            StopCoroutine(damageStopCoroutine);
            damageStopCoroutine = null;
            // Debug.Log("Stopped StopFlashingAfterDelay Coroutine in StopFlashDamage");
        }
    }
}
