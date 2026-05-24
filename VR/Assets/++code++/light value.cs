using UnityEngine;

public class SliderLightValue : MonoBehaviour
{
    public Slider3D slider3D;
    public float lightValue;

    void Update()
    {
        if (slider3D != null)
        {
            lightValue = slider3D.currentValue;
        }
    }
}