using UnityEngine;

public class AudioSpectrumVisualizer : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource[] audioSources;

    [Header("Bars")]
    public GameObject barPrefab;
    public int barCount = 64;
    public float spacing = 0.6f;
    public float minHeight = 0.2f;
    public float maxHeight = 10f;
    public float heightScale = 120f;
    public float smoothSpeed = 12f;

    [Header("Spectrum")]
    public int sampleCount = 1024;
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;
    public int startSample = 2;
    public float bassBoost = 2f;
    public float midBoost = 2f;
    public float highBoost = 2f;
    public float Boost = 50f;

    [Header("Color")]
    public Gradient colorGradient;
    public bool useEmission = true;
    public float emissionStrength = 2.0f;

    [Header("Player")]
    public KeyCode playPauseKey = KeyCode.DownArrow;
    public KeyCode forwardKey = KeyCode.RightArrow;
    public KeyCode backKey = KeyCode.LeftArrow;
    public float seekSeconds = 5f;

    private float[] spectrum;
    private float[] tempSpectrum;
    private Transform[] bars;
    private Material[] barMaterials;

    void Start()
    {
        if (audioSources == null || audioSources.Length == 0)
        {
            Debug.LogError("AudioSources are missing.");
            enabled = false;
            return;
        }

        if (barPrefab == null)
        {
            Debug.LogError("Bar prefab is missing.");
            enabled = false;
            return;
        }

        if (sampleCount < 64) sampleCount = 64;

        spectrum = new float[sampleCount];
        tempSpectrum = new float[sampleCount];
        bars = new Transform[barCount];
        barMaterials = new Material[barCount];

        if (colorGradient == null || colorGradient.colorKeys.Length == 0)
        {
            colorGradient = new Gradient();
            colorGradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(0.2f, 0.7f, 1f), 0f),
                    new GradientColorKey(new Color(0.3f, 1f, 0.5f), 0.5f),
                    new GradientColorKey(new Color(1f, 0.3f, 0.6f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );
        }

        BuildBars();
    }

    void Update()
    {
        HandleInput();
        UpdateBars();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(playPauseKey))
        {
            bool anyPlaying = false;

            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i] != null && audioSources[i].isPlaying)
                {
                    anyPlaying = true;
                    break;
                }
            }

            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i] == null) continue;

                if (anyPlaying)
                    audioSources[i].Pause();
                else
                    audioSources[i].Play();
            }
        }

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != null && audioSources[i].clip != null)
            {
                if (Input.GetKeyDown(forwardKey))
                    audioSources[i].time = Mathf.Clamp(audioSources[i].time + seekSeconds, 0f, audioSources[i].clip.length);

                if (Input.GetKeyDown(backKey))
                    audioSources[i].time = Mathf.Clamp(audioSources[i].time - seekSeconds, 0f, audioSources[i].clip.length);
            }
        }
    }

    void BuildBars()
    {
        float width = (barCount - 1) * spacing;

        for (int i = 0; i < barCount; i++)
        {
            GameObject bar = Instantiate(barPrefab, transform);
            bar.name = "Bar_" + i;

            Vector3 localPos = new Vector3(i * spacing - width * 0.5f, minHeight * 0.5f, 0f);
            bar.transform.localPosition = localPos;
            bar.transform.localRotation = Quaternion.identity;

            bars[i] = bar.transform;
            bars[i].localScale = new Vector3(0.4f, minHeight, 0.4f);

            Renderer renderer = bar.GetComponent<Renderer>();
            if (renderer != null)
            {
                barMaterials[i] = renderer.material;
                Color color = colorGradient.Evaluate(i / (float)(barCount - 1));
                barMaterials[i].color = color;

                if (useEmission)
                {
                    barMaterials[i].EnableKeyword("_EMISSION");
                    barMaterials[i].SetColor("_EmissionColor", color * emissionStrength);
                }
            }
        }
    }

    void UpdateBars()
    {
        if (audioSources == null || audioSources.Length == 0) return;

        for (int i = 0; i < spectrum.Length; i++)
            spectrum[i] = 0f;

        int activeSources = 0;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] == null || !audioSources[i].isPlaying) continue;

            audioSources[i].GetSpectrumData(tempSpectrum, 0, fftWindow);

            for (int j = 0; j < spectrum.Length; j++)
                spectrum[j] += tempSpectrum[j];

            activeSources++;
        }

        if (activeSources == 0) return;

        for (int i = 0; i < spectrum.Length; i++)
            spectrum[i] /= activeSources;

        int usableSamples = sampleCount - startSample;
        int bandSize = Mathf.Max(1, usableSamples / barCount);

        for (int i = 0; i < barCount; i++)
        {
            float sum = 0f;
            int start = startSample + i * bandSize;
            int end = Mathf.Min(start + bandSize, sampleCount);

            for (int s = start; s < end; s++)
                sum += spectrum[s];

            float value = sum / Mathf.Max(1, end - start);

            float t = i / (float)(barCount - 1);
            float boost = Mathf.Lerp(1f, Boost, t);
            value *= boost;

            if (t < 0.33f)
                value *= bassBoost;
            else if (t < 0.66f)
                value *= midBoost;
            else
                value *= highBoost;

            float targetHeight = minHeight + value * heightScale;
            targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeight);

            Vector3 scale = bars[i].localScale;
            scale.y = Mathf.Lerp(scale.y, targetHeight, Time.deltaTime * smoothSpeed);
            bars[i].localScale = scale;

            Vector3 pos = bars[i].localPosition;
            pos.y = scale.y * 0.5f;
            bars[i].localPosition = pos;

            if (barMaterials[i] != null && useEmission)
            {
                Color baseColor = colorGradient.Evaluate(i / (float)(barCount - 1));
                float glow = Mathf.Clamp(scale.y / maxHeight * 3f, 0.3f, 3f);
                barMaterials[i].SetColor("_EmissionColor", baseColor * emissionStrength * glow);
            }
        }
    }
}