using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;

public class RolePicker : NetworkBehaviour
{
    public Anterrogation anterrogationManager;

    public List<PlayerTeam> unassignedPlayers = new List<PlayerTeam>();
    public List<PlayerTeam> playerRoles = new List<PlayerTeam>();

    public TextMeshProUGUI debugText;

    public bool allPlayersInList;
    void Start()
    {
        allPlayersInList = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (allPlayersInList)
        {
            AssignRoles();
        }
    }

    public void PickRoles()
    {
        int num = Random.Range(0, unassignedPlayers.Count);
        Debug.Log(num);
        playerRoles.Add(unassignedPlayers[num]);
        unassignedPlayers.Remove(unassignedPlayers[num]);
        if (unassignedPlayers.Count <= 0)
        {
            allPlayersInList = true;
        }
    }

    void AssignRoles()
    {
        GameObject player;
        int role;
        foreach (PlayerTeam p in playerRoles)
        {
            switch (playerRoles.IndexOf(p))
            {
                case 0: //Police
                    player = playerRoles[0].gameObject;
                    role = 0;
                    //player.GetComponent<PlayerTeam>().PickPolice();
                    //if (player.GetComponent<PlayerTeam>().isLocalPlayer)
                        //debugText.text = "police";
                    RpcPickRoles(role, player);
                    break;
                case 1: //Worker
                    player = playerRoles[1].gameObject;
                    role = 0;
                    RpcPickRoles(role, player);
                    break;
                case 2: //Spy
                    player = playerRoles[2].gameObject;
                    role = 2;
                    RpcPickRoles(role, player);
                    break;
                case 3: //Worker
                    player = playerRoles[3].gameObject;
                    role = 1;
                    RpcPickRoles(role, player);
                    break;
                case 4: //Worker
                    player = playerRoles[4].gameObject;
                    role = 1;
                    RpcPickRoles(role, player);
                    break;
                case 5: //Worker
                    player = playerRoles[5].gameObject;
                    role = 1;
                    RpcPickRoles(role, player);
                    break;
                case 6: //Spy
                    player = playerRoles[6].gameObject;
                    role = 2;
                    RpcPickRoles(role, player);
                    break;
                case 7: //Worker
                    player = playerRoles[7].gameObject;
                    role = 1;
                    RpcPickRoles(role, player);
                    break;
                case 8: //Police
                    player = playerRoles[8].gameObject;
                    role = 0;
                    RpcPickRoles(role, player);
                    break;
                case 9: //Worker
                    player = playerRoles[9].gameObject;
                    role = 1;
                    RpcPickRoles(role, player);
                    break;
            }
        }
        allPlayersInList = false;
        FindObjectOfType<TeamManager>().ContinueStart();
    }

    [Command]
    public void CmdPickRoles(int role, GameObject player)
    {
        RpcPickRoles(role, player);
    }

    [ClientRpc]
    public void RpcPickRoles(int role, GameObject player)
    {
        if (role == 0)
        {
            player.GetComponent<PlayerTeam>().PickPolice();
            //debugText.text = "police";
        }
        else if (role == 1)
        {
            player.GetComponent<PlayerTeam>().PickWorker();
            //debugText.text = "worker";
        }
        else if(role == 2)
        {
            player.GetComponent<PlayerTeam>().PickSpy();
            //debugText.text = "spy";
        }
    }
}
