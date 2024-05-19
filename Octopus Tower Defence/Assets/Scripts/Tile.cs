using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public int tilehealth;
    public bool isDestroyed = false;
    public bool hasTower = false;
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f);
    public float flashDuration = 2.0f; // Duration to stop flashing if no damage is taken

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashDamageCoroutine;
    private Coroutine damageStopCoroutine;

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
    }

    private void Update()
    {
        CheckForTowerPresence();
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

    public void CheckForTowerPresence()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        bool towerFound = false;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Tower"))
            {
                towerFound = true;
                break;
            }
        }

        SetTowerPresence(towerFound);
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
                SetTowerPresence(false); // Reset tower presence when the tile is removed
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
