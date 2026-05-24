using UnityEngine;

public class PressSpaceSlideUI : MonoBehaviour
{
    // --- UI position ---
    public RectTransform uiPanel;
    public Vector3 downPos;        
    public Vector3 upPos;           

    // --- Move speed ---
    public float speed = 10f;

    void Update()
    {
        if (uiPanel == null) return;

        //  Space key
        if (Input.GetKey(KeyCode.Space))
        {
            // Up
            uiPanel.anchoredPosition = Vector3.Lerp(uiPanel.anchoredPosition, upPos, Time.deltaTime * speed);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Down
            uiPanel.anchoredPosition = Vector3.Lerp(uiPanel.anchoredPosition, downPos, Time.deltaTime * speed);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}