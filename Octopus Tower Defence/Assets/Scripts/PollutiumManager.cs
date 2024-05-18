using UnityEngine;
using System;
using TMPro;

public class PollutiumManager : MonoBehaviour
{
    public static PollutiumManager instance;

    public int pollutiumAmount = 0; // Example starting amount of Pollutium
    public TextMeshProUGUI pollutiumText; // Reference to the TextMeshPro text displaying Pollutium amount

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            UpdatePollutiumUI();
            DontDestroyOnLoad(gameObject); // Optional: keep the instance across scenes
        }
        else
        {
            Debug.LogWarning("Another instance of PollutiumManager already exists, destroying this one.");
            Destroy(gameObject);
        }
    }

    public void UpdatePollutiumUI()
    {
        if (pollutiumText != null)
        {
            pollutiumText.text = pollutiumAmount.ToString();
        }
    }

    public void AddPollutium(int amount)
    {
        pollutiumAmount += amount;
        UpdatePollutiumUI();
    }

    public void SubtractPollutium(int amount)
    {
        pollutiumAmount -= amount;
        UpdatePollutiumUI();
    }

    public bool HasEnoughPollutium(int amount)
    {
        return pollutiumAmount >= amount;
    }

    public bool SpendPollutium(int amount)
    {
        if (HasEnoughPollutium(amount))
        {
            Debug.Log("Spending " + amount + " Pollutium");
            pollutiumAmount -= amount;
            UpdatePollutiumUI();
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough Pollutium to spend.");
            return false;
        }
    }
}
