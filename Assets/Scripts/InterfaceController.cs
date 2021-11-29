using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfaceController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI txtTimer;
    [SerializeField] TextMeshProUGUI txtGameOver;
    [SerializeField] TextMeshProUGUI txtRestart;
    [SerializeField] Slider slider;
    AudioSource audioSourceSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSourceSlider = slider.GetComponent<AudioSource>();
        HideRestart();
    }

    public void SetFuel(float fuelLeft)
    {
        if (slider.value < fuelLeft)
        {
            audioSourceSlider.Play();
        }

        slider.value = fuelLeft;
    }

    public void ShowGameOver()
    {
        txtGameOver.gameObject.SetActive(true);
        StartCoroutine(ShowRestart());
    }

    public void HideGameOver()
    {
        txtGameOver.gameObject.SetActive(false);
        HideRestart();
    }

    IEnumerator ShowRestart()
    {
        yield return new WaitForSeconds(1f);

        txtRestart.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        gameManager.EnableControls();
    }

    void HideRestart()
    {
        txtRestart.gameObject.SetActive(false);
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
