using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostOptions : NetworkBehaviour
{
    [Header("Game settings")]
    [Tooltip("Round timer")] public int gameTime; //Round time
    [Tooltip("How many players for the game")] public int playerAmount; //How many players

    [Header("Player settings")]
    [Tooltip("Normal player speed")] public float normalSpeed; //Normal player speed
    [Tooltip("Player speed while holding objects")] public float holdingSpeed; //Player speed while holding objects
    [Tooltip("Police player speed")] public float policeSpeed; //Police player speed

    [Header("Ability settings")]
    [Tooltip("Total number of total anterrogations")] public int anterrogations; //Number of total anterrogations
    [Tooltip("How long the sabotage cooldown is")] public float sabotageCooldown; //How long sabotage cool timer is
    [Tooltip("How long object pickup/task deliver/sabotage time is")] public float taskSpeed; //How long object pickup/task deliver/sabotage time is
    [Tooltip("How long sabotaged objects need to repair")] public float repairTime;
    [Tooltip("How much an object repairs a sabotaged object")] public float repairValue;

    bool hasLoadedInfo;

    public GameObject hostCanvas;

    public Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.interactable = false;
        if (GetComponent<PlayerPileTask>().isServer)
        {
            hostCanvas.SetActive(true);
        }
        else
        {
            hostCanvas.SetActive(false);
        }
        //if (FindObjectsOfType<HostOptions>().Length < 2 && SceneManager.GetActiveScene().buildIndex == 0)
        //{
        //    DontDestroyOnLoad(this);
        //}
        //else
        //{
        //    Destroy(this);
        //}
        hasLoadedInfo = false;

        gameTime = 3;
        playerAmount = 2;
        normalSpeed = 5;
        holdingSpeed = 3;
        policeSpeed = 6;
        anterrogations = 2;
        sabotageCooldown = 60;
        taskSpeed = 4;
        repairTime = 60;
        repairValue = 10;

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLoadedInfo && !FindObjectOfType<TeamManager>().gameStarted)
        {
            //if (GetComponent<PlayerPileTask>().isServer)
            //{
            //    RpcLoadInfo(gameTime, playerAmount, holdingSpeed, normalSpeed, policeSpeed, sabotageCooldown, taskSpeed, anterrogations, repairTime, repairValue);
            //}

            //if (isServer)
            //RpcDoHost(gameTime, playerAmount, holdingSpeed, normalSpeed, policeSpeed, sabotageCooldown, taskSpeed, anterrogations, repairTime, repairValue);
            //hasLoadedInfo = true;
        }
        else
        {
            hostCanvas.SetActive(false);
        }

        if (FindObjectOfType<TeamManager>().playersReady.Count == FindObjectOfType<TeamManager>().playersConnected.Count - 1 && FindObjectOfType<TeamManager>().playersConnected.Count > 1)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void UpdateRoundTimer(TMP_InputField input)
    {
        int value = int.Parse(input.text);
        gameTime = value;
        //RpcUpdateRoundTimer(value);
    }

    [ClientRpc]
    void RpcUpdateRoundTimer(int value)
    {
        gameTime = value;
    }

    public void ApplySettings()
    {
        GetComponent<PlayerTeam>().HostReady();
        RpcApplySettings(gameTime, playerAmount, holdingSpeed, normalSpeed, policeSpeed, sabotageCooldown, taskSpeed, anterrogations, repairTime, repairValue);
    }

    [ClientRpc]
    void RpcApplySettings(int gT, int pA, float hS, float nS, float pS, float sC, float tS, int a, float rT, float rV)
    {
        gameTime = gT;
        playerAmount = pA;
        holdingSpeed = hS;
        normalSpeed = nS;
        policeSpeed = pS;
        sabotageCooldown = sC;
        taskSpeed = tS;
        anterrogations = a;
        repairTime = rT;
        repairValue = rV;

        FindObjectOfType<RoundManager>().gameTimerSet = gameTime;
        FindObjectOfType<TeamManager>().playerAmount = playerAmount;
        foreach (PlayerPileTask playerPiletask in FindObjectsOfType<PlayerPileTask>())
        {
            playerPiletask.holdingSpeed = holdingSpeed;
            playerPiletask.normalSpeed = normalSpeed;
            playerPiletask.policeSpeed = policeSpeed;
            playerPiletask.sabotageCooldown = sabotageCooldown;
            playerPiletask.sabotageTime = taskSpeed + taskSpeed / 4;
        }
        FindObjectOfType<Anterrogation>().usesLeft = anterrogations;
        foreach (PileGoal pileGoal in FindObjectsOfType<PileGoal>())
        {
            pileGoal.repairTimerMax = repairTime;
            pileGoal.repairValue = repairValue;
            pileGoal.sabotageTime = taskSpeed + taskSpeed / 4;
        }
    }

    [Command]
    void CmdLoadInfo(int gT, int pA, float hS, float nS, float pS, float sC, float tS, int a, float rT, float rV)
    {
        
        RpcLoadInfo(gT, pA, hS, nS, pS, sC, tS, a, rT, rV);
    }

    [ClientRpc]
    void RpcLoadInfo(int gT, int pA, float hS, float nS, float pS, float sC, float tS, int a, float rT, float rV)
    {
        //gameTime = gT;
        //playerAmount = pA;
        //holdingSpeed = hS;
        //normalSpeed = nS;
        //policeSpeed = pS;
        //sabotageCooldown = sC;
        //taskSpeed = tS;
        //anterrogations = a;
        //repairTime = rT;
        //repairValue = rV;

        FindObjectOfType<RoundManager>().gameTimerSet = gameTime;
        FindObjectOfType<TeamManager>().playerAmount = playerAmount;
        foreach (PlayerPileTask playerPiletask in FindObjectsOfType<PlayerPileTask>())
        {
            playerPiletask.holdingSpeed = holdingSpeed;
            playerPiletask.normalSpeed = normalSpeed;
            playerPiletask.policeSpeed = policeSpeed;
            playerPiletask.sabotageCooldown = sabotageCooldown;
            playerPiletask.sabotageTime = taskSpeed + taskSpeed / 4;
        }
        FindObjectOfType<Anterrogation>().usesLeft = anterrogations;
        foreach (PileGoal pileGoal in FindObjectsOfType<PileGoal>())
        {
            pileGoal.repairTimerMax = repairTime;
            pileGoal.repairValue = repairValue;
            pileGoal.sabotageTime = taskSpeed + taskSpeed / 4;
        }
    }
}
