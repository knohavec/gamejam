using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHighlighter : MonoBehaviour
{
    public GameObject highlightTilePrefab;
    private GameObject currentHighlightTile;
    private Vector3Int lastHighlightedCell;

    [SerializeField]
    private Vector2Int offset = Vector2Int.zero;

    public Tilemap tilemap; // Reference to the Tilemap component, assignable in the Inspector

    private void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not assigned in the inspector!");
            enabled = false; // Disable the script if the tilemap is not assigned
            return;
        }

        lastHighlightedCell = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue); // Initialize to an invalid position
    }

    private void Update()
{
    Vector3 mousePos = Input.mousePosition;
    mousePos.z = 0; // Set z-coordinate to 0
    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

    // Convert the world position to hex coordinates
    Vector3Int cellPos = tilemap.WorldToCell(worldPos);

    // Apply the offset
    cellPos.x += offset.x;
    cellPos.y += offset.y;

    // Check if the mouse is over a new cell
    if (cellPos != lastHighlightedCell)
    {
        // Remove the current highlight if it exists
        if (currentHighlightTile != null)
        {
            Destroy(currentHighlightTile);
        }

        // Create a new highlight at the current cell position
        Vector3 cellWorldPos = tilemap.GetCellCenterWorld(cellPos);
        cellWorldPos.z = 0; // Set z-coordinate to 0
        currentHighlightTile = Instantiate(highlightTilePrefab, cellWorldPos, Quaternion.identity);

        // Update the last highlighted cell
        lastHighlightedCell = cellPos;
    }
}
}
