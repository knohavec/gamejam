using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexBuildManager : MonoBehaviour
{
    public static HexBuildManager main; // Static reference to the instance

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

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }

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

    public bool IsInBounds(Vector3Int cellPos)
    {
        // Modify this as needed for your bounds check
        return true;
    }

    public void TryPlaceTile(Vector3Int cellPos)
    {
        if (selectedTile >= 0 && selectedTile < tiles.Length)
        {
            if (selectedTile >= tilePrefabs.Length)
            {
                Debug.LogError("selectedTile index is out of bounds for tilePrefabs array!");
                return;
            }

            GameObject tilePrefab = tilePrefabs[selectedTile];
            Tile selectedTileComponent = tilePrefab.GetComponent<Tile>();

            if (selectedTileComponent == null)
            {
                Debug.LogError("Selected tile prefab does not have a Tile component!");
                return;
            }

            int cost = selectedTileComponent.GetCost();
            Tile.CurrencyType currencyType = selectedTileComponent.GetCurrencyType();

            bool canPlaceTile = false;

            switch (currencyType)
            {
                case Tile.CurrencyType.SandDollars:
                    canPlaceTile = SandDollarManager.instance.SpendSandDollars(cost);
                    break;
                case Tile.CurrencyType.Pollutium:
                    canPlaceTile = PollutiumManager.instance.SpendPollutium(cost);
                    break;
                default:
                    Debug.LogError("Unknown CurrencyType: " + currencyType);
                    return;
            }

            if (canPlaceTile)
            {
                PlaceTile(cellPos);
                ClearSelectedTile();
            }
            else
            {
                Debug.LogWarning("Not enough resources to place tile.");
            }
        }
        else
        {
            Debug.LogError("selectedTile index is out of bounds for tiles array!");
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
            Debug.LogError("Invalid tile index!");
        }
    }

    public GameObject GetTilePrefab(int index)
    {
        if (index >= 0 && index < tilePrefabs.Length)
        {
            return tilePrefabs[index];
        }
        return null;
    }
}
