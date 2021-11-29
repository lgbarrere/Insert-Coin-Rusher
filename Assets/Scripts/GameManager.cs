using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bonus
{
    SHIELD = 0, // Avoid one dammage
    RELOAD = 1, // Reload some fuel
    POWER = 2, // Power in lasers increased
    CLEAR = 3, // Kill all ennemies
    MALUS = 4 // Decrease power or number of lasers
}

public class GameManager : MonoBehaviour
{
    [SerializeField] int startFuel = 10; //Carburant en début de partie
    [SerializeField] int fuelMax = 10; //Carburant max
    [SerializeField] int fuelLeft; //Carburant restant
    [SerializeField] int enemyLimit = 10; //Maximum d'ennemis simultanés
    [SerializeField] public Spaceship spaceship;
    [SerializeField] InterfaceController interfaceController;
    [SerializeField] UIController uIController;
    [SerializeField] FenteFuel fenteFuel;
    [SerializeField] FenteBonus fenteBonus;
    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject goSpaceship;
    [SerializeField] GameObject goEnemy;

    public Animator roulette;

    int enemyCount; //Compteur d'ennemis
    float timeCounter; //Compteur de temps
    public int coins;
    bool canControl = false;
    public bool isPlaying = false;
    bool timerActive = false;
    
    // Start is called before the first frame update
    void Start()
    {

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        interfaceController.SetGameManager(this);
        fenteFuel.SetGameManager(this);
        fenteBonus.SetGameManager(this);
        EndGame();
    }

    // Update is called once per frame
    void Update()
    {
        //Quand la partie est terminée, appuyer sur espace relance le jeu
        /*if (Input.GetKeyDown(KeyCode.Space) && canControl && !isPlaying)
        {
            StartGame();
        }*/
        
        //Si le timer atteint 0 ou le joueur se fait toucher, la partie se termine
        if ((fuelLeft == 0 || spaceship == null) && canControl && isPlaying)
        {
            EndGame();
        }

        //Mise à jour du timer
        timeCounter += Time.deltaTime;
        if (timeCounter >= 2 && timerActive)
        {
            UpdateFuel();
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

    public void AddCoin()
    {
        coins++;
        uIController.SetCoinText(coins);
    }

    public bool UseCoin()
    {
        if (coins > 0) {
            coins--;
            uIController.SetCoinText(coins);
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

        //Destruction des ennemis restants
        Component[] enemies = GetComponentsInChildren<EnemyEye>();
        foreach (EnemyEye enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

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

        audioSource.Play();
        
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

                Vector3 startPos = new Vector3(startX, startY, 0);

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

    public void AddFuel(int amount)
    {
        if (isPlaying) {
            fuelLeft = Mathf.Min(fuelLeft + amount, fuelMax);
            UpdateFuel();
        } else if (canControl) {
            StartGame();
        }
    }

    void CleanEnnemies()
    {
        //Destruction des ennemis restants
        Component[] enemies = GetComponentsInChildren<EnemyEye>();
        foreach (EnemyEye enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    public void RandomBonus()
    {
        Bonus bonus = (Bonus)Random.Range(0, 5);

        roulette.SetInteger("Bonus", (int)bonus);


        switch (bonus)
        {
            case Bonus.SHIELD:
                spaceship.Shield();
                break;
            case Bonus.RELOAD:
                AddFuel(fuelMax);
                break;
            case Bonus.POWER:
                if(spaceship.powerShoot < spaceship.maxPowerShoot)
                {
                    spaceship.powerShoot++;
                }
                break;
            case Bonus.CLEAR:
                CleanEnnemies();
                AddFuel(fuelMax);
                break;
            case Bonus.MALUS:
                if (spaceship.powerShoot > 1)
                {
                    spaceship.powerShoot--;
                }
                break;
            default:
                break;
        }
    }

    public void ReduceEnemyCount()
    {
        enemyCount--;
    }

    //Met fin à la partie en cours
    void EndGame()
    {
        //Désactive le timer
        timerActive = false;

        //Désactive l'apparition d'ennemis
        StopCoroutine(CreateEnemies());

        DisableControls();

        //Destruction du vaisseau
        if (spaceship != null) DestroyImmediate(spaceship.gameObject, true);

        //Mise à jour de l'interface
        interfaceController.ShowGameOver();
        interfaceController.SetFuel(0);

        audioSource.Stop();

        isPlaying = false;
    }
}
