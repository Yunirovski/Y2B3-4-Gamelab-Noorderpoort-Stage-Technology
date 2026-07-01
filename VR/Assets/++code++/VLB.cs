using UnityEngine;
using UnityEngine.UI;
using VLB;

public class LightSliderController : MonoBehaviour
{
    public Scrollbar brightnessSlider;
    public Light unityLight;
    public VolumetricLightBeamSD vlbBeam;

    void Start()
    {
        brightnessSlider.value = 0f;
        OnSliderValueChanged(0f);
        brightnessSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        unityLight.intensity = value * 5f;
        vlbBeam.intensityGlobal = value * 0.2f;
        vlbBeam.enabled = value > 0f;
        vlbBeam.UpdateAfterManualPropertyChange();
    }
}