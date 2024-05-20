using UnityEngine;
using UnityEngine.UI;

public class UIButtonToggle : MonoBehaviour
{
    public GameObject uiElementToToggle;

    // Add this method to the button's OnClick() event in the Inspector
    public void ToggleUIElement()
    {
        if (uiElementToToggle != null)
        {
            uiElementToToggle.SetActive(!uiElementToToggle.activeSelf);
        }
        else
        {
            Debug.LogWarning("UI element to toggle is not assigned.");
        }
    }
}
