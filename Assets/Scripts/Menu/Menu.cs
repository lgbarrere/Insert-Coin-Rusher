using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    Image[] menuImages;
    Text[] menuTexts;
    static public bool pause = false;
    public SuccessManager successManager;

    public void Start()
    {
        menuImages = GetComponentsInChildren<Image>();
        menuTexts = GetComponentsInChildren<Text>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void MenuScoreUpdate()
    {

    }

    public void PauseGame()
    {
        pause = !pause;

        // Hide menu
        foreach (Image image in menuImages)
        {
            image.enabled = pause;
        }
        foreach (Text text in menuTexts)
        {
            text.enabled = pause;
        }

        // Apply or not a pause on elements
        AudioListener.pause = pause;
        if (pause)
        {
            Time.timeScale = 0;
            successManager.ShowSuccess();
        }
        else
        {
            Time.timeScale = 1;
            successManager.HideSuccess();
        }
    }

    public void Quit()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }
}
