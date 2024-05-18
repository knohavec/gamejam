using UnityEngine;
using TMPro;

public class SandDollarManager : MonoBehaviour
{
    public static SandDollarManager instance;

    public int sandDollarTotal = 0; // Starting amount of Sand Dollars
    public TextMeshProUGUI sandDollarText; // Reference to the TextMeshPro text displaying Sand Dollar amount

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        UpdateSandDollarUI();
    }

    public void UpdateSandDollarUI()
    {
        if (sandDollarText != null)
        {
            sandDollarText.text = sandDollarTotal.ToString();
        }
    }

    public void AddSandDollars(int amount)
    {
        sandDollarTotal += amount;
        UpdateSandDollarUI();
    }

    public void SubtractSandDollars(int amount)
    {
        sandDollarTotal -= amount;
        UpdateSandDollarUI();
    }

    public bool HasEnoughSandDollars(int amount)
    {
        return sandDollarTotal >= amount;
    }

    public bool SpendSandDollars(int amount)
{
    if (HasEnoughSandDollars(amount))
    {
        Debug.Log("Spending " + amount + " SandDollars");
        SubtractSandDollars(amount);
        return true;
    }
    else
    {
        Debug.LogWarning("Not enough Sand Dollars to spend.");
        return false;
    }
}

}
