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
    public TextMeshProUGUI descriptionText;
    public Sprite[] tutorialSprites;
    [TextArea(2, 6)]
    public string[] tutorialTexts;

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
        if (tutorialSprites != null && currentIndex < tutorialSprites.Length)
            stepImage.sprite = tutorialSprites[currentIndex];

        if (pageText != null)
            pageText.text = (currentIndex + 1) + " / " + tutorialSprites.Length;

        if (descriptionText != null)
        {
            if (tutorialTexts != null && currentIndex < tutorialTexts.Length)
                descriptionText.text = tutorialTexts[currentIndex];
            else
                descriptionText.text = "";
        }

        if (prevButton != null)
            prevButton.interactable = currentIndex > 0;

        bool isLastPage = currentIndex == tutorialSprites.Length - 1;

        if (nextButton != null)
            nextButton.gameObject.SetActive(!isLastPage);

        if (startButton != null)
            startButton.gameObject.SetActive(isLastPage);
    }
}