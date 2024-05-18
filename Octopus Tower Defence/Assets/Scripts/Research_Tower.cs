using UnityEngine;

public class Research_Tower_Button_Script : MonoBehaviour
{
    [SerializeField] private GameObject buyButton; // Reference to the buy button UI object
    [SerializeField] private int researchIndex = 0; // Serialized field to specify the index in the inspector
    [SerializeField] private bool isTile = false; // Serialized field to specify if it is a tile or tower

    private void Start()
    {
        PollutiumManager.instance.UpdatePollutiumUI();
    }

    public void Research()
    {
        int researchCost = 0;

        if (isTile)
        {
            // Assuming HexBuildManager.main.GetTilePrefab() returns the GameObject of the tile
            GameObject tilePrefab = HexBuildManager.main.GetTilePrefab(researchIndex);
            if (tilePrefab != null)
            {
                Tile tileComponent = tilePrefab.GetComponent<Tile>();
                if (tileComponent != null)
                {
                    researchCost = tileComponent.GetCost();
                }
                else
                {
                    Debug.LogError("Tile component not found!");
                    return;
                }
            }
            else
            {
                Debug.LogError("Tile prefab not found!");
                return;
            }
        }
        else
        {
            Tower tower = BuildManager.main.GetTower(researchIndex);
            if (tower != null)
            {
                researchCost = tower.tower_research_cost;
            }
            else
            {
                Debug.LogError("Tower not found!");
                return;
            }
        }

        // Log the retrieved research cost
        Debug.Log("Research cost: " + researchCost);

        // Proceed with research using Pollutium only
        bool canResearch = PollutiumManager.instance.SpendPollutium(researchCost);

        if (canResearch)
        {
            buyButton.SetActive(true); // Show the buy button
            gameObject.SetActive(false); // Hide this game object

            Debug.Log("Research successful.");
        }
        else
        {
            Debug.LogWarning("Not enough resources to research.");
        }
    }
}
