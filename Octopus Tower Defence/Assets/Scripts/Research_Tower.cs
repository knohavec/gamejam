using UnityEngine;

public class Research_Tower_Button_Script : MonoBehaviour
{
    [SerializeField] private GameObject buyButton; // Reference to the buy button UI object

    private void Start()
    {
        PollutiumManager.instance.UpdatePollutiumUI();
    }

    public void ResearchTower()
    {
        Tower selectedTower = BuildManager.main.GetSelectedTower();
        if (selectedTower != null && PollutiumManager.instance.pollutiumAmount >= selectedTower.tower_research_cost)
        {
            PollutiumManager.instance.SubtractPollutium(selectedTower.tower_research_cost);
            PollutiumManager.instance.UpdatePollutiumUI();
            buyButton.SetActive(true); // Show the buy button
        }
    }
}
