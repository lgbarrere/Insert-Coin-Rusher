using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SuccessDescription : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler
{
    private Text[] textDescription;
    void Start()
    {
        textDescription = GetComponentsInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (Text text in textDescription)
        {
            if (!text.enabled)
            {
                text.enabled = true;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Text text in textDescription)
        {
            if (text.enabled)
            {
                text.enabled = false;
            }
        }
    }

    public void SetLockedTextToUnlocked()
    {
        textDescription[1].text = "Déverrouillé";
        textDescription[1].color = Color.white;
    }
}
