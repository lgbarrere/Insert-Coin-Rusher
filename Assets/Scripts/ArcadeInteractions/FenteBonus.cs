using UnityEngine;

public class FenteBonus : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ArcadeCoinSlot maPiece;
    [SerializeField] AudioSource insertSound;

    // Success variables
    public SuccessManager successManager;
    private float insertCoinsTimer = 0;
    private const float INSERT_COINS_REQUIRED_TIME = 2f;
    private int nbInsertedCoins = 0;
    private const int REQUIRED_INSERTED_COINS = 3;
    private bool mouseOverSlot;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void Update()
    {
        // Necessary section for the Speedrun success
        if (nbInsertedCoins > 0)
        {
            insertCoinsTimer += Time.deltaTime;
            if (insertCoinsTimer >= INSERT_COINS_REQUIRED_TIME)
            {
                nbInsertedCoins = 0;
                insertCoinsTimer = 0;
            }
        }
        if (!Menu.pause)
        {
            if (gameManager.nbCoins > 0 && gameManager.isPlaying && mouseOverSlot)
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
            if (!gameManager.isPlaying) return;

            if (gameManager.UseCoin())
            {
                nbInsertedCoins++;
                if (nbInsertedCoins >= REQUIRED_INSERTED_COINS)
                {
                    nbInsertedCoins = 0;
                    gameManager.ApplyBonus(Bonus.DRONE);
                    successManager.StartSpeedrunSuccess();
                }
                else
                {
                    gameManager.LaunchRoulette();
                }
                maPiece.animationIsON = true;
                insertSound.Play();
            }
        }
    }
}
