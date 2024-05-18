using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Chew_Attack : MonoBehaviour
{
    [SerializeField] private float pullSpeed = 2f; // Speed at which food is pulled towards the sea sponge
    [SerializeField] private float pollutiumSpeed = 3f; // Speed at which pollutium moves towards the UI
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject pollutiumPrefab; // Pollutium prefab
    [SerializeField] private RectTransform pollutiumTargetUI; // Pollutium target UI RectTransform
    [SerializeField] private Vector3 targetOffset = Vector3.zero; // Offset for the target position

    public float attackSpeed;
    public float conversion_rate;
    public Tower tower; // Reference to the Tower instance
    private float targetingRange;

    private float totalMeat = 0f;

    private void Start()
    {
        if (tower != null)
        {
            targetingRange = tower.tower_attack_range;
            attackSpeed = tower.tower_attack_speed;
        }
        else
        {
            Debug.LogError("Tower reference is not set in Carnivor Clam");
        }
    }

    private void Update()
    {
        if (tower != null && !tower.isDestroyed)
        {
            PullFood();
        }
    }

    private void PullFood()
    {
        // Use Physics2D.OverlapCircleAll to find objects within range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, targetingRange);

        foreach (var hitCollider in hitColliders)
        {
            // Check for objects with the "Meat" tag
            if (hitCollider.gameObject.CompareTag("Meat"))
            {
                GameObject food = hitCollider.gameObject;

                // Move the food towards the tower
                Vector3 direction = (transform.position - food.transform.position).normalized;
                food.transform.position += direction * pullSpeed * Time.deltaTime;

                // Check if the food is close enough to the tower to be considered "consumed"
                if (Vector3.Distance(food.transform.position, transform.position) < 0.01f)
                {
                    // Convert food (represented by GameObject) to pollutium and destroy it upon contact
                    ConvertFood(food);

                    StartCoroutine(ChewingAnimation());
                }
            }
        }
    }

    private void ConvertFood(GameObject food)
    {
        // Access Meat script (assuming it has a pollution_value variable)
        float meatValue = food.GetComponent<Meat>().pollution_value;

        // Track total meat collected
        totalMeat += meatValue;

        // Convert meat to pollution when totalMeat reaches the conversion rate
        if (totalMeat >= conversion_rate)
        {
            int pollutionPoints = Mathf.FloorToInt(totalMeat / conversion_rate); // Convert totalMeat to whole pollution points

            // Instantiate Pollutium prefab and start coroutine to move it
            for (int i = 0; i < pollutionPoints; i++)
            {
                GameObject pollutium = Instantiate(pollutiumPrefab, transform.position, Quaternion.identity);
                StartCoroutine(MovePollutium(pollutium));
            }

            // Reset totalMeat after conversion
            totalMeat -= (pollutionPoints * conversion_rate);
        }

        Destroy(food);
    }

    private IEnumerator MovePollutium(GameObject pollutium)
    {
        Vector3 targetPosition = GetWorldPositionFromUI(pollutiumTargetUI) + targetOffset;

        while (Vector3.Distance(pollutium.transform.position, targetPosition) > 0.01f)
        {
            Vector3 direction = (targetPosition - pollutium.transform.position).normalized;
            pollutium.transform.position += direction * pollutiumSpeed * Time.deltaTime;
            yield return null;
        }

        // Once the Pollutium reaches the target, update the Pollutium count and destroy it
        PollutiumManager.instance.AddPollutium(1);
        Destroy(pollutium);
    }

    private Vector3 GetWorldPositionFromUI(RectTransform uiElement)
    {
        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, uiElement.position, Camera.main, out worldPosition);
        return worldPosition;
    }

    private IEnumerator ChewingAnimation()
    {
        // Set the animator parameter
        animator.SetBool("IsChewing", true);

        // Wait for a frame to ensure the animator state is updated
        yield return null;

        // Get the length of the current animation clip
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Assuming it's on layer 0
        float animationLength = stateInfo.length;

        // Wait for the combined duration of attackSpeed and animationLength
        yield return new WaitForSeconds(attackSpeed + animationLength);

        animator.SetBool("IsChewing", false);
    }
}
