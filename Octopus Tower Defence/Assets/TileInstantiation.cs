using UnityEngine;
using UnityEngine.Tilemaps;

public class HexTileInstantiation : MonoBehaviour
{
    [Header("Tile Prefab")]
    [SerializeField] private GameObject tilePrefab;
    [Header("Rule Tile")]
    [SerializeField] private RuleTile ruleTile; // RuleTile to place on the Tilemap

    private Tilemap tilemap;
    private Vector3 cellSize = new Vector3(1.253f, 2.56f, 1);

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found on this GameObject.");
            return;
        }

        if (tilePrefab == null)
        {
            Debug.LogError("Tile Prefab is not assigned.");
            return;
        }

        if (ruleTile == null)
        {
            Debug.LogError("RuleTile is not assigned.");
            return;
        }

        // Create the initial tile at (0, 0)
        InstantiateTileAt(new Vector3Int(0, 0, 0));

        // Create tiles in adjacent positions for a flat-top hex grid
        Vector3Int[] adjacentPositions = {
            new Vector3Int(1, 0, 0),   // Right
            new Vector3Int(-1, 0, 0),  // Left
            new Vector3Int(0, 1, 0),   // Top-Right
            new Vector3Int(-1, 1, 0),  // Top-Left
            new Vector3Int(0, -1, 0),  // Bottom-Right
            new Vector3Int(1, -1, 0)   // Bottom-Left
        };

        foreach (var position in adjacentPositions)
        {
            InstantiateTileAt(position);
        }
    }

    private void InstantiateTileAt(Vector3Int cellPosition)
    {
        // Adjust the position based on cell size and YXZ swizzle
        Vector3 worldPosition = HexCellToWorld(cellPosition);
        GameObject tileInstance = Instantiate(tilePrefab, worldPosition, Quaternion.identity);
        tileInstance.transform.SetParent(transform);

        // Place the rule tile on the Tilemap
        tilemap.SetTile(cellPosition, ruleTile);

        Debug.Log("Tile instantiated at " + cellPosition);
    }

    private Vector3 HexCellToWorld(Vector3Int cellPosition)
    {
        float x = cellSize.x * (cellPosition.x + 0.5f * cellPosition.y);
        float y = cellSize.z * cellPosition.z;
        float z = cellSize.y * (3.0f / 4.0f * cellPosition.y);

        return new Vector3(x, y, z);
    }
}
