using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector2 targetPosition; // The position of the target object

    public float speed = 5f; // Speed at which the enemy moves

    void Update()
    {
        FindNearestTile();
        // Move the enemy towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Flip the sprite on the y-axis if the target is to the left
        FlipSprite(targetPosition);
        
    }

    // Flip the sprite based on the target position
    // Flip the sprite based on the target position
private void FlipSprite(Vector2 target)
{
    Vector2 direction = (target - (Vector2)transform.position).normalized;

    // Flip the sprite on the y-axis if the target is to the right
    if (direction.x > 0f)
    {
        transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
    }
    else
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }
}


    // Find the nearest object with the "Tile" layer tag and set it as the target position
    private void FindNearestTile()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject tile in tiles)
        {
            float distance = Vector2.Distance(transform.position, tile.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                targetPosition = tile.transform.position;
            }
        }
    }

    void Start()
    {
         // Find the nearest tile when the enemy is spawned
    }
}
