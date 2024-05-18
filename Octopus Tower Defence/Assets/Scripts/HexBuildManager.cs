using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class HexBuildManager : MonoBehaviour
{
    public RuleTile[] tiles;
    public GameObject[] tilePrefabs; // Array of prefabs containing the Tile component
    public int selectedTile = -1; // Initialize with no tile selected
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

    private void Start()
    {
        if (selectedTile < 0 || selectedTile >= tiles.Length)
        {
            selectedTile = -1; // Ensure no tile is selected
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(worldPos);

            if (IsInBounds(cellPos))
            {
                TryPlaceTile(cellPos);
            }
        }
    }

    private bool IsInBounds(Vector3Int cellPos)
    {
        return true; // Modify this as needed for your bounds check
    }

    private void TryPlaceTile(Vector3Int cellPos)
    {
        // Debug.Log("Attempting to place tile at: " + cellPos);

        if (selectedTile >= 0 && selectedTile < tiles.Length)
        {
            if (selectedTile >= tilePrefabs.Length)
            {
                // Debug.LogError("selectedTile index is out of bounds for tilePrefabs array!");
                return;
            }

            GameObject tilePrefab = tilePrefabs[selectedTile];
            Tile selectedTileComponent = tilePrefab.GetComponent<Tile>();

            if (selectedTileComponent == null)
            {
                // Debug.LogError("Selected tile prefab does not have a Tile component!");
                return;
            }

            int cost = selectedTileComponent.GetCost();
            Tile.CurrencyType currencyType = selectedTileComponent.GetCurrencyType();

            // Debug.Log("Tile Cost: " + cost + ", Currency Type: " + currencyType);

            bool canPlaceTile = false;

            switch (currencyType)
            {
                case Tile.CurrencyType.SandDollars:
                    // Debug.Log("Attempting to spend SandDollars");
                    canPlaceTile = SandDollarManager.instance.SpendSandDollars(cost);
                    break;
                case Tile.CurrencyType.Pollutium:
                    // Debug.Log("Attempting to spend Pollutium");
                    canPlaceTile = PollutiumManager.instance.SpendPollutium(cost);
                    break;
                default:
                    // Debug.LogError("Unknown CurrencyType: " + currencyType);
                    return;
            }

            if (canPlaceTile)
            {
                // Debug.Log("Placing tile...");
                PlaceTile(cellPos);
                ClearSelectedTile();
            }
            else
            {
                // Debug.LogWarning("Not enough resources to place tile.");
            }
        }
        else
        {
            // Debug.LogError("selectedTile index is out of bounds for tiles array!");
        }
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
        selectedTile = -1; // Set to no tile selected
    }

    public void SelectTile(int tileIndex)
    {
        if (tileIndex >= 0 && tileIndex < tiles.Length)
        {
            selectedTile = tileIndex;
        }
        else
        {
            // Debug.LogError("Invalid tile index!");
        }
    }
}
