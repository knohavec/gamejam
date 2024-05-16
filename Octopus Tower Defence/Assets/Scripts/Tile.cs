using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public string tilename;
    public int tilecost;
    public int tilehealth;
    public GameObject prefab;
    public Tilemap tilemap;

    public bool isDestroyed = false;

    private void Start()
    {
        tilemap = GetComponentInParent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found in parent GameObjects.");
        }
    }

    public void TakeDamage(int dmg)
    {
        tilehealth -= dmg;

        if (tilehealth <= 0)
        {
            RemoveFromTilemap();
            Destroy(gameObject);
            isDestroyed = true;
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
