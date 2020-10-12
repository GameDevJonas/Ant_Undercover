using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingGoal : MonoBehaviour
{
    public int objectsPlaced, maxObjects, sObjectsPlaced, sMaxObjects;

    public bool playerInRange, spyInRange, sabotaged;

    public GameObject canvas, spyCanvas, sabotagedCanvas, sabotagedObj, normalObj;

    public List<GameObject> objectsInPile = new List<GameObject>();

    public string need;

    public float timer, timerValue, sabotageTimer;

    void Start()
    {
        sabotaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCanvas();
        UpdatePile();
        if (sabotaged)
        {
            RepairMe();
        }
    }

    void UpdatePile()
    {
        switch (sabotaged)
        {
            case true:
                need = "Stick";
                normalObj.SetActive(false);
                sabotagedObj.SetActive(true);
                break;
            case false:
                need = "Leaf";
                normalObj.SetActive(true);
                sabotagedObj.SetActive(false);
                switch (objectsPlaced)
                {
                    case 6:
                        objectsInPile[5].SetActive(true);
                        objectsInPile[4].SetActive(true);
                        objectsInPile[3].SetActive(true);
                        objectsInPile[2].SetActive(true);
                        objectsInPile[1].SetActive(true);
                        objectsInPile[0].SetActive(true);
                        break;
                    case 5:
                        objectsInPile[5].SetActive(false);
                        objectsInPile[4].SetActive(true);
                        objectsInPile[3].SetActive(true);
                        objectsInPile[2].SetActive(true);
                        objectsInPile[1].SetActive(true);
                        objectsInPile[0].SetActive(true);
                        break;
                    case 4:
                        objectsInPile[5].SetActive(false);
                        objectsInPile[4].SetActive(false);
                        objectsInPile[3].SetActive(true);
                        objectsInPile[2].SetActive(true);
                        objectsInPile[1].SetActive(true);
                        objectsInPile[0].SetActive(true);
                        break;
                    case 3:
                        objectsInPile[5].SetActive(false);
                        objectsInPile[4].SetActive(false);
                        objectsInPile[3].SetActive(false);
                        objectsInPile[2].SetActive(true);
                        objectsInPile[1].SetActive(true);
                        objectsInPile[0].SetActive(true);
                        break;
                    case 2:
                        objectsInPile[5].SetActive(false);
                        objectsInPile[4].SetActive(false);
                        objectsInPile[3].SetActive(false);
                        objectsInPile[2].SetActive(false);
                        objectsInPile[1].SetActive(true);
                        objectsInPile[0].SetActive(true);
                        break;
                    case 1:
                        objectsInPile[5].SetActive(false);
                        objectsInPile[4].SetActive(false);
                        objectsInPile[3].SetActive(false);
                        objectsInPile[2].SetActive(false);
                        objectsInPile[1].SetActive(false);
                        objectsInPile[0].SetActive(true);
                        break;
                    case 0:
                        objectsInPile[5].SetActive(false);
                        objectsInPile[4].SetActive(false);
                        objectsInPile[3].SetActive(false);
                        objectsInPile[2].SetActive(false);
                        objectsInPile[1].SetActive(false);
                        objectsInPile[0].SetActive(false);
                        break;
                }
                break;

        }
    }

    public void Sabotager(float i)
    {
        Debug.Log(i / 5);
        spyCanvas.GetComponentInChildren<Image>().fillAmount = i / 5;
        if(i / 5 >= 1)
        {
            sabotaged = true;
            spyCanvas.GetComponentInChildren<Image>().fillAmount = 0;
        }
    }

    void RepairMe()
    {
        timer += Time.deltaTime;
        timerValue = timer / sabotageTimer;
        sabotagedCanvas.GetComponentInChildren<Image>().fillAmount = timerValue;
        if(timerValue >= 1)
        {
            sabotaged = false;
            timer = 0;
        }
    }

    void CheckForCanvas()
    {
        canvas.SetActive(playerInRange);
        spyCanvas.SetActive(spyInRange);
        sabotagedCanvas.SetActive(sabotaged);
        canvas.transform.LookAt(Camera.main.transform);
        spyCanvas.transform.LookAt(Camera.main.transform);
        sabotagedCanvas.transform.LookAt(Camera.main.transform);
    }

    public void PlaceObject()
    {
        objectsPlaced++;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTestingTask player = other.GetComponent<PlayerTestingTask>();
            if (player.holdingItem && player.holding == need && objectsPlaced < maxObjects && player.GetComponentInChildren<PlayerTeamTest>().myTeam != TeamTesting.TestingTeams.spy)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }

            if (player.GetComponentInChildren<PlayerTeamTest>().myTeam == TeamTesting.TestingTeams.spy && !sabotaged)
            {
                spyInRange = true;
                player.canSabotage = true;
            }
            else
            {
                spyInRange = false;
                player.canSabotage = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            spyInRange = false;
            other.GetComponent<PlayerTestingTask>().canSabotage = false;
        }
    }
}
