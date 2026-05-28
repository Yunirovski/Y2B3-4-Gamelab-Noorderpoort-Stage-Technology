using UnityEngine;

public class AudioPulseController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Target")]
    public Transform target;
    public Vector3 baseScale = Vector3.one;
    public Vector3 boostScale = new Vector3(1.3f, 1.3f, 1.3f);

    [Header("Pulse")]
    public int sampleCount = 256;
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;
    public int lowStart = 1;
    public int lowEnd = 16;
    public float multiplier = 25f;
    public float smoothSpeed = 8f;

    private float[] spectrum;

    void Start()
    {
        if (target == null)
            target = transform;

        spectrum = new float[sampleCount];
    }

    void Update()
    {
        if (audioSource == null || target == null) return;

        audioSource.GetSpectrumData(spectrum, 0, fftWindow);

        float low = 0f;
        for (int i = lowStart; i < lowEnd && i < spectrum.Length; i++)
        {
            low += spectrum[i];
        }

        float pulse = Mathf.Clamp01(low * multiplier);
        Vector3 targetScale = Vector3.Lerp(baseScale, boostScale, pulse);
        target.localScale = Vector3.Lerp(target.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }
}
