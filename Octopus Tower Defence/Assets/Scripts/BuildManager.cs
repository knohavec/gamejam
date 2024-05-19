using System.Collections;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    [SerializeField] private int selectedTower = -1;

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
    }

    public Tower GetSelectedTower()
    {
        if (selectedTower >= 0 && selectedTower < towers.Length)
        {
            return towers[selectedTower];
        }
        return null;
    }

    public Tower GetTower(int index)
    {
        if (index >= 0 && index < towers.Length)
        {
            return towers[index];
        }
        return null;
    }

    public void SelectTower(int _selectedTower)
    {
        if (_selectedTower >= 0 && _selectedTower < towers.Length)
        {
            selectedTower = _selectedTower;
            Debug.Log("Selected tower: " + towers[selectedTower].towername);
        }
        else
        {
            Debug.LogWarning("Invalid tower selection.");
        }
    }

    public void ClearSelectedTower()
    {
        selectedTower = -1;
    }

    public bool TryPlaceTower(Vector3 position)
    {
        Tower towerToBuild = GetSelectedTower();
        if (towerToBuild != null)
        {
            bool hasCurrency = false;
            switch (towerToBuild.currencyType)
            {
                case Tower.CurrencyType.SandDollars:
                    hasCurrency = SandDollarManager.instance.SpendSandDollars(towerToBuild.towercost);
                    break;
                case Tower.CurrencyType.Pollutium:
                    hasCurrency = PollutiumManager.instance.SpendPollutium(towerToBuild.towercost);
                    break;
            }

            if (hasCurrency)
            {
                GameObject towerObject = Instantiate(towerToBuild.towerprefab, position, Quaternion.identity);
                Tower towerInstance = towerObject.GetComponent<Tower>();

                // Find the Tile GameObject at the position and set hasTower to true
                Collider2D hitCollider = Physics2D.OverlapPoint(position);
                if (hitCollider != null)
                {
                    Tile tile = hitCollider.GetComponent<Tile>();
                    if (tile != null)
                    {
                        Debug.Log("Tile found. Setting hasTower to true.");
                        tile.SetTowerPresence(true);
                        towerInstance.Initialize(tile); // Set the parent tile
                    }
                    else
                    {
                        Debug.LogWarning("No Tile component found at the given position.");
                    }
                }
                else
                {
                    Debug.LogWarning("No Collider2D found at the given position.");
                }

                return true;
            }
            else
            {
                Debug.LogWarning("Not enough currency to place this tower.");
            }
        }
        return false;
    }
}
