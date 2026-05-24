using UnityEngine;

public class UITargetController : MonoBehaviour
{
    [Header("Move Speed")]
    public float moveSpeed = 15f;

    [Header("X Axis Bounds (Min / Max)")]
    public float minX = -100f;
    public float maxX = 100f;

    [Header("Z Axis Bounds (Min / Max)")]
    public float minZ = -100f;
    public float maxZ = 100f;

    private float inputX = 0f;
    private float inputZ = 0f;

    void Update()
    {
        // Move the object if we have any active button inputs
        if (inputX != 0f || inputZ != 0f)
        {
            // Calculate the movement step for this frame
            Vector3 movement = new Vector3(inputX, 0f, inputZ) * moveSpeed * Time.deltaTime;

            Vector3 newPosition = transform.position + movement;

            // Keep the object inside our allowed area boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            transform.position = newPosition;
        }
    }

    // --- Public methods for UI buttons ---

    public void SetMoveX(float x) { inputX = x; }
    public void StopMoveX() { inputX = 0f; }

    public void SetMoveZ(float z) { inputZ = z; }
    public void StopMoveZ() { inputZ = 0f; }
}