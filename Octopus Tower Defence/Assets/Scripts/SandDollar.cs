using System.Collections;
using UnityEngine;

public class SandDollar : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int worth = 1;
    [SerializeField] private float despawnDelay = 5f; // Delay before despawning if not clicked
    [SerializeField] private float moveSpeed = 3f; // Speed at which sand dollar moves towards the UI
    [SerializeField] private Vector3 targetOffset = Vector3.zero; // Offset for the target position

    private RectTransform targetUI;
    private bool isClicked = false;

    void Start()
    {
        Invoke(nameof(DespawnOnDelay), despawnDelay); // Start the despawn delay

        // Find the target UI element by tag
        GameObject targetUIObject = GameObject.FindGameObjectWithTag("SandDollarTarget");
        if (targetUIObject != null)
        {
            targetUI = targetUIObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Target UI element with tag 'SandDollarTarget' not found.");
        }
    }

    void OnMouseDown()
    {
        if (isClicked || targetUI == null) return; // Prevent double clicking and check if targetUI is assigned
        isClicked = true;
        StartCoroutine(MoveToUI());
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

    private IEnumerator MoveToUI()
    {
        Vector3 targetPosition = GetWorldPositionFromUI(targetUI) + targetOffset;

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

    private Vector3 GetWorldPositionFromUI(RectTransform uiElement)
    {
        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, uiElement.position, Camera.main, out worldPosition);
        return worldPosition;
    }
}
