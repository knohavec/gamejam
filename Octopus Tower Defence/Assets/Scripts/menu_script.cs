using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class menu_script : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;

    private void OnGUI()
    {
        // currencyUI.text = LevelManager.main.sand_dollar_total.ToString();
    }

    public void SetSelected(){
        
    }
}
