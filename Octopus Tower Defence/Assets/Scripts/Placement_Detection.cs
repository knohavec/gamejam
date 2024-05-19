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

    private Camera cam;
    private Color start_color;

    private void Start()
    {
        start_color = sr.color;
        cam = Camera.main; // Automatically find the main camera
        if (cam == null)
        {
            Debug.LogError("Main Camera not found. Please ensure your scene has a Camera tagged as 'MainCamera'.");
        }
    }

    private void OnMouseEnter()
{
    if (sr != null)
    {
        sr.color = hover_color;
    }
}

private void OnMouseExit()
{
    if (sr != null)
    {
        sr.color = start_color;
    }
}


   private void OnMouseDown()
{
    if (cam == null)
    {
        Debug.LogError("Camera is not assigned.");
        return;
    }

    // Convert the mouse position to world point
    Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    Vector2 detectionPoint = new Vector2(mousePosition.x, mousePosition.y);

    // Find the Tile GameObject at the position and check if it has a tower
    RaycastHit2D hit = Physics2D.Raycast(detectionPoint, Vector2.zero);
    if (hit.collider != null)
    {
        Tile tile = hit.collider.GetComponent<Tile>();
        if (tile != null)
        {
            if (tile.hasTower)
            {
                Debug.Log("Tower already present on this tile!");
                return;
            }

            Vector3 placementPosition = new Vector3(tile.transform.position.x, tile.transform.position.y + yOffset, tile.transform.position.z + zOffset);
            if (BuildManager.main.TryPlaceTower(placementPosition))
            {
                tile.SetTowerPresence(true);
                Debug.Log("Tower placed successfully.");

                Tower towerInstance = hit.collider.GetComponentInChildren<Tower>();
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
    else
    {
        Debug.LogWarning("No tile detected at the specified point.");
    }
}

    private void Update()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }
}
