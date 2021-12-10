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
        if (gameManager.coins > 0 && gameManager.isPlaying)
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
        if (!gameManager.isPlaying) return;

        if (gameManager.UseCoin())
        {
            maPiece.animationIsON = true;

            gameManager.RandomBonus();
        }
    }
}
