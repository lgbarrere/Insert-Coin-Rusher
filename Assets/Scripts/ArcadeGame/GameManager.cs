using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Bonus
{
    SHIELD = 0, // Avoid one dammage
    FUEL = 1, // Reload some fuel
    POWER = 2, // Power in lasers increased
    CLEAR = 3, // Kill all ennemies
    DRONE = 4, // Call a drone
    MALUS = 5 // Remove an existing bonus
}

public class GameManager : MonoBehaviour
{
    [SerializeField] int fuelLeft; //Carburant restant
    [SerializeField] int enemyLimit = 10; //Maximum d'ennemis simultanés
    [SerializeField] InterfaceController interfaceController;
    [SerializeField] UIController uIController;
    [SerializeField] FenteFuel fenteFuel;
    [SerializeField] FenteBonus fenteBonus;
    [SerializeField] AudioSource gameMusic;
    [SerializeField] AudioSource coinToInventorySound;
    [SerializeField] GameObject goSpaceship;
    [SerializeField] GameObject goEnemy;
    public Spaceship spaceship;
    public Animator roulette;
    public TextMeshProUGUI scoreText;

    // The elements in game
    public const int MAX_FUEL = 10; //Carburant max
    [SerializeField] int startFuel = MAX_FUEL; //Carburant en début de partie
    int enemyCount; //Compteur d'ennemis
    float timeCounter; //Compteur de temps
    public const int MAX_COIN = 3;
    public int nbCoins = MAX_COIN;
    bool canControl = false;
    public bool isPlaying = false;
    bool timerActive = false;
    public GameObject goDrone;
    private DroneManager drone;
    private bool droneActive = false;
    private const int MAX_SCORE = 10000000;
    public static int score = 0;
    public static int highScore = 0;

    // The elements of the roulette
    private const float ROULETTE_TIMER = 2f;
    private const float ROULETTE_ICON_TIMER = 0.2f;
    private float rouletteCurrentTime = 0f;
    private float rouletteIconCurrentTime = 0f;
    public static int iconID = 0;
    private bool activeRoulette = false;

    // The pause menu to update
    public Text pauseScoreText;
    public Text pauseHighScoreText;
    public SuccessManager successManager;

    void Start()
    {
        if (gameMusic == null) gameMusic = GetComponent<AudioSource>();

        interfaceController.SetGameManager(this);
        fenteFuel.SetGameManager(this);
        fenteBonus.SetGameManager(this);
        nbCoins = MAX_COIN;
        uIController.SetCoinText(nbCoins);
        uIController.SetCoinTextColor(new Color(255, 0, 0, 255));
        LoadHighScore();
        EndGame();
        successManager.InitializeSuccess();
    }

    void Update()
    {
        //Si le timer atteint 0 ou le joueur se fait toucher, la partie se termine
        if ((fuelLeft <= 0 || spaceship == null) && canControl && isPlaying)
        {
            EndGame();
        }

        //Mise à jour du timer
        timeCounter += Time.deltaTime;
        if (timerActive && timeCounter >= 2)
        {
            UpdateFuel();
        }

        // Move the drone if active
        if (droneActive)
        {
            drone.Move(spaceship.transform.position);
        }

        // Choose a bonus if the roulette is active
        if (activeRoulette)
        {
            RandomBonus();
        }
    }

    public void SaveHighScore()
    {
        SaveSystem.SaveData(highScore);
    }

    public void LoadHighScore()
    {
        GameData data = SaveSystem.LoadData();
        if(data != null)
        {
            highScore = data.score;
            SetHighscoreText();
        }
        else
        {
            highScore = 0;
        }
    }

    //Active les contrôles
    public void EnableControls()
    {
        canControl = true;
    }

    //Désactive les contrôles
    public void DisableControls()
    {
        canControl = false;
    }

    public bool MaxCoinReached()
    {
        if (nbCoins < MAX_COIN)
        {
            return false;
        }
        return true;
    }

