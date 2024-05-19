using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Tile : MonoBehaviour
{
    public int tilehealth;
    public bool isDestroyed = false;
    public bool hasTower = false;
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent red color to flash when taking damage

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashDamageCoroutine;

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
    }

    public void TakeDamage(int dmg, float attackSpeed)
    {
        if (!isDestroyed)
        {
            Debug.Log("Tile Hit");
            tilehealth -= dmg;

            if (tilehealth <= 0)
            {
                RemoveFromTilemap();
                isDestroyed = true;
                StopFlashDamage();
            }
            else
            {
                if (flashDamageCoroutine == null)
                {
                    Debug.Log("Starting FlashDamage coroutine");
                    flashDamageCoroutine = StartCoroutine(FlashDamage(attackSpeed));
                }
            }
        }
    }

    private IEnumerator FlashDamage(float attackSpeed)
    {
        float flashDuration = attackSpeed * 0.5f; // Adjust the flash duration based on the attack speed
        while (!isDestroyed)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = damageColor;
                Debug.Log("Flashing damage color");
            }
            yield return new WaitForSeconds(flashDuration);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
                Debug.Log("Reverting to original color");
            }
            yield return new WaitForSeconds(attackSpeed - flashDuration);
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

    public void StopAttack()
    {
        StopFlashDamage();
    }

    private void StopFlashDamage()
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
    }
}
