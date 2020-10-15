using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerTestingTask : MonoBehaviour
{
    public bool holdingItem, canPickUp, canDeliver, arrowOn, canSabotage;

    public float holdingSpeed, normalSpeed, sabotageValue;

    public Transform holdPoint;

    public GameObject objectHolding;

    public TestingPile currentPile;
    //public TestingGoal currentGoal;
    public TestingGoalVTwo currentGoal;

    public string holding, myRole;

    void Start()
    {
        holdingItem = false;
        arrowOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        myRole = GetComponentInChildren<PlayerTeamTest>().myTeam.ToString();

        PickUpRelease();
        PlaceObjectOnGoal();
        SabotageUpdate();

        if (holdingItem)
        {
            GetComponent<MovementTest>().playerSpeed = holdingSpeed;
        }
        else
        {
            GetComponent<MovementTest>().playerSpeed = normalSpeed;
        }
    }

    void SabotageUpdate()
    {
        if (canSabotage)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                sabotageValue = 0;
            }
            if (Input.GetKey(KeyCode.E))
            {
                sabotageValue += Time.deltaTime;
                currentGoal.Sabotager(sabotageValue);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                sabotageValue = 0;
                currentGoal.Sabotager(sabotageValue);
            }
        }
    }

    void PlaceObjectOnGoal()
    {
        /*
            if (holdingItem && canDeliver && Input.GetKeyDown(KeyCode.E) && currentGoal.objectsPlaced < currentGoal.maxObjects && !currentGoal.spyInRange)
            {
                holding = null;
                objectHolding.GetComponent<Rigidbody>().isKinematic = false;
                objectHolding.transform.SetParent(null);
                Destroy(objectHolding, .5f);
                objectHolding = null;
                currentGoal.PlaceObject();
                canDeliver = false;
                holdingItem = false;
            }
        */
        /*if (holdingItem && canDeliver && Input.GetKeyDown(KeyCode.E) && currentGoal.objectsPlaced < currentGoal.objectsNeeded && !currentGoal.spyInRange)
        {
            holding = null;
            objectHolding.GetComponent<Rigidbody>().isKinematic = false;
            objectHolding.transform.SetParent(null);
            Destroy(objectHolding, .5f);
            objectHolding = null;
            //currentGoal.PlaceObject();
            canDeliver = false;
            holdingItem = false;
        }
        else*/ if (holdingItem && canDeliver && Input.GetKeyDown(KeyCode.E) /*&& currentGoal.spyInRange*/)
        {
            currentGoal.PlayerGiveItem(holding, myRole);
            holding = null;
            objectHolding.GetComponent<Rigidbody>().isKinematic = false;
            objectHolding.transform.SetParent(null);
            Destroy(objectHolding, .5f);
            objectHolding = null;
            canDeliver = false;
            holdingItem = false;
        }
    }

    void PickUpRelease()
    {
        if (holdingItem && Input.GetKeyDown(KeyCode.E) && !canDeliver)
        {
            holding = null;
            objectHolding.GetComponent<Rigidbody>().isKinematic = false;
            objectHolding.transform.SetParent(null);
            Destroy(objectHolding, 5f);
            objectHolding = null;
            holdingItem = false;
        }

        if (!holdingItem && canPickUp)
        {
            if (Input.GetKeyDown(KeyCode.E) && currentPile.objectsReady > 0)
            {
                holding = currentPile.what;
                Debug.Log(gameObject.name + " picked up object from " + currentPile.name, currentPile);
                currentPile.TakeObject();
                PickUpObject(currentPile.prefab);
            }
        }
    }

    void PickUpObject(GameObject objectToHold)
    {
        currentPile = null;
        GameObject objClone = Instantiate(objectToHold, holdPoint);
        objClone.GetComponent<Rigidbody>().isKinematic = true;
        objectHolding = objClone;
        holdingItem = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<TestingPile>())
        {
            currentPile = other.gameObject.GetComponent<TestingPile>();
            if (!holdingItem)
            {
                canPickUp = true;
            }
        }
        /*
        if (other.gameObject.GetComponent<TestingGoal>())
        {
            currentGoal = other.gameObject.GetComponent<TestingGoal>();
            if (holdingItem && holding == other.gameObject.GetComponent<TestingGoal>().need && other.gameObject.GetComponent<TestingGoal>().objectsPlaced < 6 && !other.gameObject.GetComponent<TestingGoal>().sabotaged)
            {
                canDeliver = true;
            }
        }
        */
        if (other.gameObject.GetComponent<TestingGoalVTwo>())
        {
            currentGoal = other.gameObject.GetComponent<TestingGoalVTwo>();
            if (holdingItem && holding == other.gameObject.GetComponent<TestingGoalVTwo>().whatDoINeed /* && other.gameObject.GetComponent<TestingGoalVTwo>().objectsPlaced < other.gameObject.GetComponent<TestingGoalVTwo>().objectsNeeded && !other.gameObject.GetComponent<TestingGoalVTwo>().sabotaged*/)
            {
                canDeliver = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TestingPile>())
        {
            canPickUp = false;
            currentPile = null;
        }
        /*
        if (other.gameObject.GetComponent<TestingGoal>() && holdingItem)
        {
            canDeliver = false;
            currentGoal = null;
        }
        */
        if (other.gameObject.GetComponent<TestingGoalVTwo>() && holdingItem)
        {
            canDeliver = false;
            currentGoal = null;
        }
    }
}
