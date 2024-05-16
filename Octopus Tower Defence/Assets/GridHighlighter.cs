using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHighlighter : MonoBehaviour
{
    public Tilemap highlightTilemap; // Assign the highlight Tilemap in the inspector
    public Tile highlightTile;
    private Vector3Int lastHighlightedCell;

    private void Start()
    {
        lastHighlightedCell = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue); // Initialize to an invalid position
    }

    private void Update()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = highlightTilemap.WorldToCell(worldPos);

        // Check if the cell under the mouse has changed
        if (cellPos != lastHighlightedCell)
        {
            ClearHighlightTile(lastHighlightedCell); // Clear the previous highlight
            HighlightTile(cellPos); // Highlight the new cell
            lastHighlightedCell = cellPos; // Update the last highlighted cell
        }
    }

    private void HighlightTile(Vector3Int cellPos)
    {
        if (IsCellEmpty(cellPos))
        {
            highlightTilemap.SetTile(cellPos, highlightTile);
        }
    }

    private void ClearHighlightTile(Vector3Int cellPos)
    {
        highlightTilemap.SetTile(cellPos, null);
    }

    private bool IsCellEmpty(Vector3Int cellPos)
    {
        TileBase tile = highlightTilemap.GetTile(cellPos);
        return tile == null;
    }
}
