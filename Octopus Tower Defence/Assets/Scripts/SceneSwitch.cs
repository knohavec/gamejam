using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("Attempting to load scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f; // Ensure game speed is normal upon loading new scene
    }
}