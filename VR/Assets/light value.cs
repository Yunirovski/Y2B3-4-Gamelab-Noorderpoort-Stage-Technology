using UnityEngine;

public class SliderLightValue : MonoBehaviour
{
    public Light targetLight;
    public float lightValue = 1000f;

    void Update()
    {
        if (targetLight != null)
        {
            targetLight.intensity = lightValue;
        }
    }

}