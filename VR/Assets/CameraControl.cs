using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CameraControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField] GameObject[] cameras;
    public TextMeshProUGUI cameraName;
    bool start_flag = false;
    int i;
    int name_number;
    public Button switchButton;

    public string[] camera_names;
    void Start()
    {
        Button btn = switchButton.GetComponent<Button>();
        btn.onClick.AddListener(SwitchToTheNext);
        int i = 0;
        for (int k = 0; k < 5; k++)
        {
            cameras[k].SetActive(false);
        }
        cameras[i].SetActive(true);
        cameraName.text = camera_names[name_number];
        name_number = 0;
        start_flag = false;
        
    }


    public void SwitchToTheNext()
    {
        Debug.Log("Switch Started");
        if (!start_flag)
        {
            start_flag = true;
            cameras[i].SetActive(false);
            i++;
            cameras[i].SetActive(true);
            cameraName.text = camera_names[name_number];
            name_number++;
        }
        else
        {
            if (i == 0 && start_flag)
            {
                cameras[4].SetActive(false);
                cameras[0].SetActive(true);
                i++;

                name_number = 0;
                Debug.Log("reset name");
                cameraName.text = camera_names[name_number];
                name_number++;
            }
            else // if (i != 0 && start_flag)
            {
                cameras[i].SetActive(false);
                i++;
                cameras[i].SetActive(true);
                cameraName.text = camera_names[name_number];
                name_number++;
                Debug.Log("else if");
            }
        }
        if (i == cameras.Length - 1) i = 0;
    }

}



