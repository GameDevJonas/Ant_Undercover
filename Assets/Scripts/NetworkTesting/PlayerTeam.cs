using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTeam : NetworkBehaviour
{
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

    private void Awake()
    {
        isReady = false;
        FindObjectOfType<TeamManager>().playersConnected.Add(GetComponentInParent<PlayerMovement>());
        MAURMustache.SetActive(false);
        //myCam = Camera.main;
        mesh = GetComponentInChildren<MeshFilter>();
        manager = FindObjectOfType<TeamManager>();
        mesh.mesh = manager.teamMeshes[1];
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
        RpcSyncRoleWithClient(myTeam);
        manager.AddToList(this);
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
    }

    public void SyncAll()
    {
        if (isLocalPlayer)
        {
            CmdSyncRoleWithServer(myTeam);
        }
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
        //if (isLocalPlayer)
        {
            MAURMustache.SetActive(false);
            myTeam = TeamManager.PlayerTeams.police;
            mesh.mesh = manager.teamMeshes[0];
            if (isLocalPlayer)
            {
                CmdSyncRoleWithServer(myTeam);
                RpcSyncRoleWithClient(myTeam);
            }
            ui.MakeUI();
            myCam.cullingMask = normalMask;
        }
    }

    public void PickWorker()
    {
        //if (isLocalPlayer)
        {
            MAURMustache.SetActive(false);
            myTeam = TeamManager.PlayerTeams.civillian;
            mesh.mesh = manager.teamMeshes[1];
            if (isLocalPlayer)
            {
                CmdSyncRoleWithServer(myTeam);
                RpcSyncRoleWithClient(myTeam);
            }
            ui.MakeUI();
            myCam.cullingMask = normalMask;
        }
    }

    public void PickSpy()
    {
        //if (isLocalPlayer)
        {
            myTeam = TeamManager.PlayerTeams.spy;
            mesh.mesh = manager.teamMeshes[1];
            if (isLocalPlayer)
            {
                CmdSyncRoleWithServer(myTeam);
                RpcSyncRoleWithClient(myTeam);
            }
            ui.MakeUI();
            MAURMustache.SetActive(true);
            myCam.cullingMask = spyMask;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isLocalPlayer)
        {
            Debug.Log(gameObject.name + " is ready", gameObject);
            isReady = true;
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
