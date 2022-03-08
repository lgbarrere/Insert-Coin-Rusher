using UnityEngine;

public class Menu : MonoBehaviour
{
    GameObject menuObj;
    static public bool pause = false;

    public void Start()
    {
        menuObj = transform.GetChild(0).gameObject;
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
        menuObj.SetActive(pause);
        AudioListener.pause = pause;
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Quit()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }
}
