using UnityEngine;
using TMPro;

public class SceneTimer : MonoBehaviour
{
    [SerializeField] private float startTime = 400f;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] ESCmenu escMenu;
    private float timeLeft;
    private bool isRunning = true;
    private bool isPaused = false;


    void Start()
    {
        timeLeft = startTime;
        UpdateTimerUI();
    }

    void Update()
    {
        if (!isRunning) return;

        if (timeLeft > 0f && !escMenu.isPaused)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f) timeLeft = 0f;
            UpdateTimerUI();
        }
        else if (!escMenu.isPaused)
        {
            isRunning = false;
            Debug.Log("Time up!");
            Application.Quit();
        }
    }

    public void PauseTimer()
    {
        isPaused = true;   
    }

    public void UnPauseTimer()
    {
        isPaused = false;
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = "Time Left: " + $"{minutes:00}:{seconds:00}";
    }
}