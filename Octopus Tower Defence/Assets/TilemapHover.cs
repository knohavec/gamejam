using UnityEngine;

public class TilemapHover : MonoBehaviour
{
    public float hexSize = 1f; // Size of each hexagon
    public LayerMask gridLayer; // Layer containing the grid tiles
    public Material lineMaterial; // Material for the LineRenderer

    private LineRenderer lineRenderer;
    private Collider2D currentCollider;

    void Start()
    {
        // Create a new LineRenderer component for drawing hexagon outlines
        GameObject lineObj = new GameObject("LineRenderer");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 7;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
    }

    void Update()
    {
        // Cast a ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, gridLayer);

        if (hit.collider != null)
        {
            // Check if the mouse is over a different collider
            if (currentCollider != hit.collider)
            {
                currentCollider = hit.collider;

                // If the collider is empty (no hexagon tile present), draw the hexagon outline
                if (!Physics2D.OverlapCircle(hit.collider.transform.position, 0.1f))
                {
                    DrawHexagonOutline(hit.collider.transform.position);
                }
            }
        }
        else
        {
            // If the mouse is not over any collider, clear the LineRenderer
            currentCollider = null;
            lineRenderer.positionCount = 0;
        }
    }

    // Draw a hexagon outline at the specified position
    void DrawHexagonOutline(Vector3 position)
    {
        // Calculate the positions of the six vertices of the hexagon
        Vector3[] vertices = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            float angle = 2 * Mathf.PI / 6 * i;
            vertices[i] = new Vector3(Mathf.Cos(angle) * hexSize, Mathf.Sin(angle) * hexSize, 0);
        }

        // Set the positions of the LineRenderer
        for (int i = 0; i < 6; i++)
        {
            lineRenderer.SetPosition(i, position + vertices[i]);
        }

        // Close the loop by setting the last position to the first position
        lineRenderer.SetPosition(6, position + vertices[0]);
    }
}
