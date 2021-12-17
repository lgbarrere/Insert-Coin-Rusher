using UnityEngine;

public class FenteBonus : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ArcadeCoinSlot maPiece;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void OnMouseEnter()
    {
        if (gameManager.nbCoins > 0 && gameManager.isPlaying)
        {
            maPiece.Apparition();
        }
    }

    void OnMouseExit()
    {
        if (!maPiece.animationIsON)
        {
            maPiece.Disparition();
        }
    }

    void OnMouseDown()
    {
        if (!gameManager.isPlaying) return;

        if (gameManager.UseCoin())
        {
            maPiece.animationIsON = true;

            gameManager.RandomBonus();
        }
    }
}
