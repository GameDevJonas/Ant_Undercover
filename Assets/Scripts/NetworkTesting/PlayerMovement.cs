using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class PlayerMovement : NetworkBehaviour
{
    TeamManager manager;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float turnSpeed;
    public float xInput;
    public float yInput;

    public GameObject model;
    public CinemachineFreeLook cam;

    //Evt legge til on start local player


    private void Start()
    {
        manager = FindObjectOfType<TeamManager>();
        cam = GetComponentInChildren<CinemachineFreeLook>();
        if (!isLocalPlayer)
        {
            cam.gameObject.SetActive(false);
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        }
        controller = gameObject.GetComponent<CharacterController>();
    }

    public override void OnStopClient()
    {
        manager.playersConnected.Remove(this);
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            GetInputs();
            RotatePlayer();
            MovePlayer();
        }
    }

    private void FixedUpdate()
    {

    }

    void GetInputs()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        if (controller.isGrounded)
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            playerVelocity = (h * right + v * forward);

            if (Input.GetButton("Jump"))
            {
                playerVelocity.y = jumpHeight;
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * playerSpeed * Time.deltaTime);
    }

    void RotatePlayer()
    {
        if (yInput == 1 && xInput == 1)
        {
            GetRotation(new Vector3(-1f, 0f, -1f));
        }
        else if (yInput == 1 && xInput == -1)
        {
            GetRotation(new Vector3(1f, 0f, -1f));
        }
        else if (yInput == -1 && xInput == 1)
        {
            GetRotation(new Vector3(-1f, 0f, 1f));
        }
        else if (yInput == -1 && xInput == -1)
        {
            GetRotation(new Vector3(1f, 0f, 1f));
        }
        else if (yInput == 1)
        {
            GetRotation(new Vector3(0f, 0f, -1f));
        }
        else if (yInput == -1)
        {
            GetRotation(new Vector3(0f, 0f, 1f));
        }
        else if (xInput == 1)
        {
            GetRotation(new Vector3(-1f, 0f, 0f));
        }
        else if (xInput == -1)
        {
            GetRotation(new Vector3(1f, 0f, 0f));
        }
    }

    void GetRotation(Vector3 toRotation)
    {
        Vector3 relativePos = Camera.main.transform.TransformDirection(toRotation);
        relativePos.y = 0.0f;
        Quaternion rotation = Quaternion.LookRotation(-relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }
}
