using UnityEngine;

public class LayeredMusicController : MonoBehaviour
{
    [Header("Audio Channels")]
    public AudioSource channel1_Base;
    public AudioSource channel2_Focus;
    public AudioSource channel3_Skill;

    [Header("References")]
    public pointsystem pointSystem;
    public StageLightController stageLightController;

    [Header("Volumes")]
    [Range(0f, 1f)] public float baseVolume = 1f;
    [Range(0f, 1f)] public float focusVolume = 1f;
    [Range(0f, 1f)] public float skillVolume = 1f;
    public float fadeSpeed = 8f;

    void Start()
    {
        if (channel1_Base == null || channel2_Focus == null || channel3_Skill == null)
            return;

        channel1_Base.loop = true;
        channel2_Focus.loop = true;
        channel3_Skill.loop = true;

        channel1_Base.volume = baseVolume;
        channel2_Focus.volume = 0f;
        channel3_Skill.volume = 0f;

        channel1_Base.Play();
        channel2_Focus.Play();
        channel3_Skill.Play();
    }

    void Update()
    {
        if (channel1_Base == null || channel2_Focus == null || channel3_Skill == null)
            return;

        bool isFocusActive = pointSystem != null && pointSystem.isStarInRange;
        bool isSkillActive = stageLightController != null &&
                             stageLightController.currentMoveMode != StageLightController.MoveMode.Manual;

        channel1_Base.volume = Mathf.Lerp(channel1_Base.volume, baseVolume, Time.deltaTime * fadeSpeed);
        channel2_Focus.volume = Mathf.Lerp(channel2_Focus.volume, isFocusActive ? focusVolume : 0f, Time.deltaTime * fadeSpeed);
        channel3_Skill.volume = Mathf.Lerp(channel3_Skill.volume, isSkillActive ? skillVolume : 0f, Time.deltaTime * fadeSpeed);

        KeepSynced(channel1_Base, channel2_Focus);
        KeepSynced(channel1_Base, channel3_Skill);
    }

    void KeepSynced(AudioSource master, AudioSource slave)
    {
        if (master.clip == null || slave.clip == null) return;
        if (!master.isPlaying || !slave.isPlaying) return;

        float diff = Mathf.Abs(master.time - slave.time);
        if (diff > 0.03f)
        {
            slave.time = master.time;
        }
    }
}