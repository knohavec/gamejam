using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Tile : MonoBehaviour
{
    public int tilehealth;
    public bool isDestroyed = false;
    public bool hasTower = false;
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f);
    public float flashDuration = 2.0f; // Duration to stop flashing if no damage is taken
    public float checkTowerInterval = 5.0f; // Interval to check for tower presence

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashDamageCoroutine;
    private Coroutine damageStopCoroutine;
    private Coroutine checkTowerCoroutine;

    public enum CurrencyType
    {
        SandDollars,
        Pollutium
    }

    [SerializeField]
    private CurrencyType currencyType;
    [SerializeField]
    private int cost;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on Tile.");
        }

        Tower.OnTowerDestroyed += HandleTowerDestroyed;
        
        // Start the coroutine to periodically check for tower presence
        checkTowerCoroutine = StartCoroutine(CheckForTowerPresence());
    }

    private void OnDestroy()
    {
        Tower.OnTowerDestroyed -= HandleTowerDestroyed;

        if (checkTowerCoroutine != null)
        {
            StopCoroutine(checkTowerCoroutine);
        }
    }

    private void HandleTowerDestroyed(Tower tower)
    {
        if (tower.transform.parent == transform)
        {
            SetTowerPresence(false);
            Debug.Log("Tile updated: tower destroyed.");
        }
    }

    public CurrencyType GetCurrencyType()
    {
        return currencyType;
    }

    public int GetCost()
    {
        return cost;
    }

    public void SetTowerPresence(bool presence)
    {
        hasTower = presence;
        Debug.Log("Tile hasTower set to: " + presence);
    }

    public void TakeDamage(int dmg, float attackSpeed)
    {
        if (!isDestroyed)
        {
            Debug.Log("Tile Hit");
            tilehealth -= dmg;

            if (tilehealth <= 0)
            {
                isDestroyed = true;
                StopFlashDamage();
                RemoveFromTilemap();
            }
            else
            {
                if (flashDamageCoroutine == null)
                {
                    flashDamageCoroutine = StartCoroutine(FlashDamage(attackSpeed));
                }

                // Reset the stop coroutine to ensure the flashing continues until the delay elapses
                if (damageStopCoroutine != null)
                {
                    StopCoroutine(damageStopCoroutine);
                }
                damageStopCoroutine = StartCoroutine(StopFlashingAfterDelay());
            }
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

    public void RemoveFromTilemap()
    {
        Tilemap tilemap = GetComponentInParent<Tilemap>();
        if (tilemap != null)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
            TileBase tile = tilemap.GetTile(cellPosition);

            if (tile != null)
            {
                tilemap.SetTile(cellPosition, null);
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

    private IEnumerator CheckForTowerPresence()
    {
        while (true)
        {
            // Check for tower presence every checkTowerInterval seconds
            if (transform.childCount > 0)
            {
                SetTowerPresence(true);
            }
            else
            {
                SetTowerPresence(false);
            }

            yield return new WaitForSeconds(checkTowerInterval);
        }
    }
}
