using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtCoin;
    
    // Update is called once per frame
    public void SetCoinText(int amount)
    {
        txtCoin.SetText(amount + "x");
    }

    public void SetCoinTextColor(Color color)
    {
        txtCoin.color = color;
    }
}
