using UnityEngine;

public class EnemyLogger : MonoBehaviour
{
    public float checkInterval = 1f; // Time interval to check for enemies
    private float timer;

    private void Start()
    {
        timer = checkInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            LogEnemyPositions();
            timer = checkInterval;
        }
    }

    private void LogEnemyPositions()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            Debug.Log("No enemies found.");
        }
        else
        {
            foreach (GameObject enemy in enemies)
            {
                Vector3 position = enemy.transform.position;
                Debug.Log("Enemy position: " + position);
            }
        }
    }
}
