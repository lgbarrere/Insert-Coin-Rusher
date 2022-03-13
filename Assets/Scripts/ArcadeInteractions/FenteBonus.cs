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

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void Update()
    {
        if (nbInsertedCoins > 0)
        {
            insertCoinsTimer += Time.deltaTime;
            if (insertCoinsTimer >= INSERT_COINS_REQUIRED_TIME)
            {
                nbInsertedCoins = 0;
                insertCoinsTimer = 0;
            }
        }
    }

    void OnMouseOver()
    {
        if (!Menu.pause)
        {
            if (gameManager.nbCoins > 0 && gameManager.isPlaying)
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
