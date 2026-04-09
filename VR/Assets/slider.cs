using UnityEngine;

public class Slider3D : MonoBehaviour
{
    public Camera mainCam;      // Drag your Camera here
    public Transform pointA;    // Drag start point here
    public Transform pointB;    // Drag end point here

    [Range(0, 1)]
    public float sliderValue;

    private Player playerScript;

    void Start()
    {
        playerScript = Object.FindAnyObjectByType<Player>();

        // Auto-find camera if slot is empty
        if (mainCam == null) mainCam = Camera.main;
    }

    void Update()
    {
        // Object moves to match the sliderValue number
        if (pointA != null && pointB != null)
        {
            transform.position = Vector3.Lerp(pointA.position, pointB.position, sliderValue);
        }
    }

    void OnMouseDrag()
    {
        // Only drag if in Lighting Mode
        if (playerScript != null && playerScript.isInLightingConsoleMode)
        {
            HandleMouseMovement();
        }
    }

    void HandleMouseMovement()
    {
        if (mainCam == null || pointA == null || pointB == null) return;

        // 1. Get mouse position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCam.WorldToScreenPoint(transform.position).z;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        // 2. Math for track
        Vector3 line = pointB.position - pointA.position;
        float length = line.magnitude;
        Vector3 direction = line.normalized;

        // 3. Project mouse on track
        float dot = Vector3.Dot(worldPos - pointA.position, direction);
        sliderValue = Mathf.Clamp(dot / length, 0f, 1f);
    }
}