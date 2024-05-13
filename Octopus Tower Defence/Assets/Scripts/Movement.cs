using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 moveDirection; // The direction the enemy should move in

    public float speed = 5f; // Speed at which the enemy moves

    void Update()
    {
        // Move the enemy in the set direction
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }
}