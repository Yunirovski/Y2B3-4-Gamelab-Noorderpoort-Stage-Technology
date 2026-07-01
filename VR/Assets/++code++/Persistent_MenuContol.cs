using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonControl : MonoBehaviour

{

    [SerializeField] GameObject PauseMenu;

    [SerializeField] string PlayingScenePath;

    [SerializeField] string MainMenuPath;


    //Make sure to attach these Buttons in the Inspector
    public Button ResumeButton, RestartButton, QuitButton;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        ResumeButton.onClick.AddListener(ResumeTaskOnClick);
        //RestartButton.onClick.AddListener(RestartTaskOnClick);
        QuitButton.onClick.AddListener(QuitTaskOnClick);

    }

    void ResumeTaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked

        PauseMenu.SetActive(false);
        
        Debug.Log("Game is resumed");

    }

    void RestartTaskOnClick()
    {
        SceneManager.LoadScene(PlayingScenePath);

        Debug.Log("Game is restarted");
    }
    void QuitTaskOnClick()
    {

        SceneManager.LoadScene(MainMenuPath);
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("Quited");
    }

}

