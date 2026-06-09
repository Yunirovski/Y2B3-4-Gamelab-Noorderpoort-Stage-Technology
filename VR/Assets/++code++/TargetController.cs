using UnityEngine;

public class UITargetController : MonoBehaviour
{
    [Header("Put Big Ball and Small Balls Here")]
    public Transform[] allTargets;

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
        float keyX = Input.GetAxisRaw("Horizontal");
        float keyZ = Input.GetAxisRaw("Vertical");

        float finalX = (keyX != 0f) ? keyX : inputX;
        float finalZ = (keyZ != 0f) ? keyZ : inputZ;

        if (finalX != 0f || finalZ != 0f)
        {
            Vector3 movement = new Vector3(finalX, 0f, finalZ) * moveSpeed * Time.unscaledDeltaTime;

            foreach (Transform t in allTargets)
            {
                if (t != null)
                {
                    Vector3 newPosition = t.localPosition + movement;
                    newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                    newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
                    t.localPosition = newPosition;
                }
            }
        }
    }

    public void SetMoveX(float x) { inputX = x; }
    public void StopMoveX() { inputX = 0f; }

    public void SetMoveZ(float z) { inputZ = z; }
    public void StopMoveZ() { inputZ = 0f; }
}