using UnityEngine;
using UnityEngine.UI;

enum Success
{
    FULL_GAS_LOCKED = 0,
    FULL_GAS_UNLOCKED = 1,
    MENDICANT_LOCKED = 2,
    MENDICANT_UNLOCKED = 3,
    MASSACRE_LOCKED = 4,
    MASSACRE_UNLOCKED = 5,
    PACIFIST_LOCKED = 6,
    PASIFIST_UNLOCKED = 7,
    SPEEDRUN_LOCKED = 8,
    SPEEDRUN_UNLOCKED = 9,
    ASSISTED_LOCKED = 10,
    ASSISTED_UNLOCKED = 11,
    CHEAT_CODE_LOCKED = 12,
    CHEAT_CODE_UNLOCKED = 13,
    BOOMING_LOCKED = 14,
    BOOMING_UNLOCKED = 15,
    EXPERT_LOCKED = 16,
    EXPERT_UNLOCKED = 17,
    MASTER_LOCKED = 18,
    MASTER_UNLOCKED = 19
}

public class SuccessManager : MonoBehaviour
{
    // Variables
    private int nbFuelSlotUses = 0;
    private const int maxFuelSlotUses = 30;
    private bool fullGasSuccess = false;
    private int nbPickedUpCoin = 0;
    private const int maxPickedUpCoin = 50;
    private bool mendicantSuccess = false;
    private int nbEnemiesKilled = 0;
    private const int maxEnemiesKilled = 300;
    private bool massacreSuccess = false;
    private Image[] successImages;

    public void InitializeSuccess()
    {
        successImages = GetComponentsInChildren<Image>();
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
        if (nbFuelSlotUses == maxFuelSlotUses)
        {
            fullGasSuccess = true;
            successImages[(int)Success.FULL_GAS_LOCKED].enabled = false;
            successImages[(int)Success.FULL_GAS_UNLOCKED].enabled = true;
            nbFuelSlotUses = 0;
        }
        Debug.Log("Full Gas : " + fullGasSuccess);
    }

    public void UpdateMendicantSuccess()
    {
        nbPickedUpCoin++;
        if (nbPickedUpCoin == maxPickedUpCoin)
        {
            mendicantSuccess = true;
            successImages[(int)Success.MENDICANT_LOCKED].enabled = false;
            successImages[(int)Success.MENDICANT_UNLOCKED].enabled = true;
            nbPickedUpCoin = 0;
        }
        Debug.Log("Mendicant : " + mendicantSuccess);
    }

    public void UpdateMassacreSuccess(int nbKills = 1)
    {
        nbEnemiesKilled += nbKills;
        if (nbEnemiesKilled >= maxEnemiesKilled)
        {
            massacreSuccess = true;
            successImages[(int)Success.MASSACRE_LOCKED].enabled = false;
            successImages[(int)Success.MASSACRE_UNLOCKED].enabled = true;
            nbEnemiesKilled %= maxEnemiesKilled;
        }
        Debug.Log("Massacre : " + massacreSuccess + " " + nbEnemiesKilled);
    }
}
