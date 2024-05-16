using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector2 targetPosition;
    public float speed = 5f;

    void Update()
    {
        UpdateTargetPosition();

        if (targetPosition != Vector2.positiveInfinity)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = direction * speed;
            RotateSprite(direction);
        }
    }

    private void UpdateTargetPosition()
    {
        if (targetPosition == Vector2.positiveInfinity || !TileExists(targetPosition))
        {
            Vector3? nearestTilePosition = FindNearestTile()?.transform.position;
            if (nearestTilePosition != null)
            {
                targetPosition = (Vector2)nearestTilePosition;
            }
        }

        // Check if there is a clear path to the target position
        RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition);
        if (hit.collider != null && hit.collider.CompareTag("Tile"))
        {
            // If there is an obstacle in the way, find a new target
            targetPosition = Vector2.positiveInfinity;
        }
    }

    private bool TileExists(Vector2 position)
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles)
        {
            if ((Vector2)tile.transform.position == position)
            {
                return true;
            }
        }
        return false;
    }

    private void RotateSprite(Vector2 direction)
{
    // Calculate the angle to rotate the sprite towards the target
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    // Flip the sprite on the x-axis
    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    //Flip on Y axis
    if (direction.y > 0)
    {
        transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }
    else
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }
}

    private GameObject FindNearestTile()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        float nearestDistance = Mathf.Infinity;
        GameObject nearestTile = null;
        foreach (GameObject tile in tiles)
        {
            float distance = Vector2.Distance(transform.position, tile.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTile = tile;
            }
        }
        return nearestTile;
    }

    private GameObject FindNearestTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        float nearestDistance = Mathf.Infinity;
        GameObject nearestTower = null;
        foreach (GameObject tower in towers)
        {
            float distance = Vector2.Distance(transform.position, tower.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTower = tower;
            }
        }
        return nearestTower;
    }

    public GameObject FindTarget()
    {
        GameObject nearestTower = FindNearestTower();
        if (nearestTower != null)
        {
            // Raycast to the nearest tower
            Vector2 direction = (nearestTower.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, nearestTower.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Tiles"));

            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                // If the raycast hits a Tile collider, find the nearest tile and target it
                GameObject nearestTile = FindNearestTile();
                if (nearestTile != null)
                {
                    targetPosition = nearestTile.transform.position;
                    return nearestTile; // Return the new target
                }
                else
                {
                    targetPosition = Vector2.positiveInfinity;
                    return null; // Return null if no valid target is found
                }
            }
            else
            {
                targetPosition = nearestTower.transform.position;
                return nearestTower; // Return the tower as the target
            }
        }
        else
        {
            // If no tower is found, target the nearest tile
            GameObject nearestTile = FindNearestTile();
            if (nearestTile != null)
            {
                targetPosition = nearestTile.transform.position;
                return nearestTile; // Return the tile as the target
            }
            else
            {
                targetPosition = Vector2.positiveInfinity;
                return null; // Return null if no valid target is found
            }
        }
    }
}
