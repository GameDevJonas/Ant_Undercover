using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamTesting : MonoBehaviour
{
    public enum PlayerTeams { police, civillian, spy};

    public List<MovementTest> policePlayers = new List<MovementTest>();
    public List<MovementTest> civillianPlayers = new List<MovementTest>();
    public List<MovementTest> spyPlayers = new List<MovementTest>();

    public Mesh[] teamMeshes;

    public void AddToList(PlayerTeamTest playerToAdd)
    {
        switch (playerToAdd.myTeam)
        {
            case PlayerTeams.police:
                policePlayers.Add(playerToAdd.GetComponentInParent<MovementTest>());
                break;
            case PlayerTeams.civillian:
                civillianPlayers.Add(playerToAdd.GetComponentInParent<MovementTest>());
                break;
            case PlayerTeams.spy:
                spyPlayers.Add(playerToAdd.GetComponentInParent<MovementTest>());
                break;
        }
    }
}
