using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    public float timerSet, timer;

    public bool playerInRange;

    public GameObject canvas, prefab;

    public int objectsReady;

    public List<GameObject> objectsInPile = new List<GameObject>();

    public string what;

    void Start()
    {
        playerInRange = false;
        objectsReady = objectsInPile.Count;
        timer = timerSet;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCanvas();
        UpdatePile();
        AddObject();
    }

    void UpdatePile()
    {
        switch (objectsReady)
        {
            case 6:
                objectsInPile[5].SetActive(true);
                objectsInPile[4].SetActive(true);
                objectsInPile[3].SetActive(true);
                objectsInPile[2].SetActive(true);
                objectsInPile[1].SetActive(true);
                objectsInPile[0].SetActive(true);
                break;
            case 5:
                objectsInPile[5].SetActive(false);
                objectsInPile[4].SetActive(true);
                objectsInPile[3].SetActive(true);
                objectsInPile[2].SetActive(true);
                objectsInPile[1].SetActive(true);
                objectsInPile[0].SetActive(true);
                break;
            case 4:
                objectsInPile[5].SetActive(false);
                objectsInPile[4].SetActive(false);
                objectsInPile[3].SetActive(true);
                objectsInPile[2].SetActive(true);
                objectsInPile[1].SetActive(true);
                objectsInPile[0].SetActive(true);
                break;
            case 3:
                objectsInPile[5].SetActive(false);
                objectsInPile[4].SetActive(false);
                objectsInPile[3].SetActive(false);
                objectsInPile[2].SetActive(true);
                objectsInPile[1].SetActive(true);
                objectsInPile[0].SetActive(true);
                break;
            case 2:
                objectsInPile[5].SetActive(false);
                objectsInPile[4].SetActive(false);
                objectsInPile[3].SetActive(false);
                objectsInPile[2].SetActive(false);
                objectsInPile[1].SetActive(true);
                objectsInPile[0].SetActive(true);
                break;
            case 1:
                objectsInPile[5].SetActive(false);
                objectsInPile[4].SetActive(false);
                objectsInPile[3].SetActive(false);
                objectsInPile[2].SetActive(false);
                objectsInPile[1].SetActive(false);
                objectsInPile[0].SetActive(true);
                break;
            case 0:
                objectsInPile[5].SetActive(false);
                objectsInPile[4].SetActive(false);
                objectsInPile[3].SetActive(false);
                objectsInPile[2].SetActive(false);
                objectsInPile[1].SetActive(false);
                objectsInPile[0].SetActive(false);
                break;
        }
    }

    public void TakeObject()
    {
        if (objectsReady > 0)
            objectsReady--;
    }

    void AddObject()
    {
        if (objectsReady < 6)
        {
            if (timer <= 0)
            {
                objectsReady++;
                timer = timerSet;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    void CheckForCanvas()
    {
        canvas.SetActive(playerInRange);
        canvas.transform.LookAt(Camera.main.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPileTask player = other.GetComponent<PlayerPileTask>();
            if (!player.holdingItem && objectsReady > 0 && player.GetComponent<PlayerTeam>().myTeam != TeamManager.PlayerTeams.police)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
