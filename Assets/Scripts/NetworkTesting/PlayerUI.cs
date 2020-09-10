using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class PlayerUI : NetworkBehaviour
{
    public PlayerTeam playerParent;

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
                MakePassPort(0);
                break;
            case TeamManager.PlayerTeams.civillian:
                MakePassPort(1);
                break;
            case TeamManager.PlayerTeams.spy:
                MakePassPort(2);
                break;
        }
    }

    void MakePassPort(int role)
    {
        passBack.texture = passports[role];
        roleText.text = roleName[role];
        roleText.color = roleColors[role];
        nameText.text = names[Random.Range(0, names.Length)];
    }
}
