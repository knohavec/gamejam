using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private bool isPaused = false;
    private Button pauseButton;

    void Start()
    {
        pauseButton = GetComponent<Button>();
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f; // Resume the game
        }
        else
        {
            Time.timeScale = 0f; // Pause the game
        }

        isPaused = !isPaused;
    }
}
