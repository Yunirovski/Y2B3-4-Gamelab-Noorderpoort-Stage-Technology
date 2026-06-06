using UnityEngine;

public class StageLightController : MonoBehaviour
{
    // States
    public enum LightMode { Normal, Strobe }
    public enum MoveMode { Manual, AutoHorizontal, AutoVertical, Auto2D }

    [Header("Components")]
    public Light stageLight;
    public Transform movingTarget; // Target to look at

    [Header("Light Settings")]
    public float baseIntensity = 10f;
    public float strobeSpeed = 15f; // Flash speed

    [Header("Auto Move Settings")]
    public float moveSpeed = 3f;
    public float rangeX = 20f; // X move range
    public float rangeZ = 20f; // Z move range

    [Header("Current States")]
    public LightMode currentLightMode = LightMode.Normal;
    public MoveMode currentMoveMode = MoveMode.Manual;

    private Vector3 autoMoveCenter; // Start position
    private float timer = 0f;

    void Start()
    {
        if (stageLight == null) stageLight = GetComponent<Light>();
        if (movingTarget != null) autoMoveCenter = movingTarget.position;
    }

    void Update()
    {
        HandleLightMode();
        HandleMoveMode();
    }

    void HandleLightMode()
    {
        if (currentLightMode == LightMode.Strobe)
        {
            // Flash on and off
            float wave = Mathf.Sin(Time.time * strobeSpeed);
            stageLight.intensity = wave > 0 ? baseIntensity : 0f;
        }
        else
        {
            // Normal light
            stageLight.intensity = baseIntensity;
        }
    }

    void HandleMoveMode()
    {
        // Stop auto move if manual
        if (currentMoveMode == MoveMode.Manual || movingTarget == null) return;

        timer += Time.deltaTime * moveSpeed;

        float offsetX = 0f;
        float offsetZ = 0f;

        switch (currentMoveMode)
        {
            case MoveMode.AutoHorizontal:
                // Move on X axis
                offsetX = Mathf.Sin(timer) * rangeX;
                break;

            case MoveMode.AutoVertical:
                // Move on Z axis
                offsetZ = Mathf.Sin(timer) * rangeZ;
                break;

            case MoveMode.Auto2D:
                // Move in 2D (like an 8 shape)
                offsetX = Mathf.Sin(timer) * rangeX;
                offsetZ = Mathf.Sin(timer * 1.3f) * rangeZ;
                break;
        }

        // Set new position
        movingTarget.position = autoMoveCenter + new Vector3(offsetX, 0f, offsetZ);
    }

    // --- UI Buttons ---

    public void SetLightNormal() { currentLightMode = LightMode.Normal; }
    public void SetLightStrobe() { currentLightMode = LightMode.Strobe; }

    // For UI Slider
    public void SetBrightness(float value) { baseIntensity = value; }

    public void SetMoveManual()
    {
        currentMoveMode = MoveMode.Manual;
        // Reset start position
        if (movingTarget != null) autoMoveCenter = movingTarget.position;
    }

    public void SetMoveAutoHorizontal() { currentMoveMode = MoveMode.AutoHorizontal; }
    public void SetMoveAutoVertical() { currentMoveMode = MoveMode.AutoVertical; }
    public void SetMoveAuto2D() { currentMoveMode = MoveMode.Auto2D; }
}