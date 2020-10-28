using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FundsManager : NetworkBehaviour
{
    public int funds;

    public float timer, timerSet;

    void Start()
    {
        timer = timerSet;
        funds = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<TeamManager>().gameStarted)
            return;

        if (timer <= 0)
        {
            RpcSyncFunds(1);
            timer = timerSet;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    [ClientRpc]
    void RpcSyncFunds(int newValue)
    {
        funds = funds + newValue;
    }

    [ClientRpc]
    public void RpcRemoveFunds(int newValue)
    {
        funds = funds - newValue;
    }
}
