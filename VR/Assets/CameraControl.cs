using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField] GameObject[] cameras;
    bool start_flag = false;
    int i;
    public Button switchButton;
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
        
    }


    public void SwitchToTheNext()
    {
        if (!start_flag)
        {
            start_flag = true;
        }

        if (i == 0 && start_flag)
        {
            cameras[4].SetActive(false);
            cameras[0].SetActive(true);
            i++;
        }
        else if (i!=0 && start_flag)
        {
            cameras[i].SetActive(false);
            i++;
            cameras[i].SetActive(true);
        }    
        if (i == cameras.Length - 1) i = 0;
    }

}



