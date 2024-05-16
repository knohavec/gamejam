using UnityEngine;
using UnityEngine.Tilemaps;

public class HexagonPlacement : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase highlightedHexagonTile;
    public GameObject sandTilePrefab;

    private Vector3Int previousCellPosition;

    void Start()
    {
        if (tilemap != null && highlightedHexagonTile != null && sandTilePrefab != null)
        {
            ClearHighlightedTiles();
        }
        else
        {
            Debug.LogError("Tilemap, highlighted hexagon tile, or sand tile prefab not assigned!");
        }
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

        if (cellPosition != previousCellPosition)
        {
            ClearHighlightedTiles();
            TileBase clickedTile = tilemap.GetTile(cellPosition);
            if (clickedTile == tilemap.GetTile(cellPosition) && clickedTile == tilemap.GetTile(cellPosition))
            {
                tilemap.SetTile(cellPosition, highlightedHexagonTile);
            }
            previousCellPosition = cellPosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TileBase clickedTile = tilemap.GetTile(cellPosition);
            if (clickedTile == highlightedHexagonTile)
            {
                Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellPosition);
                Instantiate(sandTilePrefab, cellCenterWorld, Quaternion.identity);
            }
        }
    }

    private void ClearHighlightedTiles()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile == highlightedHexagonTile)
            {
                tilemap.SetTile(pos, null);
            }
        }
    }
}
