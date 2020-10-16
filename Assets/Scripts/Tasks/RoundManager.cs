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

    public List<PileGoal> goals = new List<PileGoal>();
    public List<PileGoal> doneGoals = new List<PileGoal>();

    public bool gameDone, spyWin;

    public TextMeshProUGUI timerText;

    public GameObject spyWinObj, policeWinObj;

    void Start()
    {
        spyWinObj.SetActive(false);
        policeWinObj.SetActive(false);
        gameDone = false;
        foreach (PileGoal g in FindObjectsOfType<PileGoal>())
        {
            goals.Add(g);
        }
        gameTimer = gameTimerSet * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameDone)
        {
            GoalChecker();
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
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerTeam>().LoadMenu();
        }
    }

    void GoalChecker()
    {
        foreach (PileGoal goal in goals)
        {
            if (!doneGoals.Contains(goal))
            {
                if (goal.done)
                {
                    doneGoals.Add(goal);
                }
            }
        }

        if (doneGoals.Count == goals.Count)
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
