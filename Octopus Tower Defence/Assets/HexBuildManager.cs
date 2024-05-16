using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexBuildManager : MonoBehaviour
{
    public RuleTile[] tiles;
    public int selectedTile = 0;
    public Tilemap tilemap;

    private Vector3Int[] directionsEven = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 0),
        new Vector3Int(-1, -1, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0)
    };

    private Vector3Int[] directionsOdd = new Vector3Int[]
    {
        new Vector3Int(1, 1, 0), new Vector3Int(1, 0, 0), new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, 0)
    };

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(worldPos);

            if (IsInBounds(cellPos))
            {
                PlaceTile(cellPos);
            }
        }
    }

    private bool IsInBounds(Vector3Int cellPos)
    {
        // Implement bounds checking for a flat top hex grid
        int radius = 5; // Adjust this value to define the playable area radius
        int yMin = -radius;
        int yMax = radius;
        int xMin = -radius;
        int xMax = radius;

        bool isOddColumn = Mathf.Abs(cellPos.y) % 2 == 1;
        if (isOddColumn)
        {
            xMin--;
            xMax--;
        }

        bool inBounds = cellPos.y >= yMin && cellPos.y <= yMax &&
                        cellPos.x >= xMin && cellPos.x <= xMax;

        return inBounds;
    }

    public void PlaceTile(Vector3Int cellPos)
{
    if (selectedTile >= 0 && selectedTile < tiles.Length)
    {
        TileBase tile = tiles[selectedTile];
        tilemap.SetTile(new Vector3Int(cellPos.x, cellPos.y, 0), tile);
    }
}
public void ClearSelectedTile()
    {
        selectedTile = -1; // Set the selectedTile index to 0 to unselect the tile
    }
    public void SelectTile(int tileIndex)
{
    if (tileIndex >= 0 && tileIndex < tiles.Length)
    {
        selectedTile = tileIndex;
    }
    else
    {
        Debug.LogError("Invalid tile index!");
    }
}

    
}
