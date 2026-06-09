using UnityEngine;

public class StageLightController : MonoBehaviour
{
    // States
    public enum LightMode { Normal, Strobe }
    public enum MoveMode { Manual, AutoHorizontal, AutoVertical, Auto2D }

    [Header("Components")]
    public Light stageLight;

    // --- Big ball and small balls ---
    [Header("Target Arrays")]
    public Transform[] sharedTarget;      // Just 1 big ball
    public Transform[] personalTargets;   // 5 small balls

    // The balls moving right now
    private Transform[] movingTargets;

    [Header("Light Settings")]
    public float baseIntensity = 10f;
    [Range(1f, 30f)]

    public float strobeSpeed = 15f; // Flash speed

    [Header("Auto Move Settings")]
    public float moveSpeed = 3f;
    public float rangeX = 20f; // X move range
    public float rangeZ = 20f; // Z move range

    [Header("Current States")]
    public LightMode currentLightMode = LightMode.Normal;
    public MoveMode currentMoveMode = MoveMode.Manual;

    private Vector3[] autoMoveCenters;
    private float timer = 0f;




    void Start()
    {
        if (stageLight == null) stageLight = GetComponent<Light>();

        // Start with the big ball
        movingTargets = sharedTarget;
        UpdateCenters();
    }

    void Update()
    {
        HandleLightMode();
        HandleMoveMode();
    }

    // --- For UI Toggle ---
    public void ToggleTargets(bool isScattered)
    {
        // True = 5 small balls, False = 1 big ball
        movingTargets = isScattered ? personalTargets : sharedTarget;

        // Save position so they don't teleport away
        UpdateCenters();
    }

    // Save start positions
    void UpdateCenters()
    {
        if (movingTargets != null)
        {
            autoMoveCenters = new Vector3[movingTargets.Length];
            for (int i = 0; i < movingTargets.Length; i++)
            {
                if (movingTargets[i] != null)
                {
                    autoMoveCenters[i] = movingTargets[i].position;
                }
            }
        }
    }

    void HandleLightMode()
    {
        if (currentLightMode == LightMode.Strobe)
        {
            // Mathf.Repeat 会在 0 到 1 之间不断循环。
            // 乘以 strobeSpeed 控制循环快慢。大于 0.5 开灯，小于 0.5 关灯。
            bool isLightOn = Mathf.Repeat(Time.time * strobeSpeed, 1f) > 0.5f;

            stageLight.enabled = isLightOn;       // 直接硬核开关灯光组件，防止高光残留
            stageLight.intensity = baseIntensity; // 保持开启时是你设定的高亮度
        }
        else
        {
            // 正常模式下确保灯是开着的
            stageLight.enabled = true;
            stageLight.intensity = baseIntensity;
        }
    }

    void HandleMoveMode()
    {
        if (currentMoveMode == MoveMode.Manual || movingTargets == null) return;

        timer += Time.deltaTime * moveSpeed;

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

        // Move all active balls
        for (int i = 0; i < movingTargets.Length; i++)
        {
            if (movingTargets[i] != null)
            {
                movingTargets[i].position = autoMoveCenters[i] + new Vector3(offsetX, 0f, offsetZ);
            }
        }
    }

    // --- UI Buttons ---
    public void SetLightNormal() { currentLightMode = LightMode.Normal; }
    public void SetLightStrobe() { currentLightMode = LightMode.Strobe; }
    public void SetBrightness(float value) { baseIntensity = value; }

    public void SetMoveManual()
    {
        currentMoveMode = MoveMode.Manual;
        UpdateCenters(); // Update position when back to manual
    }

    public void SetMoveAutoHorizontal() { currentMoveMode = MoveMode.AutoHorizontal; }
    public void SetMoveAutoVertical() { currentMoveMode = MoveMode.AutoVertical; }
    public void SetMoveAuto2D() { currentMoveMode = MoveMode.Auto2D; }

    // --- 新增的 UI 控制方法 ---

    // 1. 给 UI Toggle (复选框) 使用：勾选=闪烁，取消勾选=常亮
    public void ToggleStrobeMode(bool isStrobe)
    {
        currentLightMode = isStrobe ? LightMode.Strobe : LightMode.Normal;
    }

    // 2. 给 UI Dropdown (下拉菜单) 使用：0=手动, 1=水平, 2=垂直, 3=2D混合
    public void SetMoveModeByIndex(int index)
    {
        currentMoveMode = (MoveMode)index;

        // 如果切回手动模式，重新记录一下中心点，防止乱飘
        if (currentMoveMode == MoveMode.Manual)
        {
            UpdateCenters();
        }
    }
}