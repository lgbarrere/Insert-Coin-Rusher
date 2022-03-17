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
    public const int ENEMY_LIMIT = 10; //Maximum d'ennemis simultanés
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
    public static readonly int INITIAL_HIGHSCORE = 150;
    public static int highScore = INITIAL_HIGHSCORE;
    [SerializeField] EmergencyCoin emergencyCoin;

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

    // Available spaceship control keys
    private readonly KeyCode[] spaceshipInputs = {
            KeyCode.Z, KeyCode.Q, KeyCode.S, KeyCode.D,
            KeyCode.Space, KeyCode.J, KeyCode.K
        };
    private float[] keyPressedTimes;
    private const float PRESS_CHEAT_KEY_TIME = 0.5f;

    void Start()
    {
        if (gameMusic == null) gameMusic = GetComponent<AudioSource>();

        interfaceController.SetGameManager(this);
        fenteFuel.SetGameManager(this);
        fenteBonus.SetGameManager(this);
        nbCoins = MAX_COIN;
        uIController.SetCoinText(nbCoins);
        uIController.SetCoinTextColor(new Color(255, 0, 0, 255));
        keyPressedTimes = new float[spaceshipInputs.Length];
        LoadHighScore();
        EndGame();
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

        // Update success
        if (isPlaying)
        {
            successManager.UpdatePacifist();
            GetFirstControlKeyHeld();
            if (score > INITIAL_HIGHSCORE)
            {
                successManager.ValidateSuccess(Success.EXPERT);
            }
        }
    }

    private void GetFirstControlKeyHeld()
    {
        KeyCode firstKeyHeld = KeyCode.None;
        for (int i = 0; i < spaceshipInputs.Length; i++)
        {
            KeyCode key = spaceshipInputs[i];
            if (Input.GetKey(key))
            {
                keyPressedTimes[i] += Time.deltaTime;
                if (keyPressedTimes[i] >= PRESS_CHEAT_KEY_TIME)
                {
                    firstKeyHeld = key;
                    // Reset key times
                    for (int j = 0; j < spaceshipInputs.Length; j++)
                    {
                        if (keyPressedTimes[j] != 0 && i != j)
                        {
                            keyPressedTimes[j] = 0;
                        }
                    }
                    break;
                }
            }
            else if (keyPressedTimes[i] > 0)
            {
                keyPressedTimes[i] = 0;
            }
        }
        // If true, the cheat code is correct and activates a CLEAR
        if(successManager.UpdateCheatCodeSuccess(firstKeyHeld))
        {
            ApplyBonus(Bonus.CLEAR);
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

    public void AddCoin(int nbCoins = 1)
    {
        if(!MaxCoinReached())
        {
            this.nbCoins += nbCoins;
            this.nbCoins = Mathf.Min(this.nbCoins, MAX_COIN);
            uIController.SetCoinText(this.nbCoins);
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
        spaceship.SetGameManager(this);
        
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
                if (enemyCount >= ENEMY_LIMIT)
                {
                    break;
                }

                //Détermine une position aléatoire sur l'écran
                rndPosX = Random.Range(-4, 4);
                startX = rndPosX * 0.3f;
                
                rndPosY = Random.Range(0, 2);
                startY = 1f - 0.2f * rndPosY;

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
    }

    private void ChangeIcon(int bonusID)
    {
        if (roulette.GetInteger("BonusID") != bonusID)
        {
            if (!roulette.GetBool("Launch"))
            {
                roulette.SetBool("Launch", true);
            }
            roulette.SetInteger("BonusID", bonusID);
        }
    }

    private void DestroyDrone()
    {
        if (droneActive)
        {
            successManager.ResetAssistedSuccess();
            Destroy(drone.gameObject);
            droneActive = false;
        }
    }

    public void ApplyBonus(Bonus bonus)
    {
        ChangeIcon((int)bonus);
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
                if (enemyCount == ENEMY_LIMIT)
                {
                    successManager.UpdateBoomingSuccess();
                }
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
                else
                {
                    DestroyDrone();
                }
                break;
            default:
                break;
        }
        ResetRoulette();
    }

    public void RandomBonus()
    {
        rouletteCurrentTime += Time.deltaTime;
        rouletteIconCurrentTime += Time.deltaTime;

        if (rouletteIconCurrentTime >= ROULETTE_ICON_TIMER)
        {
            rouletteIconCurrentTime -= ROULETTE_ICON_TIMER;
            // Get another random icon
            iconID = (iconID + Random.Range(0, 5) + 1) % 6;
            ChangeIcon(iconID);

            if (rouletteCurrentTime >= ROULETTE_TIMER)
            {
                Bonus bonus = (Bonus)iconID;
                ApplyBonus(bonus);
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
        successManager.UpdateSpeedrunSuccess(scoreToAdd);
        score = Mathf.Min(score + scoreToAdd, MAX_SCORE);
        scoreText.text = score.ToString();
        SetScoreText();
        successManager.UpdateExpert(score);
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
        DestroyDrone();

        // Reset roulette and remove icon
        ResetRoulette();
        roulette.SetBool("Launch", false);

        //Mise à jour de l'interface
        interfaceController.ShowGameOver();
        interfaceController.SetFuel(0);

        // Refill inventory
        emergencyCoin.ResetEmergencyCoin();
        AddCoin(MAX_COIN);

        // Reset success
        successManager.ResetFullGasSuccess();
        successManager.CancelPacifistSuccess();
        successManager.ResetSpeedrunSuccess();
        successManager.ResetCheatCodeSuccess();
        successManager.ResetBoomingSuccess();
        successManager.ResetMasterSuccess();

        gameMusic.Stop();
        isPlaying = false;
    }
}
