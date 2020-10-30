using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityCam : NetworkBehaviour
{
    public GameObject camView;
    public GameObject tvStatic;

    public GameObject onObject, offObject;

    public Image myButton;
    public Sprite onButton, offButton;

    public Animator myAnim;

    public bool isOn;

    public int myCost;

    public float myRate, timer;

    FundsManager funds;

    void Start()
    {
        isOn = false;
        myAnim = GetComponent<Animator>();
        timer = myRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (funds == null)
        {
            funds = FindObjectOfType<FundsManager>();
        }

        onObject.SetActive(isOn);
        offObject.SetActive(!isOn);
        camView.SetActive(isOn);
        tvStatic.SetActive(!isOn);
        if (isOn)
        {
            myButton.sprite = onButton;
            myAnim.enabled = true;
            if (timer <= 0)
            {
                funds.RpcRemoveFunds(myCost);
                timer = myRate;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            myAnim.enabled = false;
            myButton.sprite = offButton;
        }

        if (funds.funds < myRate)
        {
            RpcTurnOnOff(false);
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdTurnOnOff(bool v)
    {
        RpcTurnOnOff(v);
    }

    [ClientRpc]
    public void RpcTurnOnOff(bool v)
    {
        isOn = v;
        timer = myRate;
        if (v)
        {
            funds.RpcRemoveFunds(myCost);
        }
    }

    //public void TurnOnOff(bool v)
    //{
    //    RpcTurnOnOff(v);
    //}
}
