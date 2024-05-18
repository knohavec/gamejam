using UnityEngine;

public class AdjustableYOffset : MonoBehaviour
{
    // The offset value that can be adjusted in the Inspector
    public float yOffset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Adjust the position of the object when it's placed
        AdjustPosition();
    }

    // Function to adjust the position
    void AdjustPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += yOffset;
        transform.position = newPosition;
    }

    // Function to set the offset (can be called from other scripts or UI)
    public void SetYOffset(float newYOffset)
    {
        yOffset = newYOffset;
        AdjustPosition();
    }
}
