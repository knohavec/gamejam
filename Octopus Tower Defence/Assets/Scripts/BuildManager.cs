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

    public void SelectTower(int _selectedTower)
    {
        Tower tower = towers[_selectedTower];
        bool canSelect = false;

        switch (tower.currencyType)
        {
            case Tower.CurrencyType.SandDollars:
                canSelect = SandDollarManager.instance.HasEnoughSandDollars(tower.tower_research_cost);
                break;
            case Tower.CurrencyType.Pollutium:
                canSelect = PollutiumManager.instance.HasEnoughPollutium(tower.tower_research_cost);
                break;
        }

        if (canSelect)
        {
            selectedTower = _selectedTower;
        }
        else
        {
            Debug.LogWarning("Not enough currency to select this tower.");
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
                Instantiate(towerToBuild.towerprefab, position, Quaternion.identity);
                return true;
            }
            else
            {
                Debug.LogWarning("Not enough currency to place this tower.");
            }
        }
        return false;
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
