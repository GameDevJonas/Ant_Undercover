using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerPileTask : NetworkBehaviour
{
    public bool holdingItem, canPickUp, canDeliver, arrowOn, canSabotage, sabotageReload;

    public float holdingSpeed, normalSpeed, policeSpeed, sabotageValue, reloadTimer, sabotageCooldown, sabotageTime, deliveryTime, pickupTime, deliveryPickupTimer;

    public Transform holdPoint;

    public GameObject leafObj, stickObj;

    public Pile currentPile;
    //public TestingGoal currentGoal;
    public PileGoal currentGoal;

    public string holding, myRole;

    public Animator civillianAnim, spyAnim, spyPickup;
    public Image spySabotager, workerTimeSlider, spyTimeSlider;

    void Start()
    {
        //HostOptions host;
        //foreach (HostOptions hostOptions in FindObjectsOfType<HostOptions>())
        //{
        //    if (hostOptions.isHost)
        //    {
        //        host = hostOptions;
        //        holdingSpeed = host.holdingSpeed;
        //        normalSpeed = host.normalSpeed;
        //        policeSpeed = host.policeSpeed;
        //        sabotageTime = host.taskSpeed + host.taskSpeed / 4;
        //        sabotageCooldown = host.sabotageCooldown;
        //    }
        //}

        spyPickup.SetBool("IsIdle", true);
        civillianAnim.SetBool("IsIdle", true);
        reloadTimer = 0;
        holdingItem = false;
        arrowOn = false;
        leafObj.SetActive(false);
        stickObj.SetActive(false);
        if (isLocalPlayer)
        {
            foreach (PileGoal task in FindObjectsOfType<PileGoal>())
            {
                task.player = this;
            }
        }
        canSabotage = true;

        workerTimeSlider.fillAmount = 0;

        spyTimeSlider.fillAmount = 0;
    }

    public void TurnOffAnim()
    {
        spyPickup.SetBool("IsIdle", true);
        civillianAnim.SetBool("IsIdle", true);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(FindObjectsOfType<HostOptions>().Length);
        myRole = GetComponent<PlayerTeam>().myTeam.ToString();

        if (myRole == "police")
        {
            GetComponent<PlayerMovement>().playerSpeed = policeSpeed;
            return;
        }

        PickUpRelease();
        PlaceObjectOnGoal();
        SabotageUpdate();

        if (sabotageReload)
        {
            ReloadSabotage();
        }

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
    void ReloadSabotage()
    {
        if (reloadTimer >= sabotageCooldown)
        {
            canSabotage = true;
            reloadTimer = 0;
            sabotageReload = false;
        }
        else
        {
            spyAnim.GetComponent<Image>().fillAmount = reloadTimer / sabotageCooldown;
            //Debug.Log(reloadTimer / sabotageCooldown);
            reloadTimer += Time.deltaTime;
        }
    }

    void SabotageUpdate()
    {
        if (canSabotage && isLocalPlayer && currentGoal != null && myRole == "spy")
        {
            spyAnim.SetBool("IsIdle", false);
            if (Input.GetKeyDown(KeyCode.E))
            {
                sabotageValue = 0;
                spySabotager.fillAmount = 0;
                //if (isLocalPlayer)
                CmdSabotageZero();
            }
            if (Input.GetKey(KeyCode.E))
            {
                float value = sabotageValue += Time.deltaTime;
                //if(value == 5)
                //{
                //    canSabotage = false;
                //    sabotageReload = true;
                //    spySabotager.fillAmount = 0;
                //}
                currentGoal.Sabotager(value);
                spySabotager.fillAmount = value / sabotageTime;
                CmdAddSabotage(value);
                //if (isLocalPlayer)
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                sabotageValue = 0;
                spySabotager.fillAmount = 0;
                currentGoal.Sabotager(sabotageValue);
                CmdResetSabotage();
                //if (currentGoal.myState != PileGoal.GoalState.sabotaged)
                //{
                //    canSabotage = true;
                //}
            }
        }
        else if (isLocalPlayer)
        {
            spyAnim.SetBool("IsIdle", true);
            sabotageValue = 0;
            spySabotager.fillAmount = 0;
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
        RpcResetSabotage();
        //if (currentGoal.myState != PileGoal.GoalState.sabotaged)
        //{
        //    canSabotage = true;
        //}
        //if (isLocalPlayer)
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
        if (v >= sabotageTime)
        {
            spySabotager.fillAmount = 0;
            canSabotage = false;
            sabotageReload = true;
        }
    }
    [ClientRpc]
    public void RpcResetSabotage()
    {
        sabotageValue = 0;
        currentGoal.Sabotager(0);
        //if(currentGoal.myState != PileGoal.GoalState.sabotaged)
        //{
        //    canSabotage = true;
        //}
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

        /*if (holdingItem && canDeliver && Input.GetKeyDown(KeyCode.E) && isLocalPlayer && currentGoal.spyInRange)
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
            civillianAnim.SetBool("IsIdle", true);
            //if (!isServer)
            //{
            //currentGoal.PlayerGiveItem(holding, myRole);
            //}
            //holding = null;
            //canDeliver = false;
            //holdingItem = false;
        }*/
        #endregion
        if (holdingItem && canDeliver && isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                deliveryPickupTimer = 0;

                if (myRole == "civillian")
                {
                    workerTimeSlider.fillAmount = deliveryPickupTimer / deliveryTime;
                }
            }
            if (Input.GetKey(KeyCode.E))
            {
                if (deliveryPickupTimer >= deliveryTime)
                {
                    if (holding == "Leaf")
                    {
                        CmdPutOnGoal("Leaf", myRole);
                    }
                    else if (holding == "Stick")
                    {
                        CmdPutOnGoal("Stick", myRole);
                    }
                    civillianAnim.SetBool("IsIdle", true);
                    deliveryPickupTimer = deliveryTime;

                    if (myRole == "civillian")
                    {
                        workerTimeSlider.fillAmount = 0;
                    }
                }
                else
                {
                    deliveryPickupTimer += Time.deltaTime;
                    if (myRole == "civillian")
                    {
                        workerTimeSlider.fillAmount = deliveryPickupTimer / deliveryTime;
                    }
                }

            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                deliveryPickupTimer = 0;

                if (myRole == "civillian")
                {
                    workerTimeSlider.fillAmount = 0;
                }
            }
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
        if (isLocalPlayer)
        {
            if (holdingItem)
            {
                if (!canDeliver)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        //deliveryPickupTimer = pickupTime;
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
                        spyPickup.SetBool("IsIdle", true);
                        civillianAnim.SetBool("IsIdle", true);
                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        /*if (deliveryPickupTimer <= 0)
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
                            spyPickup.SetBool("IsIdle", true);
                            civillianAnim.SetBool("IsIdle", true);
                            deliveryPickupTimer = pickupTime;
                        }
                        else
                        {
                            deliveryPickupTimer -= Time.deltaTime;
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        deliveryPickupTimer = pickupTime;
                    }*/

                        /*if (holding == "Leaf")
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
                        spyPickup.SetBool("IsIdle", true);
                        civillianAnim.SetBool("IsIdle", true);
                        //holding = null;
                        //holdingItem = false;*/
                    }
                }
            }
            else if (!holdingItem)
            {
                if (canPickUp)
                {
                    if (currentPile.objectsReady > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            deliveryPickupTimer = 0;
                            if (myRole == "civillian")
                            {
                                workerTimeSlider.fillAmount = 0;
                            }
                            else if (myRole == "spy")
                            {
                                spyTimeSlider.fillAmount = 0;
                            }
                        }
                        if (Input.GetKey(KeyCode.E))
                        {
                            if (deliveryPickupTimer >= pickupTime)
                            {
                                holding = currentPile.what;
                                if (holding == "Leaf")
                                {
                                    CmdObjectsOnOff("Leaf", true);
                                }
                                else if (holding == "Stick")
                                {
                                    CmdObjectsOnOff("Stick", true);
                                }
                                deliveryPickupTimer = pickupTime;

                                if (myRole == "civillian")
                                {
                                    workerTimeSlider.fillAmount = 0;
                                }
                                else if (myRole == "spy")
                                {
                                    spyTimeSlider.fillAmount = 0;
                                }
                            }
                            else
                            {
                                Debug.Log(deliveryTime);
                                deliveryPickupTimer += Time.deltaTime;
                                if (myRole == "civillian")
                                {
                                    workerTimeSlider.fillAmount = deliveryPickupTimer / pickupTime;
                                }
                                else if (myRole == "spy")
                                {
                                    spyTimeSlider.fillAmount = deliveryPickupTimer / pickupTime;
                                }
                            }
                        }
                        if (Input.GetKeyUp(KeyCode.E))
                        {
                            deliveryPickupTimer = 0;
                            if (myRole == "civillian")
                            {
                                workerTimeSlider.fillAmount = 0;
                            }
                            else if (myRole == "spy")
                            {
                                spyTimeSlider.fillAmount = 0;
                            }
                        }

                        /*holding = currentPile.what;
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
                        //currentPile = null;*/
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
        //if (!r)
        //{
        //    holding = null;
        //}
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
                spyPickup.SetBool("IsIdle", false);
                civillianAnim.SetBool("IsIdle", false);
            }
            else
            {
                canPickUp = false;
                spyPickup.SetBool("IsIdle", true);
                civillianAnim.SetBool("IsIdle", true);
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
                civillianAnim.SetBool("IsIdle", false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Pile>())
        {
            canPickUp = false;
            currentPile = null;
            spyPickup.SetBool("IsIdle", true);
            civillianAnim.SetBool("IsIdle", true);
            deliveryPickupTimer = pickupTime;
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
            spyPickup.SetBool("IsIdle", true);
            civillianAnim.SetBool("IsIdle", true);
            deliveryPickupTimer = deliveryTime;
        }
        else if (other.gameObject.GetComponent<PileGoal>())
        {
            currentGoal = null;
            spyPickup.SetBool("IsIdle", true);
            civillianAnim.SetBool("IsIdle", true);
            deliveryPickupTimer = deliveryTime;
        }
    }
}
