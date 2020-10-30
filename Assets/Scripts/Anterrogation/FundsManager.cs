using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FundsManager : NetworkBehaviour
{
    public int funds, currentCost;

    public float timer, timerSet;

    public Image filler;

    public TextMeshProUGUI fundText, currentCosts;

    public List<SecurityCam> activeCams = new List<SecurityCam>();

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

        fundText.text = "Funds: " + funds;

        foreach(SecurityCam cam in FindObjectsOfType<SecurityCam>())
        {
            if (cam.isOn && !activeCams.Contains(cam))
            {
                activeCams.Add(cam);
            }
            else if (!cam.isOn && activeCams.Contains(cam))
            {
                activeCams.Remove(cam);
            }
        }

        if(funds < 0)
        {
            RpcSetFunds(0);
        }

        switch (activeCams.Count)
        {
            case 0:
                currentCost = 0;
                currentCosts.text = "Current cost: " + currentCost;
                break;
            case 1:
                currentCost = 5;
                currentCosts.text = "Current cost: " + currentCost;
                break;
            case 2:
                currentCost = 10;
                currentCosts.text = "Current cost: " + currentCost;
                break;
            case 3:
                currentCost = 15;
                currentCosts.text = "Current cost: " + currentCost;
                break;
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdAddFunds(int newValue)
    {
        RpcAddFunds(newValue);
        //funds = funds + newValue;
    }

    [ClientRpc]
    public void RpcAddFunds(int newValue)
    {
        funds = funds + newValue;
    }

    [Command(ignoreAuthority = true)]
    public void CmdSetFunds(int newValue)
    {
        RpcSetFunds(newValue);
    }

    [ClientRpc]
    void RpcSetFunds(int newValue)
    {
        funds = newValue;
    }

    [Command(ignoreAuthority = true)]
    public void CmdRemoveFunds(int newValue)
    {
        RpcRemoveFunds(newValue);
    }

    [ClientRpc]
    public void RpcRemoveFunds(int newValue)
    {
        funds = funds - newValue;
    }
}