    public void AddCoin()
    {
        if(!MaxCoinReached())
        {
            nbCoins++;
            uIController.SetCoinText(nbCoins);
            coinToInventorySound.Play();

            if (MaxCoinReached())
            {
                uIController.SetCoinTextColor(new Color(255, 0, 0, 255)); // Red
            }
        }
    }

    public bool UseCoin()
    {
        if (nbCoins > 0) {
            nbCoins--;
            uIController.SetCoinText(nbCoins);
            uIController.SetCoinTextColor(new Color(255, 255, 255, 255)); // White
            return true;
        } else {
            return false;
        }
    }

    void UpdateFuel()
    {
        if (!timerActive) return;

        timeCounter = 0;
        fuelLeft--;
        interfaceController.SetFuel(fuelLeft);
    }

    //Démarre une nouvelle partie
    void StartGame()
    {
        //Réinitialise le timer
        fuelLeft = startFuel;
        timeCounter = 0;
        timerActive = true;

        // Reset score
        ResetScore();

        //Destruction des ennemis restants
        CleanEnnemies();

        //Apparition du vaisseau
        Vector3 startPos = transform.position;
        startPos.y -= 1f;
        startPos.z = -0.65f;
        spaceship = Instantiate(goSpaceship, startPos, Quaternion.identity).GetComponent<Spaceship>();
        spaceship.transform.parent = transform;
        spaceship.GetComponent<Spaceship>().SetGameManager(this);
        
        //Mise à jour de l'interface
        interfaceController.HideGameOver();
        interfaceController.SetFuel(fuelLeft);

        gameMusic.Play();
        
        //EnableControls();
        isPlaying = true;

        StartCoroutine(CreateEnemies());
    }

