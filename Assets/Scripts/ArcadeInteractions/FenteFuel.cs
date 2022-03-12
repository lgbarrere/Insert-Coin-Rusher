using UnityEngine;

public class FenteFuel : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ArcadeCoinSlot maPiece;
    [SerializeField] AudioSource insertSound;
    public SuccessManager successManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void OnMouseOver()
    {
        if (!Menu.pause)
        {
            if (gameManager.nbCoins > 0)
            {
                maPiece.Apparition();
            }
        }
    }

    void OnMouseExit()
    {
        if (!Menu.pause)
        {
            if (!maPiece.animationIsON)
            {
                maPiece.Disparition();
            }
        }
    }

    void OnMouseDown()
    {
        if (!Menu.pause)
        {
            if (gameManager.UseCoin())
            {
                maPiece.animationIsON = true;
                if (gameManager.isPlaying)
                {
                    successManager.UpdateFullGasSuccess();
                }
                gameManager.AddFuel();
                insertSound.Play();
            }
        }
    }
}
