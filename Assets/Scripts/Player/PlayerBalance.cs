using UnityEngine;
using System.Collections;
using Mirror;
using System;

public class PlayerBalance : NetworkBehaviour
{

    [SerializeField] PlayerInterface playerInterface;
    [SyncVar(hook = nameof(UpdateUserInterface))]
    [SerializeField] int balance = 100;
    [Tooltip("Time in seconds for the player to recover balance")]
    [SerializeField] float recoveryInterval;
    [Tooltip("Amount of balance recovered after each recovery interval")]
    [SerializeField] int balanceRecoveredPerInterval;
    [Tooltip("Seconds before balance recovery begins")]
    [SerializeField] float recoveryDelay;
    int maxBalance = 100;
    float timeOfLastBalanceLoss = 0f;
    bool isRecoveringBalance = false; //this can be true even if the recovery delay has not yet been passed.  In other words, this being true does not mean the player is actively restoring balance
    public override void OnStartServer()
    {
        balance = maxBalance;

        base.OnStartServer();
    }

    [Server]
    public void LoseBalance(int balanceLost)
    {
        balance = Math.Clamp(balance -= balanceLost, 0, maxBalance);
        timeOfLastBalanceLoss = Time.time;

        if (!isRecoveringBalance)
            StartCoroutine(RecoverBalance(balanceRecoveredPerInterval));

        Debug.Log($"..{this.name} loses {balanceLost} balance and now has {balance}");
    }

    [Server]
    IEnumerator RecoverBalance(int balanceRecovered)
    {
        isRecoveringBalance = true;

        while (balance < maxBalance)
        {
            yield return new WaitForSeconds(recoveryInterval);

            if (timeOfLastBalanceLoss + recoveryDelay < Time.time)
            {
                balance += balanceRecovered;
                Math.Clamp(balance, 0, maxBalance);
            }
        }

        isRecoveringBalance = false;
    }

    void UpdateUserInterface(int oldBalanceValue, int newBalanceValue)
    {
        playerInterface.SetBalanceText(newBalanceValue);
    }

}
