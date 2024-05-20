using System.Collections;
using UnityEngine;

public class Pollutium : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private float delayBeforeDestroy = 0.5f;

    private RectTransform targetUI;

    private void Start()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("PollutiumTarget");
        if (targetObject != null)
        {
            targetUI = targetObject.GetComponent<RectTransform>();
            StartCoroutine(MoveToTarget());
        }
        else
        {
            Debug.LogError("Pollutium target UI element with tag 'PollutiumTarget' not found.");
        }
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 targetPosition = GetWorldPositionFromUI(targetUI) + targetOffset;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(delayBeforeDestroy);

        PollutiumManager.instance.AddPollutium(1);
        Destroy(gameObject);
    }

    private Vector3 GetWorldPositionFromUI(RectTransform uiElement)
    {
        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, uiElement.position, Camera.main, out worldPosition);
        return worldPosition;
    }
}
