using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement_Detection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hover_color;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;

    private Color start_color;

    private void Start()
    {
        start_color = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hover_color;
    }

    private void OnMouseExit()
    {
        sr.color = start_color;
    }

    private void OnMouseDown()
    {
        Vector3 placementPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z + zOffset);

        // Find the Tile GameObject at the position and check if it has a tower
        Collider2D hitCollider = Physics2D.OverlapPoint(transform.position);
        if (hitCollider != null)
        {
            Tile tile = hitCollider.GetComponent<Tile>();
            if (tile != null)
            {
                if (tile.hasTower)
                {
                    Debug.Log("Tower already present on this tile!");
                    return;
                }

                if (BuildManager.main.TryPlaceTower(placementPosition))
                {
                    tile.SetTowerPresence(true);
                    Debug.Log("Tower placed successfully.");

                    Tower towerInstance = hitCollider.GetComponent<Tower>();
                    if (towerInstance != null)
                    {
                        towerInstance.SetParentTile(tile); // Assign the parent tile
                        Debug.Log("Parent tile set for the placed tower.");
                    }
                }
                else
                {
                    Debug.LogWarning("Failed to place tower.");
                }
            }
        }
    }

    private void Update()
    {
    }
}
