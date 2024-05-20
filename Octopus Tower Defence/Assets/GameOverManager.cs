using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    public GameObject object_to_check;
    private bool isGameOver = false;

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
        if (object_to_check == null && !isGameOver)
        {
            // Debug.Log("Triggering EndGame from Update");
            EndGame();
        }
    }

    public void EndGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            // Debug.Log("Ending game...");
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // Debug.Log("EndGame was already called");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("Scene loaded: " + scene.name);
        if (scene.name == "MainMenu")
        {
            Time.timeScale = 1f;
            // Debug.Log("Time scale reset to 1");
            // Do not reset isGameOver here. Only reset it when starting a new game.
        }
    }
}
