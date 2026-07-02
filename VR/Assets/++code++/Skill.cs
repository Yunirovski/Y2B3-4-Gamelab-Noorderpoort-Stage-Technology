using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Skillsystem : MonoBehaviour
{
    public GameScoreManager scoreManager;
    public StageLightController stageLightController;
    public TextMeshProUGUI skillCountText;

    public int scorePerSkill = 50;
    public int maxSkillCount = 3;
    public float skillDuration = 5f;

    public Light skillLight1;
    public Light skillLight2;

    public Toggle strobeToggle;   

    private int currentSkillCount = 0;
    private int lastProcessedScore = 0;
    private bool skillActive = false;
    private float skillTimer = 0f;

    void Start()
    {
        if (skillLight1 != null) skillLight1.gameObject.SetActive(false);
        if (skillLight2 != null) skillLight2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (scoreManager == null) return;

        int newScore = scoreManager.currentScore;

        if (newScore - lastProcessedScore >= scorePerSkill)
        {
            int skillsToAdd = (newScore - lastProcessedScore) / scorePerSkill;
            currentSkillCount += skillsToAdd;
            lastProcessedScore += skillsToAdd * scorePerSkill;

            if (currentSkillCount > maxSkillCount)
                currentSkillCount = maxSkillCount;
        }

        if (skillActive)
        {
            skillTimer -= Time.deltaTime;
            if (skillTimer <= 0f)
                EndSkill();
        }

        if (skillCountText != null)
            skillCountText.text = currentSkillCount + "/" + maxSkillCount;
    }

    public void UseHorizontalSkill()
    {
        if (currentSkillCount <= 0 || skillActive) return;
        StartSkill();
        if (stageLightController != null)
            stageLightController.SetMoveAutoHorizontal();
    }

    public void UseVerticalSkill()
    {
        if (currentSkillCount <= 0 || skillActive) return;
        StartSkill();
        if (stageLightController != null)
            stageLightController.SetMoveAutoVertical();
    }

    public void Use2DSkill()
    {
        if (currentSkillCount <= 0 || skillActive) return;
        StartSkill();
        if (stageLightController != null)
            stageLightController.SetMoveAuto2D();
    }

    void StartSkill()
    {
        currentSkillCount--;
        skillActive = true;
        skillTimer = skillDuration;

        if (skillLight1 != null) skillLight1.gameObject.SetActive(true);
        if (skillLight2 != null) skillLight2.gameObject.SetActive(true);

        if (stageLightController != null)
            stageLightController.SetLightStrobe();

        if (strobeToggle != null)
            strobeToggle.isOn = true;
    }

    void EndSkill()
    {
        skillActive = false;

        if (skillLight1 != null) skillLight1.gameObject.SetActive(false);
        if (skillLight2 != null) skillLight2.gameObject.SetActive(false);

        if (stageLightController != null)
        {
            stageLightController.SetLightNormal();
            stageLightController.SetMoveManual();
        }

        if (strobeToggle != null)
            strobeToggle.isOn = false;
    }
}