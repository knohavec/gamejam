using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Tile : MonoBehaviour
{
    public int tilehealth;
    public bool isDestroyed = false;
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent red color to flash when taking damage
    public float flashDuration = 0.1f; // Duration of the flash

    private SpriteRenderer spriteRenderer;
    private Color originalColor; // Store the original color of the sprite
    private Coroutine flashDamageCoroutine;

    public enum CurrencyType
    {
        SandDollars,
        Pollutium
    }

    [SerializeField]
    private CurrencyType currencyType; // Add this line to make it appear in the Inspector
    [SerializeField]
    private int cost; // Cost to place this tile

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Store the original color of the sprite
    }

    public CurrencyType GetCurrencyType()
    {
        return currencyType;
    }

    public int GetCost()
    {
        return cost;
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
                StopFlashDamage(); // Stop flashing when destroyed
            }
            else
            {
                if (flashDamageCoroutine == null)
                {
                    flashDamageCoroutine = StartCoroutine(FlashDamage(attackSpeed));
                }
            }
        }
    }

    private IEnumerator FlashDamage(float attackSpeed)
    {
        while (!isDestroyed)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
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

    public void StopFlashDamage()
    {
        if (flashDamageCoroutine != null)
        {
            StopCoroutine(flashDamageCoroutine);
            flashDamageCoroutine = null;
            spriteRenderer.color = originalColor; // Reset the color
        }
    }
}
