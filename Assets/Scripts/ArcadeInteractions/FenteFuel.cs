using UnityEngine;

public class FenteFuel : MonoBehaviour
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
        if (gameManager.nbCoins > 0)
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
        if (gameManager.UseCoin())
        {
            maPiece.animationIsON = true;
            gameManager.AddFuel();
            insertSound.Play();
        }
    }
}
