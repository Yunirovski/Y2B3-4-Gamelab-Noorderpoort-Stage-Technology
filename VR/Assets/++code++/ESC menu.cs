using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCmenu : MonoBehaviour
{
    public GameObject menuPanel;
    private bool isPaused = false;
    private Player playerScript;
    public Player player;
    public TMP_Text statusText;

    
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

        {
            if (player == null || statusText == null) return;

            statusText.text = player.isInLightingConsoleMode
                ? "Lighting Console Mode: ON"
                : "Lighting Console Mode: OFF";
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


