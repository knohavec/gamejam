using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    private bool isSceneSwitching = false;

    public void LoadScene(string sceneName)
    {
        if (!isSceneSwitching)
        {
            isSceneSwitching = true;
            // Debug.Log("Attempting to load scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f; // Ensure game speed is normal upon loading new scene
        }
        else
        {
            // Debug.Log("Scene switch is already in progress");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("Scene loaded: " + scene.name);
        isSceneSwitching = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
