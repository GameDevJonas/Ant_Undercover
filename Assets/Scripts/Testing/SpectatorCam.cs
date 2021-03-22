using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCam : MonoBehaviour
{
    public GameObject cam;
    public bool isOn;

    public float xInput, yInput, speed, normalSpeed, superSpeed;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerPileTask>().isLocalPlayer && Input.GetKeyDown(KeyCode.P))
        {
            isOn = !isOn;
            cam.SetActive(isOn);
            cam.transform.position = transform.position;
            if (cam.activeSelf)
            {
                cam.transform.parent = null;
                GetComponent<PlayerMovement>().enabled = false;
            }
            else
            {
                cam.transform.parent = this.transform;
                GetComponent<PlayerMovement>().enabled = true;
            }
        }

        if (isOn)
        {
            GetInputs();
            ApplyMovement();
        }
    }

    public void GetInputs()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = superSpeed;
        }
        else
        {
            speed = normalSpeed;
        }
    }

    public void ApplyMovement()
    {
        if(xInput != 0)
        {
            cam.transform.Translate(Vector3.right * xInput * speed * Time.deltaTime);
        }
        if(yInput != 0)
        {
            cam.transform.Translate(Vector3.forward * yInput * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            cam.transform.Translate(Vector3.down * normalSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            cam.transform.Translate(Vector3.up * normalSpeed * Time.deltaTime);
        }
    }
}
