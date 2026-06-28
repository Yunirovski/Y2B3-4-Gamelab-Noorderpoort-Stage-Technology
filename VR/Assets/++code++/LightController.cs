using UnityEngine;

public class StageLightController : MonoBehaviour
{
    public enum LightMode { Normal, Strobe }
    public enum MoveMode { Manual, AutoHorizontal, AutoVertical, Auto2D }

    [Header("Components")]
    public Light stageLight;

    [Header("Target Arrays")]
    public Transform[] sharedTarget;
    public Transform[] personalTargets;

    private Transform[] movingTargets;

    [Header("Light Settings")]
    public float baseIntensity = 10f;
    [Range(1f, 30f)]
    public float strobeSpeed = 15f;

    [Header("Auto Move Settings")]
    public float moveSpeed = 3f;
    public float rangeX = 20f;
    public float rangeZ = 20f;

    [Header("Current States")]
    public LightMode currentLightMode = LightMode.Normal;
    public MoveMode currentMoveMode = MoveMode.Manual;

    [Header("Audience Reaction")]
    public AudienceMeter audienceMeter;
    public float annoyanceSpeed = 15f;

    // Center positions for UI Button
    private Vector3[] sharedStartPos;
    private Vector3[] personalStartPos;

    private float timer = 0f;

    // Remember wave offset
    private float lastOffsetX = 0f;
    private float lastOffsetZ = 0f;

    void Start()
    {
        if (stageLight == null) stageLight = GetComponent<Light>();

        // Save center positions
        SaveAllStartingPositions();
        movingTargets = sharedTarget;

        // Hide skill balls at start
        ApplySkillState();
    }

    void Update()
    {
        HandleLightMode();
        HandleMoveMode();
        HandleAudienceAnnoyance();
    }

    void SaveAllStartingPositions()
    {
        if (sharedTarget != null)
        {
            sharedStartPos = new Vector3[sharedTarget.Length];
            for (int i = 0; i < sharedTarget.Length; i++)
            {
                if (sharedTarget[i] != null) sharedStartPos[i] = sharedTarget[i].localPosition;
            }
        }

        if (personalTargets != null)
        {
            personalStartPos = new Vector3[personalTargets.Length];
            for (int i = 0; i < personalTargets.Length; i++)
            {
                if (personalTargets[i] != null) personalStartPos[i] = personalTargets[i].localPosition;
            }
        }
    }

    public void ToggleTargets(bool isScattered)
    {
        movingTargets = isScattered ? personalTargets : sharedTarget;
        // Removed reset here, so it never snaps back!
    }

    // UI Button: Reset to center
    public void ResetPositionToCenter()
    {
        if (sharedTarget != null)
        {
            for (int i = 0; i < sharedTarget.Length; i++)
            {
                if (sharedTarget[i] != null) sharedTarget[i].localPosition = sharedStartPos[i];
            }
        }
        if (personalTargets != null)
        {
            for (int i = 0; i < personalTargets.Length; i++)
            {
                if (personalTargets[i] != null) personalTargets[i].localPosition = personalStartPos[i];
            }
        }
    }

    void HandleLightMode()
    {
        if (currentLightMode == LightMode.Strobe)
        {
            // Flash light
            bool isLightOn = Mathf.Repeat(Time.unscaledTime * strobeSpeed, 1f) > 0.5f;
            stageLight.enabled = isLightOn;
            stageLight.intensity = baseIntensity;
        }
        else
        {
            stageLight.enabled = true;
            stageLight.intensity = baseIntensity;
        }
    }

    void HandleMoveMode()
    {
        if (currentMoveMode == MoveMode.Manual || movingTargets == null)
        {
            // Reset wave memory
            lastOffsetX = 0f;
            lastOffsetZ = 0f;
            timer = 0f;
            return;
        }

        // Move even when game pauses
        timer += Time.unscaledDeltaTime * moveSpeed;

        float currentOffsetX = 0f;
        float currentOffsetZ = 0f;

        switch (currentMoveMode)
        {
            case MoveMode.AutoHorizontal:
                currentOffsetX = Mathf.Sin(timer) * rangeX;
                break;
            case MoveMode.AutoVertical:
                currentOffsetZ = Mathf.Sin(timer) * rangeZ;
                break;
            case MoveMode.Auto2D:
                currentOffsetX = Mathf.Sin(timer) * rangeX;
                currentOffsetZ = Mathf.Sin(timer * 1.3f) * rangeZ;
                break;
        }

        // Calculate wave difference
        float deltaX = currentOffsetX - lastOffsetX;
        float deltaZ = currentOffsetZ - lastOffsetZ;

        // Add difference to current position
        for (int i = 0; i < movingTargets.Length; i++)
        {
            if (movingTargets[i] != null)
            {
                movingTargets[i].localPosition += new Vector3(deltaX, 0f, deltaZ);
            }
        }

        lastOffsetX = currentOffsetX;
        lastOffsetZ = currentOffsetZ;
    }

    // UI Controls

    public void SetLightNormal() { currentLightMode = LightMode.Normal; }
    public void SetLightStrobe() { currentLightMode = LightMode.Strobe; }
    public void SetBrightness(float value) { baseIntensity = value; }

    public void SetMoveManual() { currentMoveMode = MoveMode.Manual; ApplySkillState(); }

    // Click again to turn off skill
    public void SetMoveAutoHorizontal()
    {
        currentMoveMode = (currentMoveMode == MoveMode.AutoHorizontal) ? MoveMode.Manual : MoveMode.AutoHorizontal;
        ApplySkillState();
    }
    public void SetMoveAutoVertical()
    {
        currentMoveMode = (currentMoveMode == MoveMode.AutoVertical) ? MoveMode.Manual : MoveMode.AutoVertical;
        ApplySkillState();
    }
    public void SetMoveAuto2D()
    {
        currentMoveMode = (currentMoveMode == MoveMode.Auto2D) ? MoveMode.Manual : MoveMode.Auto2D;
        ApplySkillState();
    }

    public void ToggleStrobeMode(bool isStrobe)
    {
        currentLightMode = isStrobe ? LightMode.Strobe : LightMode.Normal;
    }

    public void SetMoveModeByIndex(int index)
    {
        currentMoveMode = (MoveMode)index;
        ApplySkillState();
    }

    // Apply skill state
    private void ApplySkillState()
    {
        // Not manual means skill is on
        bool isSkillActive = (currentMoveMode != MoveMode.Manual);

        ToggleTargets(isSkillActive);

        // Show or hide balls
        Lighttargetmode modeScript = UnityEngine.Object.FindAnyObjectByType<Lighttargetmode>();
        if (modeScript != null)
        {
            modeScript.ToggleLightMode(isSkillActive);
        }
    }

    void HandleAudienceAnnoyance()
    {
        if (currentLightMode == LightMode.Strobe && audienceMeter != null)
        {
            audienceMeter.emotionValue += annoyanceSpeed * Time.deltaTime;
        }
    }
}