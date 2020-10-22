using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOnlyScript : MonoBehaviour
{
    bool isServerOnly;

    void Start()
    {
        isServerOnly = FindObjectOfType<MainMenuScript>().serverOnly;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServerOnly)
        {
            DontDestroyOnLoad(this);
        }
    }
}
