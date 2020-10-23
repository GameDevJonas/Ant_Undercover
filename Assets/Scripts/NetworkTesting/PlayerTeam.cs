using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class PlayerTeam : NetworkBehaviour
{
    public bool isTestPlayer;

    TeamManager manager;

    [SyncVar]
    public TeamManager.PlayerTeams myTeam;

    public PlayerUI ui;

    public GameObject MAURMustache;

    MeshFilter mesh;

    public Camera myCam;

    public LayerMask normalMask, spyMask;

    [SyncVar]
    public bool isReady;

    public TextMeshProUGUI leaveButtonText;

    public GameObject policeSprite, civillianSprite, spySprite, policeUI, civillianUI, spyUI, readyText;


    private void Awake()
    {
        readyText.SetActive(true);
        readyText.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        civillianSprite.SetActive(false);
        policeSprite.SetActive(false);
        spySprite.SetActive(false);
        civillianUI.SetActive(false);
        policeUI.SetActive(false);
        spyUI.SetActive(false);
        if (!isTestPlayer)
        {
            isReady = false;
        }
        FindObjectOfType<TeamManager>().playersConnected.Add(GetComponentInParent<PlayerMovement>());
        FindObjectOfType<RolePicker>().unassignedPlayers.Add(this);
        MAURMustache.SetActive(false);
        //myCam = Camera.main;
        mesh = GetComponentInChildren<MeshFilter>();
        manager = FindObjectOfType<TeamManager>();
        mesh.mesh = manager.teamMeshes[1];
    }

    private void Start()
    {
        if (isServer)
        {
            leaveButtonText.text = "Stop Host";
        }
        else
        {
            leaveButtonText.text = "Disconnect";
        }

        //CmdSetMarker(isLocalPlayer);


    }

    public void LoadMenu()
    {
        if (isServer)
        {
            FindObjectOfType<NetworkManager>().StopHost();
        }
    }

    [Command]
    public void CmdSetMarker(bool local)
    {
        RpcSetMarker(local);
    }

    [ClientRpc]
    public void RpcSetMarker(bool local)
    {
        if (local)
        {
            //localMarker.SetActive(true);
            //notLocalMarker.SetActive(false);
        }
        else
        {
            //notLocalMarker.SetActive(true);
            //localMarker.SetActive(false);
        }
    }

    /*public void PickRole()
    {
        int role = Random.Range(0, 3);
        if (role == 0)
        {
            MAURMustache.SetActive(false);
            myTeam = TeamManager.PlayerTeams.police;
            mesh.mesh = manager.teamMeshes[0];
            myCam.cullingMask = normalMask;
        }
        else if (role == 1)
        {
            MAURMustache.SetActive(false);
            myTeam = TeamManager.PlayerTeams.civillian;
            mesh.mesh = manager.teamMeshes[1];
            myCam.cullingMask = normalMask;
        }
        else
        {
            myTeam = TeamManager.PlayerTeams.spy;
            mesh.mesh = manager.teamMeshes[1];
            myCam.cullingMask = spyMask;
            MAURMustache.SetActive(true);
        }
        SyncRoleWithServer(myTeam);
        ui.MakeUI();
    }*/

    public void LeaveGame()
    {
        if (isServer)
        {
            FindObjectOfType<NetworkManager>().StopHost();
        }
        else
        {
            FindObjectOfType<NetworkManager>().StopClient();
        }
    }

    [Command]
    void CmdSyncRoleWithServer(TeamManager.PlayerTeams role)
    {
        if (role == TeamManager.PlayerTeams.police)
        {
            MAURMustache.SetActive(false);
            myTeam = role;
            mesh.mesh = manager.teamMeshes[0];
            myCam.cullingMask = normalMask;
        }
        else if (role == TeamManager.PlayerTeams.civillian)
        {
            MAURMustache.SetActive(false);
            myTeam = role;
            mesh.mesh = manager.teamMeshes[1];
            myCam.cullingMask = normalMask;
        }
        else
        {
            myTeam = role;
            mesh.mesh = manager.teamMeshes[1];
            myCam.cullingMask = spyMask;
            MAURMustache.SetActive(true);
        }
        RpcSyncRoleWithClient(role);
        //manager.AddToList(this);
    }

    [ClientRpc]
    void RpcSyncRoleWithClient(TeamManager.PlayerTeams role)
    {
        if (role == TeamManager.PlayerTeams.police)
        {
            MAURMustache.SetActive(false);
            myTeam = role;
            mesh.mesh = manager.teamMeshes[0];
            myCam.cullingMask = normalMask;
        }
        else if (role == TeamManager.PlayerTeams.civillian)
        {
            MAURMustache.SetActive(false);
            myTeam = role;
            mesh.mesh = manager.teamMeshes[1];
            myCam.cullingMask = normalMask;
        }
        else
        {
            myTeam = role;
            mesh.mesh = manager.teamMeshes[1];
            myCam.cullingMask = spyMask;
            MAURMustache.SetActive(true);
        }
        manager.AddToList(this);



    }

    void GetStarterTasks()
    {
        FindObjectOfType<TaskUI>().GetStarterTasks(myTeam.ToString());
    }

    public void SyncAll()
    {
        if (isLocalPlayer)
        {
            //CmdSyncRoleWithServer(myTeam);
            Invoke("GetStarterTasks", .5f);

        }
        GetComponent<PlayerAnterrogationBehaviour>().player = this;
        if (isLocalPlayer)
        {
            if (myTeam == TeamManager.PlayerTeams.civillian)
            {
                foreach (GameObject player in manager.civillianPlayers)
                {
                    if (player != this)
                    {
                        player.GetComponent<PlayerTeam>().civillianSprite.SetActive(false);
                    }
                }
                foreach (GameObject player in manager.policePlayers)
                {
                    player.GetComponent<PlayerTeam>().policeSprite.SetActive(true);
                }
                foreach (GameObject player in manager.spyPlayers)
                {
                    player.GetComponent<PlayerTeam>().spySprite.SetActive(false);
                }
                civillianSprite.SetActive(true);
                policeSprite.SetActive(false);
                spySprite.SetActive(false);
            }
            else if (myTeam == TeamManager.PlayerTeams.spy)
            {
                foreach (GameObject player in manager.civillianPlayers)
                {
                    player.GetComponent<PlayerTeam>().civillianSprite.SetActive(false);
                }
                foreach (GameObject player in manager.policePlayers)
                {
                    player.GetComponent<PlayerTeam>().policeSprite.SetActive(true);
                }
                foreach (GameObject player in manager.spyPlayers)
                {
                    if (player != this)
                    {
                        player.GetComponent<PlayerTeam>().spySprite.SetActive(true);
                    }
                }
                civillianSprite.SetActive(true);
                policeSprite.SetActive(false);
                spySprite.SetActive(false);
            }
            else if (myTeam == TeamManager.PlayerTeams.police)
            {
                foreach (GameObject player in manager.civillianPlayers)
                {
                    player.GetComponent<PlayerTeam>().civillianSprite.SetActive(false);
                }
                foreach (GameObject player in manager.policePlayers)
                {
                    if (player != this)
                    {
                        player.GetComponent<PlayerTeam>().policeSprite.SetActive(true);
                    }
                }
                foreach (GameObject player in manager.spyPlayers)
                {
                    player.GetComponent<PlayerTeam>().spySprite.SetActive(false);
                }
                civillianSprite.SetActive(true);
                policeSprite.SetActive(false);
                spySprite.SetActive(false);
            }
        }

        /*if (isLocalPlayer)
        {
            if (myTeam == TeamManager.PlayerTeams.civillian)
            {
                civillianSprite.SetActive(true);
            }
            else if (myTeam == TeamManager.PlayerTeams.spy)
            {
                civillianSprite.SetActive(true);
            }
            else if (myTeam == TeamManager.PlayerTeams.police)
            {
                civillianSprite.SetActive(true);
            }
        }
        else
        {
            if (myTeam == TeamManager.PlayerTeams.civillian)
            {
                foreach (PlayerTeam player in FindObjectsOfType<PlayerTeam>())
                {
                    if (player.myTeam == TeamManager.PlayerTeams.police)
                    {
                        player.policeSprite.SetActive(true);
                    }
                }
            }
            else if (myTeam == TeamManager.PlayerTeams.spy)
            {
                foreach (PlayerTeam player in FindObjectsOfType<PlayerTeam>())
                {
                    if (player.myTeam == TeamManager.PlayerTeams.spy)
                    {
                        player.spySprite.SetActive(true);
                        player.policeSprite.SetActive(true);
                    }
                }
            }
            else if (myTeam == TeamManager.PlayerTeams.police)
            {
                foreach (PlayerTeam player in FindObjectsOfType<PlayerTeam>())
                {
                    if (player.myTeam == TeamManager.PlayerTeams.police)
                    {
                        player.policeSprite.SetActive(true);
                    }
                }
            }
        }*/
        //CmdMarkerSync();
    }

    [Command]
    void CmdMarkerSync()
    {
        RpcMarkerSync();
    }
    [ClientRpc]
    void RpcMarkerSync()
    {
        if (isLocalPlayer)
        {
            if (myTeam == TeamManager.PlayerTeams.civillian)
            {
                civillianSprite.SetActive(true);
                foreach (GameObject player in manager.civillianPlayers)
                {
                    if (player != this)
                    {
                        player.GetComponent<PlayerTeam>().civillianSprite.SetActive(false);
                    }
                }
                foreach (GameObject player in manager.policePlayers)
                {
                    player.GetComponent<PlayerTeam>().policeSprite.SetActive(true);
                }
                foreach (GameObject player in manager.spyPlayers)
                {
                    player.GetComponent<PlayerTeam>().spySprite.SetActive(false);
                }
            }
            else if (myTeam == TeamManager.PlayerTeams.spy)
            {
                civillianSprite.SetActive(true);
                foreach (GameObject player in manager.civillianPlayers)
                {
                    player.GetComponent<PlayerTeam>().civillianSprite.SetActive(false);
                }
                foreach (GameObject player in manager.policePlayers)
                {
                    player.GetComponent<PlayerTeam>().policeSprite.SetActive(true);
                }
                foreach (GameObject player in manager.spyPlayers)
                {
                    if (player != this)
                    {
                        player.GetComponent<PlayerTeam>().spySprite.SetActive(true);
                    }
                }
            }
            else if (myTeam == TeamManager.PlayerTeams.police)
            {
                civillianSprite.SetActive(true);
                foreach (GameObject player in manager.civillianPlayers)
                {
                    player.GetComponent<PlayerTeam>().civillianSprite.SetActive(false);
                }
                foreach (GameObject player in manager.policePlayers)
                {
                    if (player != this)
                    {
                        player.GetComponent<PlayerTeam>().policeSprite.SetActive(true);
                    }
                }
                foreach (GameObject player in manager.spyPlayers)
                {
                    player.GetComponent<PlayerTeam>().spySprite.SetActive(false);
                }
            }
        }
        //else
        //{
        /*if (myTeam == TeamManager.PlayerTeams.civillian)
        {
        }
            foreach (PlayerTeam player in FindObjectsOfType<PlayerTeam>())
            {
                    if (player.myTeam == TeamManager.PlayerTeams.police && player != this)
                    {
                        player.policeSprite.SetActive(true);
                    }
            }
        else if (myTeam == TeamManager.PlayerTeams.spy)
        {
            foreach (PlayerTeam player in FindObjectsOfType<PlayerTeam>())
            {
                if (player.myTeam == TeamManager.PlayerTeams.spy && player != this)
                {
                    player.spySprite.SetActive(true);
                    player.policeSprite.SetActive(true);
                }
            }
        }
        else if (myTeam == TeamManager.PlayerTeams.police)
        {
            foreach (PlayerTeam player in FindObjectsOfType<PlayerTeam>())
            {
                if (player.myTeam == TeamManager.PlayerTeams.police && player != this)
                {
                    player.policeSprite.SetActive(true);
                }
            }
        }*/
        //}
    }

    public override void OnStopClient()
    {
        //manager.CheckForNullRefs();
        manager.playersReady.Remove(this);
    }

    public override void OnStopServer()
    {
        //manager.CheckForNullRefs();
        manager.playersReady.Remove(this);
    }

    public void PickPolice()
    {
        readyText.SetActive(false);
        MAURMustache.SetActive(false);
        myTeam = TeamManager.PlayerTeams.police;
        mesh.mesh = manager.teamMeshes[0];
        RpcSyncRoleWithClient(myTeam);
        if (isLocalPlayer)
        {
            //CmdSyncRoleWithServer(myTeam);
        }
        ui.MakeUI();
        myCam.cullingMask = normalMask;
        civillianUI.SetActive(false);
        policeUI.SetActive(true);
        spyUI.SetActive(false);
        GetComponent<PlayerPileTask>().TurnOffAnim();
    }

    public void PickWorker()
    {
        readyText.SetActive(false);
        MAURMustache.SetActive(false);
        myTeam = TeamManager.PlayerTeams.civillian;
        mesh.mesh = manager.teamMeshes[1];
        RpcSyncRoleWithClient(myTeam);
        if (isLocalPlayer)
        {
            //CmdSyncRoleWithServer(myTeam);
        }
        ui.MakeUI();
        myCam.cullingMask = normalMask;
        civillianUI.SetActive(true);
        policeUI.SetActive(false);
        spyUI.SetActive(false);
        GetComponent<PlayerPileTask>().TurnOffAnim();
    }

    public void PickSpy()
    {
        readyText.SetActive(false);
        myTeam = TeamManager.PlayerTeams.spy;
        mesh.mesh = manager.teamMeshes[1];
        RpcSyncRoleWithClient(myTeam);
        if (isLocalPlayer)
        {
            //CmdSyncRoleWithServer(myTeam);
        }
        ui.MakeUI();
        MAURMustache.SetActive(true);
        myCam.cullingMask = spyMask;
        GetComponent<PlayerPileTask>().canSabotage = true;
        civillianUI.SetActive(false);
        policeUI.SetActive(false);
        spyUI.SetActive(true);
        GetComponent<PlayerPileTask>().TurnOffAnim();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isLocalPlayer)
        {
            Debug.Log(gameObject.name + " is ready", gameObject);
            isReady = true;
            readyText.GetComponentInChildren<TextMeshProUGUI>().text = "You are ready!";
            readyText.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            CmdReadyUp(true);
        }


        /*if (Input.GetKeyDown(KeyCode.Escape) && isLocalPlayer)
        {
            Application.Quit();
        }*/
    }

    [Command]
    void CmdReadyUp(bool b)
    {
        isReady = b;
    }
}
