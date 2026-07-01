using UnityEngine;
using VLB;

public class StageLightController : MonoBehaviour
{
    public enum LightMode { Normal, Strobe }
    public enum MoveMode { Manual, AutoHorizontal, AutoVertical, Auto2D }

    [Header("Components")]
    public Light stageLight;
    public VolumetricLightBeamSD vlbBeam;

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

    // Keep their home positions so they don't group up
    private Vector3[] sharedStartPos;
    private Vector3[] personalStartPos;

    private float timer = 0f;

    void Start()
    {
        if (stageLight == null) stageLight = GetComponent<Light>();

        // Save home positions right when the game starts, never overwrite them!
        SaveAllStartingPositions();

        movingTargets = sharedTarget;
    }

    void Update()
    {
        HandleLightMode();
        HandleMoveMode();
        HandleLightMode();
        HandleMoveMode();
        HandleAudienceAnnoyance();
    }

    void SaveAllStartingPositions()
    {
        // Remember where the big ball is
        if (sharedTarget != null)
        {
            sharedStartPos = new Vector3[sharedTarget.Length];
            for (int i = 0; i < sharedTarget.Length; i++)
            {
                if (sharedTarget[i] != null)
                    sharedStartPos[i] = sharedTarget[i].localPosition;
            }
        }

        // Remember where the 5 small balls are
        if (personalTargets != null)
        {
            personalStartPos = new Vector3[personalTargets.Length];
            for (int i = 0; i < personalTargets.Length; i++)
            {
                if (personalTargets[i] != null)
                    personalStartPos[i] = personalTargets[i].localPosition;
            }
        }
    }

    public void ToggleTargets(bool isScattered)
    {
        movingTargets = isScattered ? personalTargets : sharedTarget;

        // Send the idle balls back home to prevent messy offsets
        if (isScattered) ResetTargetsToStart(sharedTarget, sharedStartPos);
        else ResetTargetsToStart(personalTargets, personalStartPos);
    }

    void ResetTargetsToStart(Transform[] targets, Vector3[] startPos)
    {
        if (targets == null || startPos == null) return;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
                targets[i].localPosition = startPos[i];
        }
    }

    void HandleLightMode()
    {
        if (currentLightMode == LightMode.Strobe)
        {
            // Use unscaledTime so it keeps flashing even if you press ESC to pause!
            bool isLightOn = Mathf.Repeat(Time.unscaledTime * strobeSpeed, 1f) < 0.5f;
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
        if (currentMoveMode == MoveMode.Manual || movingTargets == null) return;

        // Unscaled delta time so movement continues while the game is paused
        timer += Time.unscaledDeltaTime * moveSpeed;

        float offsetX = 0f;
        float offsetZ = 0f;

        switch (currentMoveMode)
        {
            case MoveMode.AutoHorizontal:
                offsetX = Mathf.Sin(timer) * rangeX;
                break;

            case MoveMode.AutoVertical:
                offsetZ = Mathf.Sin(timer) * rangeZ;
                break;

            case MoveMode.Auto2D:
                offsetX = Mathf.Sin(timer) * rangeX;
                offsetZ = Mathf.Sin(timer * 1.3f) * rangeZ;
                break;
        }

        // Pick the correct home positions to calculate offset from
        Vector3[] correctStartPos = movingTargets == sharedTarget ? sharedStartPos : personalStartPos;

        for (int i = 0; i < movingTargets.Length; i++)
        {
            if (movingTargets[i] != null && correctStartPos != null)
            {
                // Add the offset to their home position so they move together but keep their formation!
                movingTargets[i].localPosition = correctStartPos[i] + new Vector3(offsetX, 0f, offsetZ);
            }
        }
    }

    // --- UI Controls ---
    public void SetLightNormal()
    {
        currentLightMode = LightMode.Normal;
    }

    public void SetLightStrobe()
    {
        currentLightMode = LightMode.Strobe;
    }

    public void SetBrightness(float value)
    {
        baseIntensity = value;
    }

    public void SetVLBIntensity(float value)
    {
        if (vlbBeam == null) return;

        vlbBeam.intensityGlobal = Mathf.Clamp(value, 0f, 0.2f);
        vlbBeam.UpdateAfterManualPropertyChange();
    }

    public void SetMoveManual()
    {
        currentMoveMode = MoveMode.Manual;
    }

    public void SetMoveAutoHorizontal()
    {
        currentMoveMode = MoveMode.AutoHorizontal;
    }

    public void SetMoveAutoVertical()
    {
        currentMoveMode = MoveMode.AutoVertical;
    }

    public void SetMoveAuto2D()
    {
        currentMoveMode = MoveMode.Auto2D;
    }

    // Use this for UI Toggle / Checkbox
    public void ToggleStrobeMode(bool isStrobe)
    {
        currentLightMode = isStrobe ? LightMode.Strobe : LightMode.Normal;
    }

    // Use this for UI Dropdown (0 = Manual, 1 = Horizontal, etc.)
    public void SetMoveModeByIndex(int index)
    {
        currentMoveMode = (MoveMode)index;
    }
    public void ResetSharedTargets()
    {
        ResetTargetsToStart(sharedTarget, sharedStartPos);
    }

    public void ResetPersonalTargets()
    {
        ResetTargetsToStart(personalTargets, personalStartPos);
    }

    void HandleAudienceAnnoyance()
    {
        if (currentLightMode == LightMode.Strobe && audienceMeter != null)
        {
            audienceMeter.emotionValue += annoyanceSpeed * Time.deltaTime;
        }
    }
}