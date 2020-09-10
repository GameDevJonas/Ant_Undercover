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

    Camera mainCam;

    public LayerMask normalMask, spyMask;

    [SyncVar]
    public bool isReady;

    public override void OnStartClient()
    {
        
    }

    private void Awake()
    {
        isReady = false;
        FindObjectOfType<TeamManager>().playersConnected.Add(GetComponentInParent<PlayerMovement>());
        MAURMustache.SetActive(false);
        mainCam = Camera.main;
        mesh = GetComponentInChildren<MeshFilter>();
        manager = FindObjectOfType<TeamManager>();
        mesh.mesh = manager.teamMeshes[1];
    }
    
    public void PickRole()
    {
        int role = Random.Range(0, 3);
        if (role == 0)
        {
            MAURMustache.SetActive(false);
            myTeam = TeamManager.PlayerTeams.police;
            mesh.mesh = manager.teamMeshes[0];
            mainCam.cullingMask = normalMask;
        }
        else if (role == 1)
        {
            MAURMustache.SetActive(false);
            myTeam = TeamManager.PlayerTeams.civillian;
            mesh.mesh = manager.teamMeshes[1];
            mainCam.cullingMask = normalMask;
        }
        else
        {
            myTeam = TeamManager.PlayerTeams.spy;
            mesh.mesh = manager.teamMeshes[1];
            mainCam.cullingMask = spyMask;
            MAURMustache.SetActive(true);
        }
        SyncRoleWithServer(myTeam);
        ui.MakeUI();
    }

    [Command]
    void SyncRoleWithServer(TeamManager.PlayerTeams role)
    {
        if (role == TeamManager.PlayerTeams.police)
        {
            MAURMustache.SetActive(false);
            myTeam = role;
            mesh.mesh = manager.teamMeshes[0];
            //mainCam.cullingMask = normalMask;
        }
        else if (role == TeamManager.PlayerTeams.civillian)
        {
            MAURMustache.SetActive(false);
            myTeam = role;
            mesh.mesh = manager.teamMeshes[1];
            //mainCam.cullingMask = normalMask;
        }
        else
        {
            myTeam = role;
            mesh.mesh = manager.teamMeshes[1];
            //mainCam.cullingMask = spyMask;
            MAURMustache.SetActive(true);
        }
    }

    public void PickPolice()
    {
        MAURMustache.SetActive(false);
        myTeam = TeamManager.PlayerTeams.police;
        mesh.mesh = manager.teamMeshes[0];
        mainCam.cullingMask = normalMask;
        SyncRoleWithServer(myTeam);
        ui.MakeUI();
    }

    public void PickWorker()
    {
        MAURMustache.SetActive(false);
        myTeam = TeamManager.PlayerTeams.civillian;
        mesh.mesh = manager.teamMeshes[1];
        mainCam.cullingMask = normalMask;
        SyncRoleWithServer(myTeam);
        ui.MakeUI();
    }

    public void PickSpy()
    {
        myTeam = TeamManager.PlayerTeams.spy;
        mesh.mesh = manager.teamMeshes[1];
        mainCam.cullingMask = spyMask;
        MAURMustache.SetActive(true);
        SyncRoleWithServer(myTeam);
        ui.MakeUI();
    }

    void Start()
    {
        manager.AddToList(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isLocalPlayer)
        {
            Debug.Log(gameObject.name + " is ready", gameObject);
            isReady = true;
            ReadyUp(true);
        }
    }

    [Command]
    void ReadyUp(bool b)
    {
        isReady = b;
    }
}
