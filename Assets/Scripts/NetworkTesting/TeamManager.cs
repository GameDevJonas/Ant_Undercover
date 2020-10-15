using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public enum PlayerTeams { police, civillian, spy };

    public List<PlayerMovement> playersConnected = new List<PlayerMovement>();
    public List<PlayerTeam> playersReady = new List<PlayerTeam>();

    public List<GameObject> policePlayers = new List<GameObject>();
    public List<GameObject> civillianPlayers = new List<GameObject>();
    public List<GameObject> spyPlayers = new List<GameObject>();

    public Mesh[] teamMeshes;

    public bool gameStarted, pickedRole;

    public int role;

    private void Start()
    {
        gameStarted = false;
        pickedRole = false;
    }

    public void AddToList(PlayerTeam playerToAdd)
    {
        switch (playerToAdd.myTeam)
        {
            case PlayerTeams.police:
                policePlayers.Add(playerToAdd.gameObject);
                break;
            case PlayerTeams.civillian:
                civillianPlayers.Add(playerToAdd.gameObject);
                break;
            case PlayerTeams.spy:
                spyPlayers.Add(playerToAdd.gameObject);
                break;
        }
    }

    public void Update()
    {
        CheckForReadyPlayers();
        CheckForNullRefs();

        if (!gameStarted && playersConnected.Count == playersReady.Count && playersConnected.Count > 0)
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SyncAllClients();
        }
    }

    public void CheckForNullRefs()
    {
        foreach (GameObject player in policePlayers)
        {
            if (player == null)
                policePlayers.Remove(player);
        }
        foreach (GameObject player in civillianPlayers)
        {
            if (player == null)
                civillianPlayers.Remove(player);
        }
        foreach (GameObject player in spyPlayers)
        {
            if (player == null)
                spyPlayers.Remove(player);
        }
    }

    void SyncAllClients()
    {
        Debug.Log("Synced all clients");
        foreach (PlayerMovement player in playersConnected)
        {
            player.GetComponent<PlayerTeam>().SyncAll();
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

        if (playersReady.Count < 1)
        {
            gameStarted = false;
        }
    }

    public void RoleButton(int roleNum)
    {
        role = roleNum;
        pickedRole = true;
    }

    void StartGame()
    {
        Debug.Log("Game started");
        foreach (PlayerTeam player in playersReady)
        {
            if (!pickedRole)
            {
                role = Random.Range(0, 3);
            }
            switch (role)
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
        Invoke("SyncAllClients", 3f);
        gameStarted = true;
    }
}
