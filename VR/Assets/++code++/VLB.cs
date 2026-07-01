using UnityEngine;
using UnityEngine.UI;
using VLB;

public class LightSliderController : MonoBehaviour
{
    public Scrollbar brightnessSlider;
    public StageLightController stageLightController;
    public VolumetricLightBeamSD vlbBeam;

    public float maxLightIntensity = 5f;
    public float maxVLBIntensity = 0.2f;

    void Start()
    {
        brightnessSlider.value = 0f;
        OnSliderValueChanged(0f);
        brightnessSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        if (stageLightController != null)
        {
            stageLightController.baseIntensity = value * maxLightIntensity;
        }

        if (vlbBeam != null)
        {
            vlbBeam.intensityGlobal = value * maxVLBIntensity;
            vlbBeam.enabled = value > 0f;
            vlbBeam.UpdateAfterManualPropertyChange();
        }
    }
}