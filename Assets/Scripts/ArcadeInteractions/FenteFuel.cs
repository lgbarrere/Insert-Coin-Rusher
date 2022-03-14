using UnityEngine;

public class FenteFuel : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ArcadeCoinSlot maPiece;
    [SerializeField] AudioSource insertSound;
    public SuccessManager successManager;
    private bool mouseOverSlot;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void Update()
    {
        if (!Menu.pause)
        {
            if (gameManager.nbCoins > 0 && mouseOverSlot)
            {
                maPiece.Apparition();
            }
            else if (!maPiece.animationIsON && !mouseOverSlot)
            {
                maPiece.Disparition();
            }
        }
    }

    void OnMouseEnter()
    {
        mouseOverSlot = true;
    }

    void OnMouseExit()
    {
        mouseOverSlot = false;
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
