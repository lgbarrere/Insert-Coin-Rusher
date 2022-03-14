using UnityEngine;
using UnityEngine.UI;

public enum Success
{
    FULL_GAS = 0,
    MENDICANT = 1,
    MASSACRE = 2,
    PACIFIST = 3,
    SPEEDRUN = 4,
    ASSISTED = 5,
    CHEAT_CODE = 6,
    BOOMING = 7,
    EXPERT = 8,
    MASTER = 9,
}

public class SuccessManager : MonoBehaviour
{
    // Variables
    private Image[] successImages;
    private readonly bool[] successIDs = new bool[10];
    private SuccessDescription[] description;
    // Full gas success
    private int nbFuelSlotUses = 0;
    private const int MAX_FUEL_SLOT_USES = 30;
    // Mendicant success
    private int nbPickedUpCoin = 0;
    private const int MAX_PICKED_UP_COIN = 50;
    // Massacre success
    private int nbEnemiesKilled = 0;
    private const int MAX_ENEMY_KILLED = 300;
    // Pacifist success
    private bool startedPacifistSuccess = false;
    private float pacifistTimer = 0;
    private const float PACIFIST_TIME_SUCCESS = 120f;
    // Speedrun success
    private bool startedSpeedrunSuccess = false;
    private int speedrunPoints = 0;
    private const int SPEEDRUN_POINTS_SUCCESS = 100;
    // Assisted success
    private int droneKills = 0;
    private const int REQUIRED_DRONE_KILLS = 20;
    // Cheat code success
    private float cheatKeyTimer = 0;
    private const float PRESS_CHEAT_KEY_TIME = 0.5f;
    private readonly KeyCode[] cheatCode = {
        KeyCode.J, KeyCode.S, KeyCode.Space, KeyCode.K, KeyCode.Z
    };
    private int nbCorrectCheatKeys = 0;
    private KeyCode lastCheatKey = KeyCode.None;
    [SerializeField] AudioSource validCheatKeySound;
    [SerializeField] AudioSource wrongCheatCodeSound;
    // Booming success
    private int boomingUsesCount = 0;
    private const int REQUIRED_BOOMING_USES = 3;
    // Expert success (no necessary variable)
    // Master success
    private int startMasterSuccessCountDown = 3;
    private readonly bool[] masterSuccessProgress = { false, false, false };

    void Start()
    {
        successImages = GetComponentsInChildren<Image>();
        description = GetComponentsInChildren<SuccessDescription>();
        for (int i = 0; i < 10; i++)
        {
            successIDs[i] = false;
        }
    }

    void Update()
    {
        if (StartMasterSuccess())
        {
            UpdateMasterSuccess();
        }
    }

    public void ShowSuccess()
    {
        for (int i = 0; i < 10; i++)
        {
            if (successImages[2*i].enabled == successIDs[i])
            {
                successImages[2*i].enabled = !successIDs[i];
            }
            if (successImages[2*i+1].enabled != successIDs[i])
            {
                successImages[2*i+1].enabled = successIDs[i];
            }
        }
    }

    public void HideSuccess()
    {
        for (int i = 0; i < 20; i++)
        {
            if (successImages[i].enabled != false)
            {
                successImages[i].enabled = false;
            }
        }
    }

    public void ValidateSuccess(Success success)
    {
        if (!successIDs[(int)success])
        {
            successIDs[(int)success] = true;
            description[(int)success].SetLockedTextToUnlocked();
            Debug.Log("Expert : " + successIDs[(int)success]);
        }
    }

    public void ResetFullGasSuccess()
    {
        if(nbFuelSlotUses != 0)
        {
            nbFuelSlotUses = 0;
            Debug.Log("Reset Full gas");
        }
    }

    public void UpdateFullGasSuccess()
    {
        nbFuelSlotUses++;
        if (nbFuelSlotUses == MAX_FUEL_SLOT_USES)
        {
            ValidateSuccess(Success.FULL_GAS);
            nbFuelSlotUses = 0;
        }
        Debug.Log("Full Gas : " + successIDs[(int)Success.FULL_GAS] + " " + nbFuelSlotUses);
    }

