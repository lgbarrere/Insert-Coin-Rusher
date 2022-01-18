using UnityEngine;

public class FenteBonus : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ArcadeCoinSlot maPiece;
    [SerializeField] AudioSource insertSound;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void OnMouseOver()
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
            gameManager.LaunchRoulette();
            insertSound.Play();
        }
    }
}
