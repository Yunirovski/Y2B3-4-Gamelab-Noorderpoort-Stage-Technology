using UnityEngine;
using UnityEngine.UI;

public class Slider3D : MonoBehaviour
{
    public Scrollbar uiScrollbar;

    [Range(0, 1)]
    public float sliderValue;

    public float currentValue;
    public float maxLightIntensity = 1000f;

    
    public Light targetLight;

    void Update()
    {
        // Read UI Scrollbar
        if (uiScrollbar != null)
        {
            sliderValue = uiScrollbar.value;
        }

        // Calculate
        currentValue = sliderValue * maxLightIntensity;

        // Change light brightness directly
        if (targetLight != null)
        {
            targetLight.intensity = currentValue;
        }
    }
}