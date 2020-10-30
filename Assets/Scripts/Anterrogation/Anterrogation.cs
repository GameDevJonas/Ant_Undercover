using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Anterrogation : MonoBehaviour
{
    public List<SecurityCam> cams = new List<SecurityCam>();

    public GameObject door, policeP, otherP, myMinimapObj, myCanvas;

    public Transform policePlace, otherPlace, exitOne, exitTwo;

    public bool startAnterrogation, playersNotExited, ableToAnterrogate, activatedUI;

    public float timerSet, timer;

    public CinemachineVirtualCamera myCam;

    public Collider hitbox;

    public int funds;

    public FundsManager manager;

    void Start()
    {
        //HostOptions host;
        //foreach (HostOptions hostOptions in FindObjectsOfType<HostOptions>())
        //{
        //    if (hostOptions.isHost)
        //    {
        //        host = hostOptions;
        //        usesLeft = host.anterrogations;
        //    }
        //}
        activatedUI = false;
        myMinimapObj.SetActive(false);
        myCam.gameObject.SetActive(false);
        startAnterrogation = false;
        ableToAnterrogate = true;
        timer = timerSet;
        myCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<TeamManager>().gameStarted)
        {
            if (manager == null)
            {
                manager = FindObjectOfType<FundsManager>();
            }
            else
            {
                funds = manager.funds;
            }

            myCam.gameObject.SetActive(true);
            if (!activatedUI)
            {
                myMinimapObj.SetActive(true);
                activatedUI = true;
            }
            if (funds < 30)
            {
                ableToAnterrogate = false;
            }
            else if (!startAnterrogation)
            {
                ableToAnterrogate = true;
            }
        }
        if (startAnterrogation)
        {
            AnterrogationTimer();
        }
        myMinimapObj.GetComponent<Animator>().SetBool("IsIdle", startAnterrogation);

    }

    public void DoAnAnterrogation(GameObject policePlayer, GameObject otherPlayer)
    {
        policeP = policePlayer;
        otherP = otherPlayer;
        policePlayer.transform.position = policePlace.position;
        policePlayer.transform.rotation = policePlace.rotation;
        policePlayer.GetComponent<PlayerMovement>().enabled = false;
        policePlayer.GetComponent<CharacterController>().enabled = false;
        policePlayer.GetComponent<PlayerAnterrogationBehaviour>().ChangeCam(myCam, 11);
        otherPlayer.transform.position = otherPlace.transform.position;
        otherPlayer.transform.rotation = otherPlace.transform.rotation;
        otherPlayer.GetComponent<PlayerMovement>().enabled = false;
        otherPlayer.GetComponent<CharacterController>().enabled = false;
        otherPlayer.GetComponent<PlayerAnterrogationBehaviour>().ChangeCam(myCam, 11);
        ableToAnterrogate = false;
        manager.RpcRemoveFunds(30);
        startAnterrogation = true;
    }

    void ExitAnterrogation(bool jail)
    {
        policeP.transform.position = exitOne.position;
        policeP.transform.rotation = exitOne.rotation;
        policeP.GetComponent<PlayerMovement>().enabled = true;
        policeP.GetComponent<CharacterController>().enabled = true;
        policeP.GetComponent<PlayerAnterrogationBehaviour>().ChangeCam(myCam, 9);
        policeP.GetComponent<PlayerAnterrogationBehaviour>().playerInRange = false;
        otherP.GetComponent<PlayerMovement>().enabled = true;
        otherP.GetComponent<CharacterController>().enabled = true;
        otherP.GetComponent<PlayerAnterrogationBehaviour>().ChangeCam(myCam, 9);
        if (!jail)
        {
            otherP.transform.position = exitTwo.transform.position;
            otherP.transform.rotation = exitTwo.transform.rotation;
            ableToAnterrogate = true;
        }
    }

    void AnterrogationTimer()
    {
        if (timer <= 0)
        {
            policeP.GetComponent<PlayerAnterrogationBehaviour>().OpenUI();

        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void ThrowInJail()
    {
        manager.RpcRemoveFunds(200);
        ExitAnterrogation(true);
        timer = timerSet;
        startAnterrogation = false;
    }

    public void LetGo()
    {
        ExitAnterrogation(false);
        timer = timerSet;
        startAnterrogation = false;
    }

    public void TurnOnOffCam(SecurityCam cam)
    {
        if (!cam.isServer)
        {
            cam.CmdTurnOnOff(!cam.isOn);
            //cam.RpcTurnOnOff(!cam.isOn);
        }
        else
        {
            cam.RpcTurnOnOff(!cam.isOn);
        }
    }

    public void OpenUI()
    {
        myCanvas.SetActive(!myCanvas.activeSelf);
    }

}
