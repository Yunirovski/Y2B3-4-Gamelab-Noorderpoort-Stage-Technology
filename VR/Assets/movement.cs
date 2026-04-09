using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float mouseSensitivity = 1000f;
    public Transform cameraTransform;
    public float xRotation = 0f;
    public bool isInLightingConsoleMode = false;

    void Start()
    {
        EnterPlayerMode();
    }

    void Update()
    {
        if (isInLightingConsoleMode)
        {
            HandleLightingConsoleMode();
        }
        else
        {
            HandleMovementAndLook();
            HandleInteraction();
        }
    }

    void HandleMovementAndLook()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveInputX + transform.forward * moveInputZ;
        transform.position += move * moveSpeed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleInteraction()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("LeftButton!");
        }

        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("RightButton!");
        }
    }

    void HandleLightingConsoleMode()
    {
        if (Keyboard.current != null &&
            (Keyboard.current.wKey.wasPressedThisFrame ||
             Keyboard.current.aKey.wasPressedThisFrame ||
             Keyboard.current.sKey.wasPressedThisFrame ||
             Keyboard.current.dKey.wasPressedThisFrame))
        {
            ExitLightingConsoleMode();
        }
    }

    public void EnterLightingConsoleMode()
    {
        isInLightingConsoleMode = true;
        Object.FindAnyObjectByType<ESCmenu>().UpdateCursor();
    }

    public void ExitLightingConsoleMode()
    {
        isInLightingConsoleMode = false;
        Object.FindAnyObjectByType<ESCmenu>().UpdateCursor();
    }

    void EnterPlayerMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}