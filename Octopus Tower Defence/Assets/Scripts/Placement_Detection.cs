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
    private bool hasTower = false; // Added boolean variable to track tower presence

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
        if (hasTower) // Check if a tower is already present
        {
            Debug.Log("Tower already present!");
            return;
        }

        Vector3 placementPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z + zOffset);

        if (BuildManager.main.TryPlaceTower(placementPosition))
        {
            hasTower = true; // Set to true after placing tower

            // Find the Tile GameObject at the position and set hasTower to true
            Collider2D hitCollider = Physics2D.OverlapPoint(transform.position);
            if (hitCollider != null)
            {
                Tile tile = hitCollider.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.SetTowerPresence(true);
                }
            }
        }
    }

    private void Update()
    {
    }
}
