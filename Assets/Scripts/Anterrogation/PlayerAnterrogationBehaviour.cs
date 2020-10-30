using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
using TMPro;
using System.Threading;
using UnityEngine.UI;

public class PlayerAnterrogationBehaviour : NetworkBehaviour
{
    public CinemachineFreeLook myCam;

    public Anterrogation manager;

    public PlayerTeam player;

    public bool playerInRange, fundTaskInRange, working;

    public float fundTimer, fundTimerSet;
    public int fundValue;

    public GameObject playerInRangeOf, policeUI;

    public Collider hitbox;

    public Animator myAnim;
    public TextMeshProUGUI fundsText, costText;

    public Button jailButton;

    void Start()
    {
        working = false;
        fundTaskInRange = false;
        policeUI.SetActive(false);
        myCam = GetComponentInChildren<CinemachineFreeLook>();
        manager = FindObjectOfType<Anterrogation>();
        if (isLocalPlayer)
            hitbox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.myTeam == TeamManager.PlayerTeams.police)
        {
            if (isLocalPlayer && (manager.ableToAnterrogate && playerInRange) || fundTaskInRange)
            {
                myAnim.SetBool("IsIdle", false);
            }
            else
            {
                myAnim.SetBool("IsIdle", true);
            }

            //float sliderValue = (manager.manager.funds) / 30f;
            //Debug.Log(sliderValue);
            //myAnim.GetComponent<Image>().fillAmount = sliderValue;

            fundsText.text = manager.manager.funds + "";

            costText.text = manager.manager.currentCost + "";

            if (isLocalPlayer && Input.GetKeyDown(KeyCode.E) && playerInRange && manager.ableToAnterrogate)
            {
                Debug.Log("Started anterrogation with " + this.gameObject + " as police, and " + playerInRangeOf.gameObject + "as other player.");
                CmdCallArrogation(playerInRangeOf);
            }

            if(manager.manager.funds < 200)
            {
                jailButton.interactable = false;
            }
            else
            {
                jailButton.interactable = true;
            }

            if (isLocalPlayer && Input.GetKeyDown(KeyCode.E) && fundTaskInRange)
            {
                manager.OpenUI();
                if (!manager.myCanvas.activeSelf)
                {
                    fundTimer = 0;
                    working = false;
                    GetComponent<PlayerMovement>().enabled = true;
                    //GetComponent<PlayerMovement>().cursorVisible = false;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    fundTimer = 0;
                    working = true;
                    GetComponent<PlayerMovement>().enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    //GetComponent<PlayerMovement>().cursorVisible = true;
                }
            }

            if (isLocalPlayer && working)
            {
                manager.manager.filler.fillAmount = fundTimer / fundTimerSet;
                if (fundTimer >= fundTimerSet)
                {
                    if (isClientOnly)
                    {
                        Debug.Log("Is client");
                        manager.manager.CmdAddFunds(fundValue);
                    }
                    else
                    {
                        Debug.Log("Is server");
                        manager.manager.RpcAddFunds(fundValue);
                    }
                    fundTimer = 0;
                }
                else
                {
                    fundTimer += Time.deltaTime;
                }
            }
        }
    }

    public void OpenUI()
    {
        policeUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PoliceOptions(bool yes)
    {
        if (yes)
        {
            CmdSayYes();
        }
        else
        {
            CmdSayNo();
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    [Command]
    void CmdSayYes()
    {
        RpcSayYes();
    }
    [Command]
    void CmdSayNo()
    {
        RpcSayNo();
    }

    [ClientRpc]
    public void RpcSayYes()
    {
        manager.ThrowInJail();
        policeUI.SetActive(false);
    }
    [ClientRpc]
    public void RpcSayNo()
    {
        manager.LetGo();
        policeUI.SetActive(false);
    }


    [Command]
    void CmdCallArrogation(GameObject otherP)
    {
        RpcCallArrogation(otherP);
    }

    [ClientRpc]
    void RpcCallArrogation(GameObject otherP)
    {
        manager.DoAnAnterrogation(this.gameObject, otherP);
    }

    public void ChangeCam(CinemachineVirtualCamera newCam, int priority)
    {
        if (isLocalPlayer)
            newCam.Priority = priority;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHitbox"))
        {
            if (player.myTeam == TeamManager.PlayerTeams.police && other.gameObject.GetComponentInParent<PlayerTeam>().myTeam != TeamManager.PlayerTeams.police)
            {
                Debug.Log("Trigger");
                playerInRange = true;
                playerInRangeOf = other.gameObject.GetComponentInParent<PlayerAnterrogationBehaviour>().gameObject;
            }
        }
        else
        {
            playerInRange = false;
            playerInRangeOf = null;
        }

        if (other.CompareTag("FundTask"))
        {
            if (player.myTeam == TeamManager.PlayerTeams.police)
            {
                fundTaskInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHitbox") && player.myTeam == TeamManager.PlayerTeams.police)
        {
            playerInRange = false;
            playerInRangeOf = null;
        }

        if (other.CompareTag("FundTask"))
        {
            if (player.myTeam == TeamManager.PlayerTeams.police)
            {
                fundTaskInRange = false;
            }
        }
    }
}
