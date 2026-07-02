using UnityEngine;
using UnityEngine.UI;

public class FiveScrollbarBounce : MonoBehaviour
{
    public enum MoveMode
    {
        Wave,
        AllTogether
    }

    public Scrollbar[] scrollbars;
    public float speed = 3f;
    public bool playOnStart = false;
    public MoveMode moveMode = MoveMode.Wave;

    bool isPlaying;

    void Start()
    {
        isPlaying = playOnStart;
    }

    void Update()
    {
        if (!isPlaying) return;
        if (scrollbars == null) return;

        float value = Mathf.Sin(Time.unscaledTime * speed) * 0.5f + 0.5f;

        for (int i = 0; i < scrollbars.Length; i++)
        {
            if (scrollbars[i] == null) continue;

            if (moveMode == MoveMode.Wave)
                scrollbars[i].value = Mathf.Sin(Time.unscaledTime * speed + i) * 0.5f + 0.5f;
            else
                scrollbars[i].value = value;
        }
    }

    public void SetWaveMode()
    {
        moveMode = MoveMode.Wave;
        isPlaying = true;
    }

    public void SetAllTogetherMode()
    {
        moveMode = MoveMode.AllTogether;
        isPlaying = true;
    }

    public void StopBounce()
    {
        isPlaying = false;
    }
}