using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    // Reference to the background objects to move
    public Transform[] backgroundObjects;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        // Move the background based on the camera's movement
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        foreach (Transform backgroundObject in backgroundObjects)
        {
            backgroundObject.position += deltaMovement;
        }

        lastCameraPosition = cameraTransform.position;
    }
}
