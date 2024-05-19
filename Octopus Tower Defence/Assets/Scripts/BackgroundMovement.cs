using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    // Reference to the background objects to move
    public Transform[] backgroundObjects;

    // Speed at which the background objects move
    [SerializeField] private float moveSpeed = 1.0f;

    // Timeout duration in seconds
    [SerializeField] private float inputTimeout = 5.0f;

    private float inputTimer;
    private bool isMoving = true;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        inputTimer = inputTimeout;
    }

    private void Update()
    {
        // Reset timer if there is mouse input
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            inputTimer = inputTimeout;
            isMoving = true;
        }
        else
        {
            // Decrease timer if no mouse input
            inputTimer -= Time.deltaTime;
            if (inputTimer <= 0)
            {
                isMoving = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (isMoving)
        {
            // Move the background based on the camera's movement
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

            foreach (Transform backgroundObject in backgroundObjects)
            {
                backgroundObject.position += deltaMovement * moveSpeed;
            }

            lastCameraPosition = cameraTransform.position;
        }
    }
}
