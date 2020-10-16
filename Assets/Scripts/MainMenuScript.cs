using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public NetworkManager manager;

    public TMP_InputField input;
    public string ip;

    void Start()
    {
        manager = FindObjectOfType<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void IpChanged()
    {
        ip = input.text;
        manager.networkAddress = ip;
    }

    public void StartGame(bool host)
    {
        if (host)
        {
            manager.StartHost();
        }
        else
        {
            manager.StartClient();
        }
    }
}
