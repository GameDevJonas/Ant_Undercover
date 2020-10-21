using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Anterrogation : MonoBehaviour
{
    public GameObject door, policeP, otherP;

    public Transform policePlace, otherPlace, exitOne, exitTwo;

    public bool startAnterrogation, playersNotExited, ableToAnterrogate;

    public float timerSet, timer;

    public CinemachineVirtualCamera myCam;

    public Collider hitbox;

    void Start()
    {
        //door.SetActive(false);
        myCam.gameObject.SetActive(false);
        startAnterrogation = false;
        ableToAnterrogate = true;
        timer = timerSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<TeamManager>().gameStarted)
        {
            myCam.gameObject.SetActive(true);
        }
        if (startAnterrogation)
        {
            AnterrogationTimer();
        }
    }

    public void DoAnAnterrogation(GameObject policePlayer, GameObject otherPlayer)
    {
        policeP = policePlayer;
        otherP = otherPlayer;
        policePlayer.transform.position = policePlace.position;
        policePlayer.transform.rotation = policePlace.rotation;
        policePlayer.GetComponent<PlayerMovement>().enabled = false;
        policePlayer.GetComponent<PlayerAnterrogationBehaviour>().ChangeCam(myCam, 11);
        otherPlayer.transform.position = otherPlace.transform.position;
        otherPlayer.transform.rotation = otherPlace.transform.rotation;
        otherPlayer.GetComponent<PlayerMovement>().enabled = false;
        otherPlayer.GetComponent<PlayerAnterrogationBehaviour>().ChangeCam(myCam, 11);
        ableToAnterrogate = false;
        startAnterrogation = true;
    }

    void ExitAnterrogation()
    {
        policeP.transform.position = exitOne.position;
        policeP.transform.rotation = exitOne.rotation;
        policeP.GetComponent<PlayerMovement>().enabled = true;
        policeP.GetComponent<PlayerAnterrogationBehaviour>().ChangeCam(myCam, 9);
        otherP.transform.position = exitTwo.transform.position;
        otherP.transform.rotation = exitTwo.transform.rotation;
        otherP.GetComponent<PlayerMovement>().enabled = true;
        otherP.GetComponent<PlayerAnterrogationBehaviour>().ChangeCam(myCam, 9);
        ableToAnterrogate = true;
    }

    void AnterrogationTimer()
    {
        if(timer <= 0)
        {
            ExitAnterrogation();
            timer = timerSet;
            startAnterrogation = false;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
