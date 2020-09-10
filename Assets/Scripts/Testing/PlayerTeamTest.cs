using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamTest : MonoBehaviour
{
    TeamTesting manager;
    public TeamTesting.PlayerTeams myTeam;
    public GameObject MAURMustache;
    MeshFilter mesh;

    Camera mainCam;

    public LayerMask normalMask, spyMask;

    private void Awake()
    {
        MAURMustache.SetActive(false);
        mainCam = Camera.main;
        mesh = GetComponentInChildren<MeshFilter>();
        manager = FindObjectOfType<TeamTesting>();
        int role = Random.Range(0, 3);
        if (role == 0)
        {
            myTeam = TeamTesting.PlayerTeams.police;
            mesh.mesh = manager.teamMeshes[0];
            mainCam.cullingMask = normalMask;
        }
        else if (role == 1)
        {
            myTeam = TeamTesting.PlayerTeams.civillian;
            mesh.mesh = manager.teamMeshes[1];
            mainCam.cullingMask = normalMask;
        }
        else
        {
            myTeam = TeamTesting.PlayerTeams.spy;
            mesh.mesh = manager.teamMeshes[1];
            mainCam.cullingMask = spyMask;
            MAURMustache.SetActive(true);
        }
    }

    void Start()
    {
        manager.AddToList(this);

    }
}
