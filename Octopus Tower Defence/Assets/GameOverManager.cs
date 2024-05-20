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

    private void Update()
    {
        if (object_to_check == null)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        CleanupGameObjects();
        SceneManager.LoadScene("MainMenu");
    }

    private void CleanupGameObjects()
    {
        // Find and destroy dynamically created objects
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.name.Contains("polluted_meat(Clone)"))
            {
                Destroy(obj);
            }
        }
    }
}
