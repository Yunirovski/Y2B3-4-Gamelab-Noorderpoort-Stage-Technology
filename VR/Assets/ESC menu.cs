using UnityEngine;
using UnityEngine.InputSystem; 

public class ESCmenu : MonoBehaviour
{
    public GameObject menuPanel;
    private bool isPaused = false;

    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
       
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isPaused = !isPaused;

            // Show or hide the menu
            menuPanel.SetActive(isPaused);

            // Stop or start time
            Time.timeScale = isPaused ? 0f : 1f;

            // Unlock or lock mouse cursor
            if (isPaused)
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
}