using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    public GameObject object_to_check;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (object_to_check == null)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        Debug.Log("Ending game...");
        SceneManager.LoadScene("MainMenu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Time.timeScale = 1f;
            Debug.Log("Time scale reset to 1");
        }
    }
}
