using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public NetworkManager manager;

    public TMP_InputField input;
    public string ip;

    public bool showControls;

    public GameObject mainMenu, controlsMenu;

    void Start()
    {
        showControls = false;
        manager = FindObjectOfType<NetworkManager>();
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        mainMenu.SetActive(!showControls);
        controlsMenu.SetActive(showControls);
    }

    public void ControlsOnOff()
    {
        showControls = !showControls;
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
