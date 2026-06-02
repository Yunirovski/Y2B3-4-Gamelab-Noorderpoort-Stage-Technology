using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoreTrigger
{
    public float time;           // Start time
    public float duration = 5f;  // Show for how many seconds
    public int bonus = 2;        // Score multiplier
    public Sprite bonusSprite;   // UI sprite
    [HideInInspector]
    public bool isDone = false;
}

public class GameScoreManager : MonoBehaviour
{
    [Header("Music")]
    public AudioSource audioSource;
    public Slider progressBar;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI bonusText;

    [Header("UI")]
    public Image bonusImage;

    [Header("Events")]
    public ScoreTrigger[] triggers;

    [Header("Status")]
    public int currentScore = 0;
    public int currentBonus = 1;

    private float bonusEndTime = -1f;

    void Update()
    {
        if (audioSource == null || audioSource.clip == null) return;

        float time = audioSource.time;

        if (progressBar != null)
            progressBar.value = time / audioSource.clip.length;

        foreach (var trigger in triggers)
        {
            if (time >= trigger.time && !trigger.isDone)
            {
                trigger.isDone = true;
                ActivateEvent(trigger);
            }
        }

        if (bonusEndTime > 0f && time >= bonusEndTime)
        {
            currentBonus = 1;
            bonusEndTime = -1f;

            if (bonusImage != null)
                bonusImage.enabled = false;
        }

        if (scoreText != null) scoreText.text = currentScore.ToString();
        if (bonusText != null) bonusText.text = currentBonus.ToString();
    }

    void ActivateEvent(ScoreTrigger trigger)
    {
        currentBonus = trigger.bonus;
        bonusEndTime = trigger.time + trigger.duration;

        if (bonusImage != null && trigger.bonusSprite != null)
        {
            bonusImage.sprite = trigger.bonusSprite;
            bonusImage.enabled = true;
        }
    }

    public void ResetEvents()
    {
        float time = audioSource.time;

        foreach (var trigger in triggers)
        {
            if (trigger.time > time)
                trigger.isDone = false;
        }

        currentBonus = 1;
        bonusEndTime = -1f;

        if (bonusImage != null)
            bonusImage.enabled = false;
    }
}