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

    public bool showControls, serverOnly;

    public GameObject mainMenu, controlsMenu;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        manager = FindObjectOfType<NetworkManager>();
        if (serverOnly)
        {
            manager.StartServer();
        }
        else
        {
            showControls = false;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
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

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
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
