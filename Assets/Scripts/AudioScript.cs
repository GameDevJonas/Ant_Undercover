using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    AudioSource source;
    TeamManager manager;
    AudioHighPassFilter highPassFilter;

    void Start()
    {
        source = GetComponent<AudioSource>();
        manager = FindObjectOfType<TeamManager>();
        highPassFilter = GetComponent<AudioHighPassFilter>();
    }

    void Update()
    {
        highPassFilter.enabled = !manager.gameStarted;
    }
}
