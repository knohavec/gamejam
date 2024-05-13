using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 moveDirection; // The direction the enemy should move in

    public float speed = 5f; // Speed at which the enemy moves

    void Update()
    {
        // Move the enemy in the set direction
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    // Optionally, you can add a method to smoothly rotate the enemy towards a target
    public void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 360f);
    }
}
