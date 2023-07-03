using UnityEngine;
using System.Collections;
using Mirror;
using System;

public class PlayerEnergy : NetworkBehaviour
{

    [SyncVar(hook = nameof(UpdateUserInterface))]
    [SerializeField] int energy;
    bool isRecoveringEnergy = false;
    [SerializeField] PlayerInterface playerInterface;
    [SerializeField] int maxEnergy;
    [Tooltip("Time in seconds for the player to recover energy")]
    [SerializeField] float recoveryIntervalSeconds = 4f;
    [Tooltip("Amount of energy recovered after each recovery interval")]
    [SerializeField] int energyRecoveredPerInterval = 1;
    public int GetEnergy() { return energy; }

    public override void OnStartServer()
    {
        energy = maxEnergy;

        base.OnStartServer();
    }

    [Server]
    public bool SpendEnergy(int energySpent)
    {
        if (energy < energySpent)
        {
            Debug.Log($"..{this.name} does not have enough energy");
            return false;
        }
        else
        {
            energy = Math.Clamp(energy -= energySpent, 0, maxEnergy);

            if (!isRecoveringEnergy)
                StartCoroutine(RecoverEnergy(energyRecoveredPerInterval));

            Debug.Log($"..{this.name} spends {energySpent} energy and now has {energy}");

            return true;
        }

    }

    [Server]
    public void LoseEnergy(int energyLost)
    {
        energy -= energyLost;

        Math.Clamp(energy, 0, maxEnergy);

        if (!isRecoveringEnergy)
            StartCoroutine(RecoverEnergy(energyRecoveredPerInterval));

        Debug.Log($"..{this.name} loses {energyLost} energy and now has {energy}");
    }

    [Server]
    IEnumerator RecoverEnergy(int energyRecovered)
    {
        isRecoveringEnergy = true;
        while (energy < maxEnergy)
        {
            yield return new WaitForSeconds(recoveryIntervalSeconds);
            energy += energyRecovered;
            Math.Clamp(energy, 0, maxEnergy);
        }
        isRecoveringEnergy = false;
    }

    void UpdateUserInterface(int oldEnergyValue, int newEnergyValue)
    {
        playerInterface.SetEnergyText(newEnergyValue);
    }
}
