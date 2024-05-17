using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections; // Add this using directive

public class Tile : MonoBehaviour
{
    
    public string tilename;
    public int tilecost;
    public int tilehealth;
    public GameObject prefab;
    public Tilemap tilemap;

    public bool isDestroyed = false;
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent red
 // Color to flash when taking damage
    public float flashDuration = 0.1f; // Duration of the flash

    private SpriteRenderer spriteRenderer;
    private Color originalColor; // Store the original color of the sprite

    private void Start()
    {
        tilemap = GetComponentInParent<Tilemap>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found in parent GameObjects.");
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on child GameObjects.");
        }

        // Store the original color of the sprite
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log("Tile Hit");
        tilehealth -= dmg;

        if (tilehealth <= 0)
        {
            RemoveFromTilemap();
            Destroy(gameObject);
            isDestroyed = true;
        }
        else
        {
            StartCoroutine(FlashDamage());
        }
    }

    private IEnumerator FlashDamage()
    {
        // Check if the SpriteRenderer component is present
        if (spriteRenderer != null)
        {
            // Flash the sprite with the damage color
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            // Reset to original color
            spriteRenderer.color = originalColor;
        }
        else
        {
            Debug.LogError("SpriteRenderer component is missing.");
        }
    }

    public void RemoveFromTilemap()
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile != null)
        {
            tilemap.SetTile(cellPosition, null);
        }
    }
}
