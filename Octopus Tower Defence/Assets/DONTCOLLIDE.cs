using UnityEngine;

public class DontGoThroughThings2D : MonoBehaviour
{
    public LayerMask layerMask = -1;
    public float skinWidth = 0.1f;
    public float searchCollisionVerticalOffset = 0.5f;

    private float minimumExtent;
    private float partialExtent;
    private Vector2 previousPosition;
    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y);
        partialExtent = minimumExtent * (1.0f - skinWidth);
    }

    void FixedUpdate()
    {
        if (myRigidbody.velocity.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Vector2 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > 0)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            bool isObstacle = FindObstacle(previousPosition + Vector2.up * searchCollisionVerticalOffset, movementThisStep, movementMagnitude, layerMask.value, out RaycastHit2D hitInfo);

            if (isObstacle)
            {
                if (hitInfo.collider.isTrigger)
                {
                    hitInfo.collider.SendMessage("OnTriggerEnter2D", myCollider);
                }
                else
                {
                    myRigidbody.position = hitInfo.point - Vector2.up * searchCollisionVerticalOffset - (movementThisStep / movementMagnitude) * partialExtent;
                }
            }
        }
        previousPosition = myRigidbody.position;
    }

    private bool FindObstacle(Vector2 position, Vector2 step, float distance, int layers, out RaycastHit2D hitInfo)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, step, distance, layers);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != myCollider)
            {
                hitInfo = hit;
                return true;
            }
        }
        hitInfo = default(RaycastHit2D);
        return false;
    }
}
