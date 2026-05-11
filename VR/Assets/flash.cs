using UnityEngine;
using System.Collections.Generic;

public class LightFlashController : MonoBehaviour
{
    [Header("Connect Scripts")]
    public Slider3D linkedSlider; // Connect to your slider script

    [Header("Light Settings")]
    public List<Light> targetLights = new List<Light>();

    [Header("Flash Settings")]
    public bool isFlashing = false;
    public float flashSpeed = 10f; // How fast it flashes
    [Range(0, 1)]
    public float minBright = 0.2f; // How dark it gets

    void OnMouseDown()
    {
        isFlashing = !isFlashing; // change state
    }
    void Update()
    {
        if (linkedSlider == null || targetLights.Count == 0) return;

        float basePower = linkedSlider.currentValue;

        foreach (Light lt in targetLights)
        {
            if (lt == null) continue;

            if (isFlashing)
            {
                // Use a wave to go from 0 to 1
                // Make the light jump between minBright and 1
                float multiplier = Mathf.Lerp(minBright, 1f, (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f);
                lt.intensity = basePower * multiplier;
            }
            else
            {
                // If no flash, use slider power
                lt.intensity = basePower;
            }
        }
    }
}