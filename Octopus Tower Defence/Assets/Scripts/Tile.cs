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
                    Debug.Log("Starting FlashDamage coroutine");
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
                Debug.Log("Flashing damage color");
            }
            yield return new WaitForSeconds(attackSpeed * 0.5f);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
                Debug.Log("Reverting to original color");
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
                Debug.Log("Stopped FlashDamage coroutine and reset color");
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
                Debug.Log("Stopped FlashDamage coroutine and reset color");
            }
        }

        if (damageStopCoroutine != null)
        {
            StopCoroutine(damageStopCoroutine);
            damageStopCoroutine = null;
        }
    }
}
