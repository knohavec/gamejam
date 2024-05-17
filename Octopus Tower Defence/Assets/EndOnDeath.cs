using UnityEngine;

public class EndGameOnDeath : MonoBehaviour
{
    public void DestroyAndEndGame()
    {
        // Call the game over logic
        GameOverManager.instance.EndGame();
        
        // Destroy this GameObject
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Optionally, add any additional logic here if needed
    }
}
