using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // We need this for UI

public class LightFlashController : MonoBehaviour
{
    [Header("Connect Scripts")]
    public Slider3D linkedSlider;
    public Toggle flashToggle; // Link your UI toggle here

    [Header("Light Settings")]
    public List<Light> targetLights = new List<Light>();

    [Header("Flash Settings")]
    public bool isFlashing = false;
    public float flashSpeed = 10f;
    [Range(0, 1)]
    public float minBright = 0.2f;

    void Update()
    {
        // Get state from UI toggle
        if (flashToggle != null)
        {
            isFlashing = flashToggle.isOn;
        }

        if (linkedSlider == null || targetLights.Count == 0) return;

        float basePower = linkedSlider.currentValue;

        foreach (Light lt in targetLights)
        {
            if (lt == null) continue;

            if (isFlashing)
            {
                // Flash the light
                float multiplier = Mathf.Lerp(minBright, 1f, (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f);
                lt.intensity = basePower * multiplier;
            }
            else
            {
                // Normal light
                lt.intensity = basePower;
            }
        }
    }
}