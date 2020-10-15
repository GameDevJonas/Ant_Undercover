using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingGoalVTwo : MonoBehaviour
{
    public string whatDoINeed;

    public int objectsPlaced, objectsNeeded;

    public float repairTimerMax, repairTimer, repairValue;

    public bool playerInRange, spyInRange, sabotaged, done;

    public List<GameObject> objectsInPile = new List<GameObject>();

    public enum GoalState { inProgress, done, sabotaged };
    public GoalState myState;

    //public Collider myHitBox;

    public Canvas normalCanvas, spyCanvas, sabotagedCanvas;

    public GameObject normalObj, sabotagedObj;

    void Start()
    {
        repairTimer = 0;
        objectsPlaced = 0;
        myState = GoalState.inProgress;
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
    }

    void StateUpdate()
    {
        switch (myState)
        {
            case GoalState.inProgress:
                normalObj.SetActive(true);
                sabotagedObj.SetActive(false);
                normalCanvas.gameObject.SetActive(playerInRange);
                spyCanvas.gameObject.SetActive(spyInRange);
                InProgressState();
                break;
            case GoalState.done:
                normalObj.SetActive(true);
                sabotagedObj.SetActive(false);
                normalCanvas.gameObject.SetActive(false);
                spyCanvas.gameObject.SetActive(false);
                sabotagedCanvas.gameObject.SetActive(false);
                DoneState();
                break;
            case GoalState.sabotaged:
                normalObj.SetActive(false);
                sabotagedObj.SetActive(true);
                sabotagedCanvas.gameObject.SetActive(true);
                SabotagedState();
                break;
        }
    }

    public void PlayerGiveItem(string item, string role)
    {
        switch (myState)
        {
            case GoalState.inProgress:
                if (item == whatDoINeed && role == "civillian")
                {
                    PlaceObject();
                }
                else
                {
                    return;
                }
                break;
            case GoalState.done:
                break;
            case GoalState.sabotaged:
                if (item == whatDoINeed && role == "civillian")
                {
                    ReduceTimer();
                }
                break;
        }
    }

    #region In Progress State Functions
    void InProgressState()
    {
        whatDoINeed = "Leaf";
        UpdatePile();
        if (objectsPlaced >= objectsNeeded)
        {
            myState = GoalState.done;
        }
    }

    void UpdatePile()
    {
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
    }

    void PlaceObject()
    {
        objectsPlaced++;
    }

    public void Sabotager(float i)
    {
        if (myState == GoalState.inProgress)
        {
            Debug.Log(i / 5);
            spyCanvas.GetComponentInChildren<Image>().fillAmount = i / 5;
            if (i / 5 >= 1)
            {
                sabotaged = true;
                spyCanvas.GetComponentInChildren<Image>().fillAmount = 0;
                myState = GoalState.sabotaged;
            }
        }
    }
    #endregion

    #region Done State Functions
    void DoneState()
    {
        done = true;
    }
    #endregion

    #region Sabotaged State Functions
    void SabotagedState()
    {
        whatDoINeed = "Stick";
        sabotagedCanvas.GetComponentInChildren<Image>().fillAmount = repairTimer / repairTimerMax;
        RepairMe();
    }

    void RepairMe()
    {
        if (repairTimer >= repairTimerMax)
        {
            sabotaged = false;
            myState = GoalState.inProgress;
            repairTimer = 0;
        }
        else
        {
            repairTimer += Time.deltaTime;
        }
    }

    void ReduceTimer()
    {
        repairTimer += repairValue;
    }

    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTestingTask player = other.GetComponent<PlayerTestingTask>();
            if (player.GetComponentInChildren<PlayerTeamTest>().myTeam != TeamTesting.TestingTeams.spy /*&& player.holdingItem && player.holding == whatDoINeed && objectsPlaced < objectsNeeded*/)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }

            if (player.GetComponentInChildren<PlayerTeamTest>().myTeam == TeamTesting.TestingTeams.spy /* && !sabotaged*/)
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
