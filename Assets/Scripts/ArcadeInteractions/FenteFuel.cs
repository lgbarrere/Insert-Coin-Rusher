using UnityEngine;

public class FenteFuel : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ArcadeCoinSlot maPiece;
    
    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void OnMouseEnter()
    {
        if (gameManager.coins > 0)
        {
            maPiece.apparition();
        }
    }

    void OnMouseExit()
    {
        if (!maPiece.animationIsON)
        {
            maPiece.disparition();
        }
    }

    void OnMouseDown()
    {
        if (gameManager.UseCoin())
        {
            maPiece.animationIsON = true;
            gameManager.AddFuel(3);
        }
    }
}
