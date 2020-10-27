using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Task))]
public class PileGoal : MonoBehaviour
{
    Task myTask;

    public string whatDoINeed;

    public int objectsPlaced, objectsNeeded;

    public float repairTimerMax, repairTimer, repairValue, sabotageTime;

    public bool playerInRange, spyInRange, sabotaged;

    public List<GameObject> objectsInPile = new List<GameObject>();

    public enum GoalState { inProgress, done, sabotaged };
    public GoalState myState;

    //public Collider myHitBox;

    public Canvas normalCanvas, spyCanvas, sabotagedCanvas;

    public GameObject normalObj, sabotagedObj;

    public PlayerPileTask player;

    //public float testTimer, testTimerSet;

    public ServerOnlyScript serverOnly;

    void Start()
    {
        //HostOptions host;
        //foreach (HostOptions hostOptions in FindObjectsOfType<HostOptions>())
        //{
        //    if (hostOptions.isHost)
        //    {
        //        host = hostOptions;
        //        repairTimerMax = host.repairTime;
        //        repairValue = host.repairValue;
        //        sabotageTime = host.taskSpeed + host.taskSpeed / 4;
        //    }
        //}

        myTask = GetComponent<Task>();
        myTask.spyMinimapObj.SetActive(false);
        myTask.myTaskType = Task.TaskType.pile;
        //testTimer = testTimerSet;
        repairTimer = 0;
        objectsPlaced = 0;
        myState = GoalState.inProgress;
        myTask.done = false;
        myTask.percentage = 0;
        myTask.sabotaged = sabotaged;
        if (serverOnly == null && FindObjectOfType<ServerOnlyScript>())
        {
            serverOnly = FindObjectOfType<ServerOnlyScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
        if (serverOnly == null)
        {
            if (player.myRole == "spy")
            {
                if (myState == GoalState.inProgress && player.canSabotage)
                {
                    myTask.taskUI.ShowUiObject(myTask.spyMinimapObj);
                }
                else
                {
                    myTask.taskUI.CloseUiObject(myTask.spyMinimapObj);
                }

            }
            else if (player.myRole == "police")
            {
                myTask.showOnUi = true;
            }
            else
            {
                myTask.taskUI.CloseUiObject(myTask.spyMinimapObj);
                if (player.holding == whatDoINeed)
                {
                    myTask.showOnUi = true;
                }
                else
                {
                    myTask.showOnUi = false;
                }
            }
        }


        //if (!player.canSabotage)
        //{
        //    myTask.spyMinimapObj.SetActive(false);
        //}

        /*if (FindObjectOfType<TeamManager>().gameStarted)
        {
            if (testTimer <= 0 && objectsPlaced < objectsNeeded)
            {
                PlaceObject();
                testTimer = testTimerSet;
            }
            else if (objectsPlaced < objectsNeeded)
            {
                testTimer -= Time.deltaTime;
            }
        }*/
    }

    void StateUpdate()
    {
        switch (myState)
        {
            case GoalState.inProgress:
                myTask.sabotaged = sabotaged;
                normalObj.SetActive(true);
                sabotagedObj.SetActive(false);
                //normalCanvas.gameObject.SetActive(playerInRange);
                //spyCanvas.gameObject.SetActive(spyInRange);
                InProgressState();
                break;
            case GoalState.done:
                myTask.percentage = 100;
                normalObj.SetActive(true);
                sabotagedObj.SetActive(false);
                normalCanvas.gameObject.SetActive(false);
                spyCanvas.gameObject.SetActive(false);
                sabotagedCanvas.gameObject.SetActive(false);
                DoneState();
                break;
            case GoalState.sabotaged:
                myTask.sabotaged = sabotaged;
                normalObj.SetActive(false);
                sabotagedObj.SetActive(true);
                //sabotagedCanvas.gameObject.SetActive(true);
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
        float placed = objectsPlaced;
        float needed = objectsNeeded;
        float percentage = (placed / needed) * 100;
        myTask.percentage = percentage;
        whatDoINeed = "Leaf";

        UpdatePile();
        if (objectsPlaced >= objectsNeeded)
        {
            myState = GoalState.done;
        }

        //if (player.myRole == "spy" && player.canSabotage)
        //{
        //    myTask.spyMinimapObj.SetActive(true);
        //}
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
            //Debug.Log(i / 5);
            spyCanvas.GetComponentInChildren<Image>().fillAmount = i / sabotageTime;
            if (i >= sabotageTime)
            {
                sabotaged = true;
                player.canSabotage = false;
                spyCanvas.GetComponentInChildren<Image>().fillAmount = 0;
                myState = GoalState.sabotaged;
            }
        }
    }
    #endregion

    #region Done State Functions
    void DoneState()
    {
        myTask.done = true;
        //myTask.active = false;
    }
    #endregion

    #region Sabotaged State Functions
    void SabotagedState()
    {
        whatDoINeed = "Stick";
        sabotagedCanvas.GetComponentInChildren<Image>().fillAmount = repairTimer / repairTimerMax;
        myTask.repairPercentage = (repairTimer / repairTimerMax) * 100;
        RepairMe();
    }

    void RepairMe()
    {
        if (repairTimer >= repairTimerMax)
        {
            sabotaged = false;
            //player.canSabotage = true;
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
            PlayerPileTask player = other.GetComponent<PlayerPileTask>();
            if (player.GetComponent<PlayerTeam>().myTeam != TeamManager.PlayerTeams.spy /*&& player.holdingItem && player.holding == whatDoINeed && objectsPlaced < objectsNeeded*/)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }

            if (player.GetComponent<PlayerTeam>().myTeam == TeamManager.PlayerTeams.spy /* && !sabotaged*/)
            {
                spyInRange = true;
                //player.canSabotage = true;
            }
            else
            {
                spyInRange = false;
                //player.canSabotage = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            spyInRange = false;
            //other.GetComponent<PlayerPileTask>().canSabotage = false;
        }
    }
}
