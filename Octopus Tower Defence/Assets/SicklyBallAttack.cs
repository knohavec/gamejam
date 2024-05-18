using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SicklyBallAttack : MonoBehaviour
{
    public Animator animator;
    public LayerMask enemyLayer;

    private Tower tower;
    private bool isAttacking = false;
    private Tilemap tilemap;

    void Start()
    {
        tower = GetComponent<Tower>();
        if (tower == null)
        {
            Debug.LogError("Tower component not found on the GameObject.");
        }

        tilemap = FindObjectOfType<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found in the scene.");
        }
    }

    void Update()
    {
        if (!isAttacking)
        {
            CheckForEnemies();
        }
    }

    private void CheckForEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, tower.tower_attack_range, enemyLayer);
        if (hitEnemies.Length > 0)
        {
            isAttacking = true;
            animator.SetBool("IsExploding", true);
        }
    }

    // This method is called at the end of the explosion animation
    public void OnAttackComplete()
    {
        DealDamageToEnemies();
        DestroyTileAndObject();
    }

    private void DealDamageToEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, tower.tower_attack_range, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy_Stats enemyScript = enemy.GetComponent<Enemy_Stats>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(tower.towerdamage);
            }
        }
    }

    private void DestroyTileAndObject()
    {
        // Use a small radius to detect the tile directly under the sickly ball
        float detectionRadius = 0.1f;
        Collider2D[] hitTiles = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Debug.Log("Number of colliders found: " + hitTiles.Length);

        foreach (Collider2D tile in hitTiles)
        {
            Debug.Log("Collider found: " + tile.gameObject.name);

            if (tile.CompareTag("Tile"))
            {
                Debug.Log("Tile found: " + tile.gameObject.name);

                // Get the cell position of the tile
                Vector3Int cellPosition = tilemap.WorldToCell(tile.transform.position);

                // Remove the tile from the tilemap
                tilemap.SetTile(cellPosition, null);
                Debug.Log("Tile removed from tilemap at: " + cellPosition);

                // Destroy the tile GameObject
                Destroy(tile.gameObject);
                Debug.Log("Tile destroyed: " + tile.gameObject.name);
            }
        }

        // Destroy the sickly ball object
        Destroy(gameObject);
        Debug.Log("Sickly ball destroyed.");
    }

    void OnDrawGizmosSelected()
    {
        if (tower != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, tower.tower_attack_range);
        }
    }
}
