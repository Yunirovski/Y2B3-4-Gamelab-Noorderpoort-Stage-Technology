using UnityEngine;

public class LayeredMusicController : MonoBehaviour
{
    public AudioSource channel1_Base;
    public AudioSource channel2_Focus;
    public AudioSource channel3_Skill;

    public pointsystem pointSystem;
    public StageLightController stageLightController;

    [Range(0f, 1f)] public float baseVolume = 1f;
    [Range(0f, 1f)] public float focusVolume = 1f;
    [Range(0f, 1f)] public float skillVolume = 1f;
    public float fadeSpeed = 8f;

    void Start()
    {
        if (channel1_Base == null || channel2_Focus == null || channel3_Skill == null) return;

        channel1_Base.loop = true;
        channel2_Focus.loop = true;
        channel3_Skill.loop = true;

        channel1_Base.volume = baseVolume;
        channel2_Focus.volume = 0f;
        channel3_Skill.volume = 0f;

        double startTime = AudioSettings.dspTime + 0.2f;
        channel1_Base.PlayScheduled(startTime);
        channel2_Focus.PlayScheduled(startTime);
        channel3_Skill.PlayScheduled(startTime);
    }

    void Update()
    {
        if (channel1_Base == null || channel2_Focus == null || channel3_Skill == null) return;

        if (channel1_Base.isPlaying == false && channel2_Focus.isPlaying == false && channel3_Skill.isPlaying == false) Debug.Log("game ended");

        bool isFocusActive = pointSystem != null && pointSystem.isStarInRange;
        bool isSkillActive = stageLightController != null &&
                             stageLightController.currentMoveMode != StageLightController.MoveMode.Manual;

        channel1_Base.volume = Mathf.Lerp(channel1_Base.volume, baseVolume, Time.deltaTime * fadeSpeed);
        channel2_Focus.volume = Mathf.Lerp(channel2_Focus.volume, isFocusActive ? focusVolume : 0f, Time.deltaTime * fadeSpeed);
        channel3_Skill.volume = Mathf.Lerp(channel3_Skill.volume, isSkillActive ? skillVolume : 0f, Time.deltaTime * fadeSpeed);
    }
}