using UnityEngine;
using UnityEngine.UI;

public class AudienceMeter : MonoBehaviour
{
    // --- UI Stuff ---
    public Slider meterSlider;
    public Image fillImage;

    // --- Number (0 to 100) ---
    [Range(0f, 100f)]
    public float emotionValue = 50f;

    // --- Line for change state ---
    public float boredMax = 40f;
    public float enjoyMax = 80f;

    // --- Colors ---
    public Color boredColor = Color.blue;
    public Color enjoyColor = Color.green;
    public Color angryColor = Color.red;
    public float cooldownSpeed = 5f;

    void Start()
    {
        if (meterSlider != null)
        {
            meterSlider.minValue = 0f;
            meterSlider.maxValue = 100f;
        }
    }

    void Update()
    {
        emotionValue -= cooldownSpeed * Time.deltaTime;

       
        emotionValue = Mathf.Clamp(emotionValue, 0f, 100f);
        // Keep number inside 0 - 100
        emotionValue = Mathf.Clamp(emotionValue, 0f, 100f);

        // Move the UI bar
        if (meterSlider != null)
        {
            meterSlider.value = emotionValue;
        }

        CheckColor();
    }

    void CheckColor()
    {
        if (emotionValue < boredMax)
        {
            // Too cold! (Bored)
            if (fillImage != null) fillImage.color = boredColor;
        }
        else if (emotionValue <= enjoyMax)
        {
            // Good! (Enjoy)
            if (fillImage != null) fillImage.color = enjoyColor;
        }
        else
        {
            // Too hot! (Angry)
            if (fillImage != null) fillImage.color = angryColor;
        }
    }
}