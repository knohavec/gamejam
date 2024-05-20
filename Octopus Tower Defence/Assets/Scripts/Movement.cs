using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector2 targetPosition;
    public float speed = 5f;
    private Enemy_Stats enemyStats;

    private Vector2 lastPosition;
    private float stuckTimer;
    private float stuckCheckInterval = 1f; // Time interval to check if the enemy is stuck

    private Rigidbody2D rb;
    private bool hasValidTarget;

    private void Start()
    {
        enemyStats = GetComponent<Enemy_Stats>();
        targetPosition = Vector2.zero;
        lastPosition = transform.position;
        stuckTimer = stuckCheckInterval;
        rb = GetComponent<Rigidbody2D>();
        hasValidTarget = false;
    }

    private void Update()
    {
        UpdateTargetPosition();

        Vector2 direction = (targetPosition - (Vector2)transform.position);

        // Ensure the direction vector is not zero
        if (direction != Vector2.zero)
        {
            direction.Normalize();
            rb.velocity = direction * speed;
            RotateSprite(direction);
        }
        else
        {
            rb.velocity = Vector2.zero; // If the direction is zero, stop the enemy
        }

        CheckIfStuck();

        // Debug logs to track position and velocity
        // Debug.Log($"Enemy position: {transform.position}, velocity: {rb.velocity}, target: {targetPosition}");
    }

    private void UpdateTargetPosition()
    {
        if (!hasValidTarget || !TileExists(targetPosition))
        {
            GameObject nearestTile = FindNearestTile();
            if (nearestTile == null)
            {
                GameObject nearestTower = FindNearestTower();
                if (nearestTower != null)
                {
                    targetPosition = nearestTower.transform.position;
                    hasValidTarget = true;
                }
                else
                {
                    // If no target is found, move towards (0, 0)
                    targetPosition = Vector2.zero;
                    hasValidTarget = true;
                }
            }
            else
            {
                targetPosition = nearestTile.transform.position;
                hasValidTarget = true;
            }
        }

        // Check if there is a clear path to the target position
        if (hasValidTarget)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition);
            if (hit.collider != null && !hit.collider.CompareTag("Tile"))
            {
                // If there is an obstacle in the way, find a new target
                hasValidTarget = false;
            }
        }
    }

    private bool TileExists(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tile"))
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

        // Calculate the screen's center in world coordinates
        Vector3 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

        // Check if the sprite's position is left of the screen center
        bool isLeftOfScreen = transform.position.x < screenCenter.x;

        if (isLeftOfScreen)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Flip on X and Y axis
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), -Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
    }

    private GameObject FindNearestTile()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyStats.TargetingRadius);
        float nearestDistance = Mathf.Infinity;
        GameObject nearestTile = null;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tile"))
            {
                Tile tile = collider.GetComponent<Tile>();
                if (tile != null)
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestTile = collider.gameObject;
                    }
                }
            }
        }
        return nearestTile;
    }

    private GameObject FindNearestTower()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyStats.TargetingRadius);
        float nearestDistance = Mathf.Infinity;
        GameObject nearestTower = null;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tower"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTower = collider.gameObject;
                }
            }
        }
        return nearestTower;
    }

    public GameObject FindTarget()
    {
        GameObject nearestTower = FindNearestTower();
        if (nearestTower != null)
        {
            targetPosition = nearestTower.transform.position;
            hasValidTarget = true;
            return nearestTower;
        }

        GameObject nearestTile = FindNearestTile();
        if (nearestTile != null)
        {
            targetPosition = nearestTile.transform.position;
            hasValidTarget = true;
            return nearestTile;
        }

        // If no target is found, set target position to (0, 0)
        targetPosition = Vector2.zero;
        hasValidTarget = true;
        return null;
    }

    private void CheckIfStuck()
    {
        stuckTimer -= Time.deltaTime;

        if (stuckTimer <= 0)
        {
            if (Vector2.Distance(transform.position, lastPosition) < 0.1f)
            {
                // The enemy is stuck, find a new target
                hasValidTarget = false;
                // Debug.Log($"Enemy is stuck at position {transform.position}, finding new target.");
            }

            // Reset the timer and update last position
            stuckTimer = stuckCheckInterval;
            lastPosition = transform.position;
        }
    }
}
