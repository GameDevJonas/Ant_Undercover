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
            RpcAddFunds(1);
            timer = timerSet;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if(funds < 0)
        {
            RpcSetFunds(0);
        }
    }

    [ClientRpc]
    public void RpcAddFunds(int newValue)
    {
        funds = funds + newValue;
    }

    [ClientRpc]
    void RpcSetFunds(int newValue)
    {
        funds = newValue;
    }

    [ClientRpc]
    public void RpcRemoveFunds(int newValue)
    {
        funds = funds - newValue;
    }
}
