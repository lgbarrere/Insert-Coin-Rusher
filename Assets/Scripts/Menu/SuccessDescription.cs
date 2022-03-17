using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SuccessDescription : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler
{
    private Text[] textDescription;
    private Image[] successImages;
    private bool successUnlocked = false;
    private Slider slider;

    void Start()
    {
        textDescription = GetComponentsInChildren<Text>();
        successImages = GetComponentsInChildren<Image>();
        slider = GetComponentInChildren<Slider>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!textDescription[0].enabled)
        {
            textDescription[0].enabled = true;
        }
        if (!textDescription[1].enabled)
        {
            textDescription[1].enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textDescription[0].enabled)
        {
            textDescription[0].enabled = false;
        }
        if (textDescription[1].enabled)
        {
            textDescription[1].enabled = false;
        }
    }

    public void ShowSuccess()
    {
        // Icons
        if (successImages[0].enabled == successUnlocked)
        {
            successImages[0].enabled = !successUnlocked;
        }
        if (successImages[1].enabled != successUnlocked)
        {
            successImages[1].enabled = successUnlocked;
        }
        // Slider's images
        if (!successImages[2].enabled)
        {
            successImages[2].enabled = true;
        }
        if (!successImages[3].enabled)
        {
            successImages[3].enabled = true;
        }
        // Slider's text
        if (!textDescription[2].enabled)
        {
            textDescription[2].enabled = true;
        }
    }

    public void HideSuccess()
    {
        // Icons
        if (successImages[0].enabled != false)
        {
            successImages[0].enabled = false;
        }
        if (successImages[1].enabled != false)
        {
            successImages[1].enabled = false;
        }
        // Slider's images
        if (successImages[2].enabled != false)
        {
            successImages[2].enabled = false;
        }
        if (successImages[3].enabled != false)
        {
            successImages[3].enabled = false;
        }
        // Slider's text
        if (textDescription[2].enabled != false)
        {
            textDescription[2].enabled = false;
        }
    }

    public void SetLockedTextToUnlocked()
    {
        if (!successUnlocked)
        {
            textDescription[1].text = "Déverrouillé";
            textDescription[1].color = Color.white;
            successUnlocked = true;
        }
    }

    public void UpdateSlider(int value)
    {
        slider.value = value;
        textDescription[2].text = value + " / " + slider.maxValue;
    }
}
