using UnityEngine;
using UnityEngine.InputSystem;

public class ESCmenu : MonoBehaviour
{
    public GameObject menuPanel;
    private bool isPaused = false;
    private Player playerScript;

    void Start()
    {
        playerScript = Object.FindAnyObjectByType<Player>();
        UpdateCursor();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isPaused = !isPaused;
            menuPanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
            UpdateCursor();
        }
    }

    public void UpdateCursor()
    {
        // Show mouse if Paused OR in Lighting Mode
        if (isPaused || (playerScript != null && playerScript.isInLightingConsoleMode))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}