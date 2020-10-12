using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScript : MonoBehaviour
{
    TeamManager manager;

    void Start()
    {
        manager = FindObjectOfType<TeamManager>();
    }

    void Update()
    {
        gameObject.SetActive(!manager.gameStarted);
    }
}
