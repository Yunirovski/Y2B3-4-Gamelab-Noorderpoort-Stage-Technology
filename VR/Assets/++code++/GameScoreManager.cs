using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoreTrigger
{
    // The time to start the event.
    public float time;

    // The score bonus.
    public int bonus = 2;

    // Is the event done?
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

    [Header("Events")]
    public ScoreTrigger[] triggers;

    [Header("Status")]
    public int currentScore = 0;
    public int currentBonus = 1;

    void Update()
    {
        if (audioSource == null) return;
        if (audioSource.clip == null) return;

        float time = audioSource.time;

        // Update the progress bar.
        if (progressBar != null)
        {
            progressBar.value = time / audioSource.clip.length;
        }

        // Check all events.
        foreach (var trigger in triggers)
        {
            if (time >= trigger.time && !trigger.isDone)
            {
                trigger.isDone = true;
                ActivateEvent(trigger.bonus);
            }
        }

        // Show the score on the screen.
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }

        if (bonusText != null)
        {
            bonusText.text = currentBonus.ToString();
        }
    }


    void ActivateEvent(int newBonus)
    {
        currentBonus = newBonus;
        Debug.Log("Bonus is now: " + newBonus);

        // Add effects here.
    }

    public void ResetEvents()
    {
        float time = audioSource.time;

        // Reset future events.
        foreach (var trigger in triggers)
        {
            if (trigger.time > time)
            {
                trigger.isDone = false;
            }
        }
    }
}