    public void UpdateMendicantSuccess()
    {
        nbPickedUpCoin++;
        if (nbPickedUpCoin == MAX_PICKED_UP_COIN)
        {
            ValidateSuccess(Success.MENDICANT);
            nbPickedUpCoin = 0;
            if (startMasterSuccessCountDown == 0)
            {
                masterSuccessProgress[0] = true;
            }
        }
        Debug.Log("Mendicant : " + successIDs[(int)Success.MENDICANT] + " " + nbPickedUpCoin);
    }

    public void UpdateMassacreSuccess(int nbKills = 1)
    {
        nbEnemiesKilled += nbKills;
        if (nbEnemiesKilled >= MAX_ENEMY_KILLED)
        {
            ValidateSuccess(Success.MASSACRE);
            nbEnemiesKilled %= MAX_ENEMY_KILLED;
        }
        Debug.Log("Massacre : " + successIDs[(int)Success.MASSACRE] + " " + nbEnemiesKilled);
        if (startedPacifistSuccess && nbKills > 0)
        {
            CancelPacifistSuccess();
            ResetMasterSuccess();
        }
    }

    public void StartPacifistSuccess()
    {
        if (!startedPacifistSuccess)
        {
            Debug.Log("Start Pacifist");
            startedPacifistSuccess = true;
        }
    }

    public void CancelPacifistSuccess()
    {
        if (startedPacifistSuccess)
        {
            startedPacifistSuccess = false;
            pacifistTimer = 0;
            Debug.Log("Cancel Pacifist");
        }
    }

    public void UpdatePacifist()
    {
        if (startedPacifistSuccess)
        {
            pacifistTimer += Time.deltaTime;
            if (pacifistTimer >= PACIFIST_TIME_SUCCESS)
            {
                ValidateSuccess(Success.PACIFIST);
                CancelPacifistSuccess();
                Debug.Log("Pacifist : " + successIDs[(int)Success.PACIFIST]);
                if (startMasterSuccessCountDown == 0)
                {
                    masterSuccessProgress[2] = true;
                }
            }
        }
    }

    public void StartSpeedrunSuccess()
    {
        if (!startedSpeedrunSuccess)
        {
            startedSpeedrunSuccess = true;
            Debug.Log("Start Speedrun");
        }
    }

    public void ResetSpeedrunSuccess()
    {
        if (startedSpeedrunSuccess)
        {
            speedrunPoints = 0;
            startedSpeedrunSuccess = false;
            Debug.Log("Reset Speedrun");
        }
    }

    public void UpdateSpeedrunSuccess(int points)
    {
        if (startedSpeedrunSuccess)
        {
            speedrunPoints += points;
            if (speedrunPoints >= SPEEDRUN_POINTS_SUCCESS)
            {
                ValidateSuccess(Success.SPEEDRUN);
                ResetSpeedrunSuccess();
                Debug.Log("Speedrun : " + successIDs[(int)Success.SPEEDRUN] + " " + speedrunPoints);
                if (startMasterSuccessCountDown == 0)
                {
                    masterSuccessProgress[1] = true;
                }
            }
        }
    }

    public void ResetAssistedSuccess()
    {
        if (droneKills != 0)
        {
            droneKills = 0;
            Debug.Log("Reset Assisted");
        }
    }

    public void UpdateAssistedSuccess()
    {
        droneKills++;
        if (droneKills >= REQUIRED_DRONE_KILLS)
        {
            ValidateSuccess(Success.ASSISTED);
            ResetAssistedSuccess();
        }
        Debug.Log("Assisted : " + successIDs[(int)Success.ASSISTED] + " " + droneKills);
    }

    public void ResetCheatCodeSuccess()
    {
        if (nbCorrectCheatKeys != 0)
        {
            nbCorrectCheatKeys = 0;
        }
        if (cheatKeyTimer != 0)
        {
            cheatKeyTimer = 0;
        }
    }

