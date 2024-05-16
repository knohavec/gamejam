using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class TowerShopButtonScript : MonoBehaviour
{
    public GameObject sectionToToggle;

    public void ToggleSectionVisibility()
    {
        if (sectionToToggle != null)
        {
            // Debug.Log("TOGGLE SHOP");
            sectionToToggle.SetActive(!sectionToToggle.activeSelf);
        }
    }
}

