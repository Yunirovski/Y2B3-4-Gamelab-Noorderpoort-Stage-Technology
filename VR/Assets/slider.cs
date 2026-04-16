using UnityEngine;

public class Slider3D : MonoBehaviour
{
    public Camera mainCam;
    public Transform pointA;
    public Transform pointB;

    [Range(0, 1)]
    public float sliderValue;

    public float currentValue;
    public float maxLightIntensity = 1000f;
    public Light targetLight;

    private Player playerScript;

    void Start()
    {
        playerScript = Object.FindAnyObjectByType<Player>();

        if (mainCam == null)
            mainCam = Camera.main;
    }

    void Update()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = Vector3.Lerp(pointA.position, pointB.position, sliderValue);
        }

        currentValue = sliderValue * maxLightIntensity;

        if (targetLight != null)
        {
            targetLight.intensity = currentValue;
        }
    }

    void OnMouseDrag()
    {
        if (playerScript != null && playerScript.isInLightingConsoleMode)
        {
            HandleMouseMovement();
        }
    }

    void HandleMouseMovement()
    {
        if (mainCam == null || pointA == null || pointB == null) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCam.WorldToScreenPoint(transform.position).z;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        Vector3 line = pointB.position - pointA.position;
        float length = line.magnitude;
        Vector3 direction = line.normalized;

        float dot = Vector3.Dot(worldPos - pointA.position, direction);
        sliderValue = Mathf.Clamp(dot / length, 0f, 1f);
    }
}
