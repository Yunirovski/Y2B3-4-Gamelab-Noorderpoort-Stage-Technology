using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public GameObject tutorialPanel;
    public Image stepImage;
    public Button prevButton;
    public Button nextButton;
    public Button startButton;
    public TextMeshProUGUI pageText;
    public Sprite[] tutorialSprites;

    int currentIndex = 0;
    bool finishedTutorial = false;

    void Start()
    {
        tutorialPanel.SetActive(true);
        currentIndex = 0;
        UpdateUI();
    }

    public void NextStep()
    {
        if (currentIndex < tutorialSprites.Length - 1)
        {
            currentIndex++;
            UpdateUI();
        }

        if (currentIndex == tutorialSprites.Length - 1)
        {
            finishedTutorial = true;
        }
    }

    public void PrevStep()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }

    public void StartGame()
    {
        if (!finishedTutorial) return;
        tutorialPanel.SetActive(false);
    }

    void UpdateUI()
    {
        stepImage.sprite = tutorialSprites[currentIndex];
        pageText.text = (currentIndex + 1) + " / " + tutorialSprites.Length;
        prevButton.interactable = currentIndex > 0;
        bool isLastPage = currentIndex == tutorialSprites.Length - 1;
        nextButton.gameObject.SetActive(!isLastPage);
        startButton.gameObject.SetActive(isLastPage);
    }
}