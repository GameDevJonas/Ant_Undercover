using Cinemachine;
using Mirror;
using System;
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

    public bool cursorException;

    public GameObject hostCanvas;

    public Button startButton;

    public List<TMP_InputField> settings = new List<TMP_InputField>();

    public Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        cursorException = false;
        //    gameTime = 3;
        //    playerAmount = 2;
        //    normalSpeed = 5;
        //    holdingSpeed = 3;
        //    policeSpeed = 6;
        //    anterrogations = 2;
        //    sabotageCooldown = 60;
        //    taskSpeed = 4;
        //    repairTime = 60;
        //    repairValue = 10;

        if (GetComponent<PlayerPileTask>().isServer)
        {
            hostCanvas.SetActive(true);
            myAnim.SetBool("FullSettings", false);
            settings[0].text = gameTime + "";
            settings[1].text = playerAmount + "";
            settings[2].text = normalSpeed + "";
            settings[3].text = holdingSpeed + "";
            settings[4].text = policeSpeed + "";
            settings[5].text = anterrogations + "";
            settings[6].text = sabotageCooldown + "";
            settings[7].text = taskSpeed + "";
            settings[8].text = repairTime + "";
            settings[9].text = repairValue + "";
        }
        else
        {
            hostCanvas.SetActive(false);
        }

        //hasLoadedInfo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<TeamManager>().testing || (FindObjectOfType<TeamManager>().playersReady.Count == FindObjectOfType<TeamManager>().playersConnected.Count - 1 && FindObjectOfType<TeamManager>().playersConnected.Count > 1))
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }

        if (!FindObjectOfType<TeamManager>().gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && isLocalPlayer)
            {
                myAnim.SetBool("FullSettings", !myAnim.GetBool("FullSettings"));
                if (myAnim.GetBool("FullSettings"))
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    GetComponentInChildren<CinemachineFreeLook>().enabled = false;
                    cursorException = false;
                }
                else if (!myAnim.GetBool("FullSettings") && !cursorException)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    GetComponentInChildren<CinemachineFreeLook>().enabled = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) && isLocalPlayer)
            {
                if (!myAnim.GetBool("FullSettings"))
                {
                    if (Cursor.visible)
                    {
                        cursorException = false;
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                    else if(!Cursor.visible)
                    {
                        cursorException = true;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
            }
        }
    }

    public void UpdateRoundTimer(TMP_InputField input)
    {
        int value = int.Parse(input.text);
        gameTime = value;
    }
    public void UpdatePlayerAmount(TMP_InputField input)
    {
        int value = int.Parse(input.text);
        playerAmount = value;
    }
    public void UpdateNormalSpeed(TMP_InputField input)
    {
        float value = float.Parse(input.text);
        normalSpeed = value;
    }
    public void UpdateHoldingSpeed(TMP_InputField input)
    {
        float value = float.Parse(input.text);
        holdingSpeed = value;
    }
    public void UpdatePoliceSpeed(TMP_InputField input)
    {
        float value = float.Parse(input.text);
        policeSpeed = value;
    }
    public void UpdateAnterrogations(TMP_InputField input)
    {
        int value = int.Parse(input.text);
        anterrogations = value;
    }
    public void UpdateSabotageCooldown(TMP_InputField input)
    {
        float value = float.Parse(input.text);
        sabotageCooldown = value;
    }
    public void UpdateTaskSpeed(TMP_InputField input)
    {
        float value = float.Parse(input.text);
        taskSpeed = value;
    }
    public void UpdateRepairTime(TMP_InputField input)
    {
        float value = float.Parse(input.text);
        repairTime = value;
    }
    public void UpdateRepairValue(TMP_InputField input)
    {
        float value = float.Parse(input.text);
        repairValue = value;
    }


    public void ApplySettings()
    {
        GetComponentInChildren<CinemachineFreeLook>().enabled = true;
        GetComponent<PlayerTeam>().HostReady();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
            playerPiletask.deliveryTime = taskSpeed;
            playerPiletask.pickupTime = taskSpeed - taskSpeed / 4;
        }
        FindObjectOfType<Anterrogation>().manager.funds = anterrogations;
        foreach (PileGoal pileGoal in FindObjectsOfType<PileGoal>())
        {
            pileGoal.repairTimerMax = repairTime;
            pileGoal.repairValue = repairValue;
            pileGoal.sabotageTime = taskSpeed + taskSpeed / 4;
        }
    }

    #region Not used
    [ClientRpc]
    void RpcUpdateRoundTimer(int value)
    {
        gameTime = value;
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
        FindObjectOfType<Anterrogation>().manager.funds = anterrogations;
        foreach (PileGoal pileGoal in FindObjectsOfType<PileGoal>())
        {
            pileGoal.repairTimerMax = repairTime;
            pileGoal.repairValue = repairValue;
            pileGoal.sabotageTime = taskSpeed + taskSpeed / 4;
        }
    }
    #endregion
}
