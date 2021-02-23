using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class PlayerUI : NetworkBehaviour
{
    public PlayerTeam playerParent;

    [SyncVar]
    public string myName;
    public TextMeshProUGUI nameTextUi;

    public RawImage passBack;
    public TextMeshProUGUI nameText, roleText;
    public Canvas myCanvas;

    public List<Texture> passports;
    public string[] names;
    public string[] roleName;
    public Color[] roleColors;

    public void MakeUI()
    {
        myCanvas = GetComponentInChildren<Canvas>();
        //if (!GetComponentInParent<PlayerMovement>().isLocalPlayer)
        //{
        //    myCanvas.gameObject.SetActive(false);
        //}
        //playerParent = GetComponentInParent<PlayerTeamTest>();
        switch (playerParent.myTeam)
        {
            case TeamManager.PlayerTeams.police:
                CmdMakePassPort(0);
                break;
            case TeamManager.PlayerTeams.civillian:
                CmdMakePassPort(1);
                break;
            case TeamManager.PlayerTeams.spy:
                CmdMakePassPort(2);
                break;
        }
    }

    [Command]
    void CmdMakePassPort(int role)
    {
        myName = names[Random.Range(0, names.Length)];
        //myName = names[Random.Range(0, names.Length)];
        //passBack.texture = passports[role];
        //roleText.text = roleName[role];
        //roleText.color = roleColors[role];
        //nameText.text = myName;
        //nameTextUi.text = myName;
        RpcMakePassPort(role, myName);
    }

    [ClientRpc]
    public void RpcMakePassPort(int role, string name)
    {
        myName = name;
        passBack.texture = passports[role];
        roleText.text = roleName[role];
        roleText.color = roleColors[role];
        nameText.text = myName;
        nameTextUi.text = myName;
    }
}
