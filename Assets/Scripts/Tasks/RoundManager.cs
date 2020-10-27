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
    public int minutes, seconds;

    public List<Task> tasks = new List<Task>();
    public List<Task> doneTasks = new List<Task>();

    public bool gameDone, spyWin;

    public TextMeshProUGUI timerText;

    public GameObject spyWinObj, policeWinObj;

    public Color normalTimerColor, lastTimerColor;
    public Animator timerAnim;

    void Start()
    {
        //HostOptions host;
        //foreach (HostOptions hostOptions in FindObjectsOfType<HostOptions>())
        //{
        //    if (hostOptions.isHost)
        //    {
        //        host = hostOptions;
        //        gameTimer = host.gameTime;
        //    }
        //}
        timerAnim.SetBool("LastBool", false);
        timerText.text = "";

        spyWinObj.SetActive(false);
        policeWinObj.SetActive(false);
        gameDone = false;
        foreach (Task g in FindObjectsOfType<Task>())
        {
            tasks.Add(g);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<TeamManager>().gameStarted)
        {
            gameTimer = gameTimerSet * 60;
        }
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
            foreach(PileGoal pileGoal in FindObjectsOfType<PileGoal>())
            {
                pileGoal.enabled = false;
            }
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
                timerAnim.SetBool("LastBool", false);
                timerText.text = "";
                spyWin = true;
                gameDone = true;
            }
            else
            {
                gameTimer -= Time.deltaTime;
            }
            minutes = Mathf.FloorToInt(gameTimer / 60);
            seconds = Mathf.FloorToInt(gameTimer - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            double timeInText = Math.Round((double)gameTimer, 2);

            if (gameTimer >= 0)
            {
                if (gameTimer >= 31)
                {
                    timerText.color = normalTimerColor;
                    timerText.text = niceTime;
                }
                else
                {
                    timerAnim.SetBool("LastBool", true);
                    timerText.color = lastTimerColor;
                    timerText.text = (double)timeInText + "";
                }
            }
        }
    }
}
