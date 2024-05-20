using System.Collections;
using UnityEngine;

public class SandDollar : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int worth = 1;
    [SerializeField] private float despawnDelay = 5f; // Delay before despawning if not clicked
    [SerializeField] private float moveSpeed = 3f; // Speed at which sand dollar moves towards the target
    [SerializeField] private Vector3 targetOffset = Vector3.zero; // Offset for the target position
    [SerializeField] private Color highlightColor = Color.yellow; // Color to highlight on mouse enter

    private Transform targetTransform;
    private bool isClicked = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on SandDollar object.");
        }

        Invoke(nameof(DespawnOnDelay), despawnDelay); // Start the despawn delay

        // Find the target object by tag
        GameObject targetObject = GameObject.FindGameObjectWithTag("SandDollarTarget");
        if (targetObject != null)
        {
            targetTransform = targetObject.transform;
        }
        else
        {
            Debug.LogError("Target object with tag 'SandDollarTarget' not found.");
        }
    }

    void OnMouseDown()
    {
        if (isClicked || targetTransform == null) return; // Prevent double clicking and check if targetTransform is assigned
        isClicked = true;
        StartCoroutine(MoveToTarget());
    }

    void UpdateCounter()
    {
        SandDollarManager.instance.AddSandDollars(worth);
    }

    void DespawnOnDelay()
    {
        if (!isClicked)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 targetPosition = targetTransform.position + targetOffset;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // Once the sand dollar reaches the target, update the counter and destroy it
        UpdateCounter();
        Destroy(gameObject);
    }

    void OnMouseEnter()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlightColor;
        }
    }

    void OnMouseExit()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