    //Crée des ennemis durant la partie
    IEnumerator CreateEnemies()
    {
        int rndWait, rndNumber, rndPosX, rndPosY;
        float startX, startY;

        enemyCount = 0;

        while (isPlaying)
        {
            rndWait = Random.Range(3, 5); //Temps d'attente pour la prochaine vague

            rndNumber = Random.Range(2, 4); //Nombre d'ennemis sur cette vague

            for (float i = 0; i < rndNumber; i++)
            {
                //Attendre d'être en-dessous de la limite d'ennemis pour en faire apparaitre de nouveaux
                if (enemyCount >= enemyLimit)
                {
                    break;
                }

                //Détermine une position aléatoire sur l'écran
                rndPosX = Random.Range(-4, 4);
                startX = rndPosX * 0.3f;
                
                rndPosY = Random.Range(0, 2);
                startY = .85f - 0.2f * rndPosY;

                Vector3 startPos = new(startX, startY, 0);

                //Crée un ennemi à la position calculée
                GameObject enemy = Instantiate(goEnemy, transform, false);
                enemy.transform.parent = transform;
                enemy.transform.localPosition = startPos;
                enemy.GetComponent<EnemyEye>().SetGameManager(this);
                enemyCount++;

                //Délai entre chaque apparition
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(rndWait);
        }
    }

    public void AddFuel(int amount = MAX_FUEL)
    {
        if (isPlaying) {
            fuelLeft = Mathf.Min(fuelLeft + amount, MAX_FUEL);
            UpdateFuel();
        } else if (canControl) {
            StartGame();
        }
    }

    void CleanEnnemies()
    {
        if (enemyCount != 0)
        {
            //Destruction des ennemis restants
            Component[] enemies = GetComponentsInChildren<EnemyEye>();
            foreach (EnemyEye enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
            enemyCount = 0;
        }
    }

    public void LaunchRoulette()
    {
        roulette.SetBool("Launch", true);
        roulette.SetBool("Choosing", true);
        activeRoulette = true;
        iconID = Random.Range(0, 6);
        roulette.SetInteger("BonusID", iconID);
    }

    public void ResetRoulette()
    {
        if (activeRoulette)
        {
            activeRoulette = false;
            rouletteCurrentTime = 0f;
            rouletteIconCurrentTime = 0f;
            roulette.SetBool("Choosing", false);
        }
        roulette.SetBool("Launch", false);
    }

    public void RandomBonus()
    {
        rouletteCurrentTime += Time.deltaTime;
        rouletteIconCurrentTime += Time.deltaTime;

        if (rouletteIconCurrentTime >= ROULETTE_ICON_TIMER)
        {
            rouletteIconCurrentTime -= ROULETTE_ICON_TIMER;
            // ChangeIcon
            iconID = (iconID + Random.Range(0, 5) + 1) % 6;
            roulette.SetInteger("BonusID", iconID);

            // Reset roulette
            if (rouletteCurrentTime >= ROULETTE_TIMER)
            {
                rouletteCurrentTime = 0;
                rouletteIconCurrentTime = 0;
                activeRoulette = false;
                roulette.SetBool("Choosing", false);

                //Bonus
                Bonus bonus = (Bonus)iconID;
                switch (bonus)
                {
                    case Bonus.SHIELD:
                        spaceship.Shield();
                        break;
                    case Bonus.FUEL:
                        AddFuel(MAX_FUEL / 3);
                        break;
                    case Bonus.POWER:
                        if (spaceship.powerShoot < spaceship.maxPowerShoot)
                        {
                            spaceship.powerShoot++;
                        }
                        break;
                    case Bonus.CLEAR:
                        AddScore(enemyCount);
                        AddFuel(2 * enemyCount);
                        successManager.UpdateMassacreSuccess(enemyCount);
                        CleanEnnemies();
                        break;
                    case Bonus.DRONE:
                        if (!droneActive)
                        {
                            droneActive = true;
                            Vector3 startPos = spaceship.transform.position;
                            startPos.y += 0.2f;
                            drone = Instantiate(goDrone, startPos, Quaternion.identity).GetComponent<DroneManager>();
                        }
                        break;
                    case Bonus.MALUS:
                        if (spaceship.powerShoot > 1)
                        {
                            spaceship.powerShoot--;
                        }
                        else if (droneActive)
                        {
                            Destroy(drone.gameObject);
                            droneActive = false;
                        }
                        break;
                    default:
                        Debug.Log("Undefined bonus.");
                        break;
                }
            }
        }
    }

    public void ReduceEnemyCount()
    {
        enemyCount--;
        successManager.UpdateMassacreSuccess();
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
        SetScoreText();
    }

    public void AddScore(int scoreToAdd)
    {
        // Update the score if under the max score we can reach
        score = Mathf.Min(score + scoreToAdd, MAX_SCORE);
        scoreText.text = score.ToString();
        SetScoreText();
    }

    private void SetScoreText()
    {
        pauseScoreText.text = "Score actuel : " + score.ToString();
    }

    private void SetHighscoreText()
    {
        pauseHighScoreText.text = "Highscore : " + highScore.ToString();
    }

    //Met fin à la partie en cours
    void EndGame()
    {
        // Save the score as highscore and reset it
        if(score > highScore)
        {
            highScore = score;
            SetHighscoreText();
        }
        SaveHighScore();

        //Désactive le timer
        timerActive = false;

        //Désactive l'apparition d'ennemis
        StopCoroutine(CreateEnemies());

        DisableControls();

        //Destruction du vaisseau
        if (spaceship != null) DestroyImmediate(spaceship.gameObject, true);

        // Drone destruction if active
        if (droneActive)
        {
            DestroyImmediate(drone.gameObject, true);
            droneActive = false;
        }

        ResetRoulette();

        //Mise à jour de l'interface
        interfaceController.ShowGameOver();
        interfaceController.SetFuel(0);

        // Reset success
        successManager.ResetFullGasSuccess();

        gameMusic.Stop();

        isPlaying = false;
    }
}
