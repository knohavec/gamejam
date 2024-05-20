using System.Collections;
using UnityEngine;

public class CC_Chew_Attack : MonoBehaviour
{
    [SerializeField] private float pullSpeed = 2f;
    [SerializeField] private float pollutiumSpeed = 3f;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject pollutiumPrefab;
    [SerializeField] private RectTransform pollutiumTargetUI;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;

    public float attackSpeed;
    public float conversion_rate;
    public Tower tower;
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, targetingRange);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Meat"))
            {
                GameObject food = hitCollider.gameObject;

                Vector3 direction = (transform.position - food.transform.position).normalized;
                food.transform.position = Vector3.MoveTowards(food.transform.position, transform.position, pullSpeed * Time.deltaTime);

                if (Vector3.Distance(food.transform.position, transform.position) < 0.01f)
                {
                    ConvertFood(food);
                    StartCoroutine(ChewingAnimation());
                }
            }
        }
    }

    private void ConvertFood(GameObject food)
    {
        float meatValue = food.GetComponent<Meat>().pollution_value;
        totalMeat += meatValue;

        if (totalMeat >= conversion_rate)
        {
            int pollutionPoints = Mathf.FloorToInt(totalMeat / conversion_rate);

            for (int i = 0; i < pollutionPoints; i++)
            {
                GameObject pollutium = Instantiate(pollutiumPrefab, transform.position, Quaternion.identity);
                StartCoroutine(MovePollutium(pollutium));
            }

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
            pollutium.transform.position = Vector3.MoveTowards(pollutium.transform.position, targetPosition, pollutiumSpeed * Time.deltaTime);
            yield return null;
        }

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
        animator.SetBool("IsChewing", true);
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(attackSpeed + animationLength);

        animator.SetBool("IsChewing", false);
    }

    public void ChewMeat(GameObject meat)
    {
        Debug.Log("Chewing meat: " + meat.name);
        ConvertFood(meat);
    }
}
