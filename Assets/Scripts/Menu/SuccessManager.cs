using UnityEngine;
using UnityEngine.UI;

enum Success
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
    private int nbFuelSlotUses = 0;
    private const int MAX_FUEL_SLOT_USES = 30;
    private int nbPickedUpCoin = 0;
    private const int MAX_PICKED_UP_COIN = 50;
    private int nbEnemiesKilled = 0;
    private const int MAX_ENEMY_KILLED = 300;
    private bool startedPacifistSuccess = false;
    private float pacifistTimer = 0;
    private const float PACIFIST_TIME_SUCCESS = 120f;
    private bool startedSpeedrunSuccess = false;
    private int speedrunPoints = 0;
    private const int SPEEDRUN_POINTS_SUCCESS = 100;
    private int droneKills = 0;
    private const int REQUIRED_DRONE_KILLS = 20;
    private float cheatKeyTimer = 0;
    private const float PRESS_CHEAT_KEY_TIME = 0.5f;
    private KeyCode[] cheatCode = {
        KeyCode.J, KeyCode.S, KeyCode.Space, KeyCode.K, KeyCode.Z
    };
    private int nbCorrectCheatKeys = 0;
    private KeyCode lastCheatKey = KeyCode.None;
    [SerializeField] AudioSource validCheatKeySound;
    [SerializeField] AudioSource wrongCheatCodeSound;

    void Start()
    {
        successImages = GetComponentsInChildren<Image>();
        for (int i = 0; i < 10; i++)
        {
            successIDs[i] = false;
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

    public void ResetFullGasSuccess()
    {
        if(nbFuelSlotUses != 0)
        {
            nbFuelSlotUses = 0;
        }
    }

    public void UpdateFullGasSuccess()
    {
        nbFuelSlotUses++;
        if (nbFuelSlotUses == MAX_FUEL_SLOT_USES)
        {
            successIDs[(int)Success.FULL_GAS] = true;
            nbFuelSlotUses = 0;
        }
        Debug.Log("Full Gas : " + successIDs[(int)Success.FULL_GAS]);
    }

    public void UpdateMendicantSuccess()
    {
        nbPickedUpCoin++;
        if (nbPickedUpCoin == MAX_PICKED_UP_COIN)
        {
            successIDs[(int)Success.MENDICANT] = true;
            nbPickedUpCoin = 0;
        }
        Debug.Log("Mendicant : " + successIDs[(int)Success.MENDICANT]);
    }

    public void UpdateMassacreSuccess(int nbKills = 1)
    {
        nbEnemiesKilled += nbKills;
        if (nbEnemiesKilled >= MAX_ENEMY_KILLED)
        {
            successIDs[(int)Success.MASSACRE] = true;
            nbEnemiesKilled %= MAX_ENEMY_KILLED;
        }
        Debug.Log("Massacre : " + successIDs[(int)Success.MASSACRE] + " " + nbEnemiesKilled);
        if (nbKills > 0)
        {
            CancelPacifistSuccess();
        }
    }

    public void StartPacifistSuccess()
    {
        if (!startedPacifistSuccess)
        {
            startedPacifistSuccess = true;
            Debug.Log("Start Pacifist");
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
                successIDs[(int)Success.PACIFIST] = true;
                CancelPacifistSuccess();
                Debug.Log("Pacifist : " + successIDs[(int)Success.PACIFIST]);
            }
        }
    }

    public void StartSpeedrunSuccess()
    {
        if (!startedSpeedrunSuccess)
        {
            startedSpeedrunSuccess = true;
            Debug.Log("Start speedrun");
        }
    }

    public void ResetSpeedrunSuccess()
    {
        if (startedSpeedrunSuccess)
        {
            speedrunPoints = 0;
            startedSpeedrunSuccess = false;
            Debug.Log("Reset speedrun");
        }
    }

    public void UpdateSpeedrunSuccess(int points)
    {
        if (startedSpeedrunSuccess)
        {
            speedrunPoints += points;
            if (speedrunPoints >= SPEEDRUN_POINTS_SUCCESS)
            {
                successIDs[(int)Success.SPEEDRUN] = true;
                ResetSpeedrunSuccess();
            }
            Debug.Log("Speedrun : " + successIDs[(int)Success.SPEEDRUN] + " " + speedrunPoints);
        }
    }

    public void ResetAssistedSuccess()
    {
        if (droneKills != 0)
        {
            droneKills = 0;
            Debug.Log("Reset assisted");
        }
    }

    public void UpdateAssistedSuccess()
    {
        droneKills++;
        if (droneKills >= REQUIRED_DRONE_KILLS)
        {
            successIDs[(int)Success.ASSISTED] = true;
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
                            successIDs[(int)Success.CHEAT_CODE] = true;
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
}
