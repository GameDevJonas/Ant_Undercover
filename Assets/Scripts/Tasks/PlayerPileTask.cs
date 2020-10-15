using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerPileTask : NetworkBehaviour
{
    public bool holdingItem, canPickUp, canDeliver, arrowOn, canSabotage;

    public float holdingSpeed, normalSpeed, sabotageValue;

    public Transform holdPoint;

    public GameObject leafObj, stickObj;

    public Pile currentPile;
    //public TestingGoal currentGoal;
    public PileGoal currentGoal;

    public string holding, myRole;

    void Start()
    {
        holdingItem = false;
        arrowOn = false;
        leafObj.SetActive(false);
        stickObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        myRole = GetComponent<PlayerTeam>().myTeam.ToString();

        if(myRole == "police")
        {
            return;
        }

        PickUpRelease();
        PlaceObjectOnGoal();
        SabotageUpdate();

        if (holdingItem)
        {
            GetComponent<PlayerMovement>().playerSpeed = holdingSpeed;
        }
        else
        {
            GetComponent<PlayerMovement>().playerSpeed = normalSpeed;
        }
    }

    #region Sabotage funcs
    void SabotageUpdate()
    {
        if (canSabotage && isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                sabotageValue = 0;
                //if (isLocalPlayer)
                CmdSabotageZero();
            }
            if (Input.GetKey(KeyCode.E))
            {
                float value = sabotageValue += Time.deltaTime;
                currentGoal.Sabotager(value);
                CmdAddSabotage(value);
                //if (isLocalPlayer)
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                sabotageValue = 0;
                currentGoal.Sabotager(sabotageValue);
                //if (isLocalPlayer)
                CmdResetSabotage();
            }
        }
    }

    [Command]
    public void CmdSabotageZero()
    {
        sabotageValue = 0;
        //if (isLocalPlayer)
        RpcSabotageZero();
    }
    [Command]
    public void CmdAddSabotage(float v)
    {
        currentGoal.Sabotager(v);
        //if (isLocalPlayer)
        RpcAddSabotage(v);
    }
    [Command]
    public void CmdResetSabotage()
    {
        sabotageValue = 0;
        currentGoal.Sabotager(0);
        //if (isLocalPlayer)
        RpcResetSabotage();
    }
    [ClientRpc]
    public void RpcSabotageZero()
    {
        sabotageValue = 0;
    }
    [ClientRpc]
    public void RpcAddSabotage(float v)
    {
        currentGoal.Sabotager(v);
    }
    [ClientRpc]
    public void RpcResetSabotage()
    {
        sabotageValue = 0;
        currentGoal.Sabotager(0);
    }
    #endregion

    void PlaceObjectOnGoal()
    {
        #region Old
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
        else*/
        #endregion

        if (holdingItem && canDeliver && Input.GetKeyDown(KeyCode.E) && isLocalPlayer /*&& currentGoal.spyInRange*/)
        {
            if (holding == "Leaf")
            {
                CmdPutOnGoal("Leaf", myRole);
                //if (isServer)
                //RpcPutOnGoal("Leaf", currentGoal.gameObject, myRole);
                //leafObj.SetActive(false);
            }
            else if (holding == "Stick")
            {
                CmdPutOnGoal("Stick", myRole);
                //if (isServer)
                //    //RpcPutOnGoal("Stick", currentGoal.gameObject, myRole);
                //stickObj.SetActive(false);
            }
            //if (!isServer)
            //{
                //currentGoal.PlayerGiveItem(holding, myRole);
            //}
            //holding = null;
            //canDeliver = false;
            //holdingItem = false;
        }
    }

    [Command]
    public void CmdPutOnGoal(string item, string role)
    {
        //goal.GetComponent<PileGoal>().PlayerGiveItem(item, role);
        //if (item == "Leaf")
        //{
        //    leafObj.SetActive(false);
        //}
        //else if (item == "Stick")
        //{
        //    stickObj.SetActive(false);
        //}
        RpcPutOnGoal(item, role);
    }
    [ClientRpc]
    public void RpcPutOnGoal(string item, string role)
    {
        if (item == "Leaf")
        {
            leafObj.SetActive(false);
        }
        else if (item == "Stick")
        {
            stickObj.SetActive(false);
        }
        currentGoal.PlayerGiveItem(item, role);
        holding = null;
        canDeliver = false;
        holdingItem = false;
        currentGoal = null;
    }

    #region Pickup and release NOT ON GOAL
    void PickUpRelease()
    {
        if (Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            if (holdingItem)
            {
                if (!canDeliver)
                {
                    if (holding == "Leaf")
                    {
                        //leafObj.SetActive(false);
                        CmdObjectsOnOff("Leaf", false);
                        //if (isServer)
                        //RpcObjectsOnOff("Leaf", false);
                    }
                    else if (holding == "Stick")
                    {
                        //stickObj.SetActive(false);
                        CmdObjectsOnOff("Stick", false);
                        //if (isServer)
                        //RpcObjectsOnOff("Stick", false);
                    }
                    //holding = null;
                    //holdingItem = false;
                }
            }
            else if (!holdingItem)
            {
                if (canPickUp)
                {
                    if (currentPile.objectsReady > 0)
                    {
                        holding = currentPile.what;
                        if (holding == "Leaf")
                        {
                            //leafObj.SetActive(true);
                            CmdObjectsOnOff("Leaf", true);
                            //if (isServer)
                            //    RpcObjectsOnOff("Leaf", true);
                        }
                        else if (holding == "Stick")
                        {
                            //stickObj.SetActive(true);
                            CmdObjectsOnOff("Stick", true);
                            //if (isServer)
                            //    RpcObjectsOnOff("Stick", true);
                        }
                        //currentPile.TakeObject();
                        //holdingItem = true;
                        //currentPile = null;
                    }
                }
            }
        }
    }

    [Command]
    public void CmdObjectsOnOff(string item, bool r)
    {
        //holding = item;
        //holdingItem = r;
        RpcObjectsOnOff(item, r);
        //if (newItem == "Leaf")
        //{
        //    leafObj.SetActive(r);
        //}
        //else if (newItem == "Stick")
        //{
        //    stickObj.SetActive(r);
        //}
        //if(!isServer)
        //currentPile.TakeObject();
        //currentPile = null;
    }

    [ClientRpc]
    public void RpcObjectsOnOff(string item, bool r)
    {
        holding = item;
        if (item == "Leaf")
        {
            leafObj.SetActive(r);
        }
        else if (item == "Stick")
        {
            stickObj.SetActive(r);
        }
        if (r == true)
        {
            currentPile.TakeObject();
        }
        //if(!isServer)
        holdingItem = r;
        currentPile = null;
        //if (r == true)
        //{
        //    holdingItem = true;
        //}
        //else
        //{
        //    Invoke("PickUpTimer", .5f);
        //}
    }
    #endregion

    /*void PickUpObject(GameObject objectToHold)
    {
        currentPile = null;
        GameObject objClone = Instantiate(objectToHold, holdPoint);
        objClone.GetComponent<Rigidbody>().isKinematic = true;
        objectHolding = objClone;
        holdingItem = true;
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Pile>())
        {
            currentPile = other.gameObject.GetComponent<Pile>();
            if (!holdingItem)
            {
                canPickUp = true;
            }
            else
            {
                canPickUp = false;
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
        if (other.gameObject.GetComponent<PileGoal>())
        {
            currentGoal = other.gameObject.GetComponent<PileGoal>();
            if (holdingItem && holding == other.gameObject.GetComponent<PileGoal>().whatDoINeed /* && other.gameObject.GetComponent<TestingGoalVTwo>().objectsPlaced < other.gameObject.GetComponent<TestingGoalVTwo>().objectsNeeded && !other.gameObject.GetComponent<TestingGoalVTwo>().sabotaged*/)
            {
                canDeliver = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Pile>())
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
        if (other.gameObject.GetComponent<PileGoal>() && holdingItem)
        {
            canDeliver = false;
            currentGoal = null;
        }
    }
}
