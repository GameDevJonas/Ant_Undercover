using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public enum PlayerTeams { police, civillian, spy };

    public List<PlayerMovement> playersConnected = new List<PlayerMovement>();
    public List<PlayerTeam> playersReady = new List<PlayerTeam>();

    public List<MovementTest> policePlayers = new List<MovementTest>();
    public List<MovementTest> civillianPlayers = new List<MovementTest>();
    public List<MovementTest> spyPlayers = new List<MovementTest>();

    public Mesh[] teamMeshes;

    public bool gameStarted;

    private void Start()
    {
        gameStarted = false;
    }

    public void AddToList(PlayerTeam playerToAdd)
    {
        switch (playerToAdd.myTeam)
        {
            case PlayerTeams.police:
                policePlayers.Add(playerToAdd.GetComponent<MovementTest>());
                break;
            case PlayerTeams.civillian:
                civillianPlayers.Add(playerToAdd.GetComponent<MovementTest>());
                break;
            case PlayerTeams.spy:
                spyPlayers.Add(playerToAdd.GetComponent<MovementTest>());
                break;
        }
    }

    private void Update()
    {
        CheckForReadyPlayers();

        if(!gameStarted && playersConnected.Count == playersReady.Count && playersConnected.Count > 0)
        {
            StartGame();
        }
    }

    void CheckForReadyPlayers()
    {
        foreach (PlayerMovement player in playersConnected)
        {
            if (player.GetComponent<PlayerTeam>().isReady)
            {
                if (!playersReady.Contains(player.GetComponent<PlayerTeam>()))
                {
                    playersReady.Add(player.GetComponent<PlayerTeam>());
                }
            }
        }

        if(playersReady.Count < 1)
        {
            gameStarted = false;
        }
    }

    void StartGame()
    {
        Debug.Log("Game started");
        foreach(PlayerTeam player in playersReady)
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    player.PickPolice();
                    break;
                case 1:
                    player.PickWorker();
                    break;
                case 2:
                    player.PickSpy();
                    break;
            }
        }
        gameStarted = true;
    }
}
