using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public float gameTimer, gameTimerSet;

    public List<Task> tasks = new List<Task>();
    public List<Task> doneTasks = new List<Task>();

    public bool gameDone, spyWin;

    public TextMeshProUGUI timerText;

    public GameObject spyWinObj, policeWinObj;

    void Start()
    {
        spyWinObj.SetActive(false);
        policeWinObj.SetActive(false);
        gameDone = false;
        foreach (Task g in FindObjectsOfType<Task>())
        {
            tasks.Add(g);
        }
        gameTimer = gameTimerSet * 60;
    }

    // Update is called once per frame
    void Update()
    {
        GoalChecker();
        if (!gameDone)
        {
            if (FindObjectOfType<TeamManager>().gameStarted)
            {
                GameTimer();
            }
        }
        else
        {
            spyWinObj.SetActive(spyWin);
            policeWinObj.SetActive(!spyWin);
            Invoke("SendToMainMenu", 5f);
        }
    }

    void SendToMainMenu()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerTeam>().LoadMenu();
        }
    }

    void GoalChecker()
    {
        foreach (Task goal in tasks)
        {
            if (!doneTasks.Contains(goal))
            {
                if (goal.done)
                {
                    doneTasks.Add(goal);
                    tasks.Remove(goal);
                }
            }
        }

        if (tasks.Count == 0)
        {
            Debug.Log("GAME IS DONE");
            spyWin = false;
            gameDone = true;
        }
    }

    void GameTimer()
    {
        if (!gameDone)
        {
            if (gameTimer <= 0)
            {
                spyWin = true;
                gameDone = true;
            }
            else
            {
                gameTimer -= Time.deltaTime;
            }
            double timeInText = Math.Round((double)gameTimer, 2);
            timerText.text = timeInText.ToString();
        }
    }
}