    // Return true if the cheat code is correct, false otherwise
    public bool UpdateCheatCodeSuccess(KeyCode key)
    {
        // If a key is held pressed
        if (key != KeyCode.None)
        {
            if (key != lastCheatKey)
            {
                cheatKeyTimer += Time.deltaTime;
                if (cheatKeyTimer >= PRESS_CHEAT_KEY_TIME)
                {
                    lastCheatKey = key;
                    cheatKeyTimer = 0;
                    // If detected key is correct, remember it
                    if (key == cheatCode[nbCorrectCheatKeys])
                    {
                        nbCorrectCheatKeys++;
                        validCheatKeySound.Play();
                        Debug.Log("Cheat key : " + key);
                        if (nbCorrectCheatKeys >= cheatCode.Length)
                        {
                            ValidateSuccess(Success.CHEAT_CODE);
                            ResetCheatCodeSuccess();
                            Debug.Log("Cheat code : " + successIDs[(int)Success.CHEAT_CODE]);
                            return true;
                        }
                    }
                    // Reset otherwise
                    else if (nbCorrectCheatKeys != 0)
                    {
                        nbCorrectCheatKeys = 0;
                        wrongCheatCodeSound.Play();
                        Debug.Log("Cheat code wrong : " + key);
                    }
                }
            }
        }
        // Id the current held key is released, reset key detection
        else
        {
            if (cheatKeyTimer != 0)
            {
                cheatKeyTimer = 0;
            }
            if (lastCheatKey != KeyCode.None)
            {
                lastCheatKey = KeyCode.None;
            }
        }
        return false;
    }

    public void ResetBoomingSuccess()
    {
        if (boomingUsesCount != 0)
        {
            boomingUsesCount = 0;
            Debug.Log("Reset Booming");
        }
    }

    public void UpdateBoomingSuccess()
    {
        boomingUsesCount++;
        if (boomingUsesCount >= REQUIRED_BOOMING_USES)
        {
            ValidateSuccess(Success.BOOMING);
            ResetBoomingSuccess();
        }
        Debug.Log("Booming : " + successIDs[(int)Success.BOOMING] + " " + boomingUsesCount);
    }

    private bool StartMasterSuccess()
    {
        if (startMasterSuccessCountDown == 0)
        {
            return true;
        }
        else if (startMasterSuccessCountDown == 3 && nbPickedUpCoin > 0 && !startedSpeedrunSuccess && !startedPacifistSuccess)
        {
            startMasterSuccessCountDown--;
            Debug.Log("Start Master : " + startMasterSuccessCountDown);
        }
        else if (startMasterSuccessCountDown == 2 && nbPickedUpCoin > 0 && startedSpeedrunSuccess && !startedPacifistSuccess)
        {
            startMasterSuccessCountDown--;
            Debug.Log("Start Master : " + startMasterSuccessCountDown);
        }
        else if (startMasterSuccessCountDown == 1 && nbPickedUpCoin > 0 && startedSpeedrunSuccess && startedPacifistSuccess)
        {
            startMasterSuccessCountDown--;
            Debug.Log("Start Master : " + startMasterSuccessCountDown);
            return true;
        }
        return false;
    }

    public void ResetMasterSuccess()
    {
        for (int i = 0; i < masterSuccessProgress.Length; i++)
        {
            if (masterSuccessProgress[i])
            {
                masterSuccessProgress[i] = false;
            }
        }
        if (startMasterSuccessCountDown != 3)
        {
            startMasterSuccessCountDown = 3;
            Debug.Log("Reset Master");
        }
    }

    private void UpdateMasterSuccess()
    {
        if(masterSuccessProgress[0] && masterSuccessProgress[1] && masterSuccessProgress[2])
        {
            ValidateSuccess(Success.MASTER);
            Debug.Log("Master : " + successIDs[(int)Success.MASTER]);
        }
    }
}
