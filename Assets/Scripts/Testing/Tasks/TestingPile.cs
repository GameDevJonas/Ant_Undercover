using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPile : MonoBehaviour
{
    public bool playerInRange;

    public GameObject canvas, prefab;

    void Start()
    {
        playerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCanvas();
    }

    void CheckForCanvas()
    {
        canvas.SetActive(playerInRange);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTestingTask player = other.GetComponent<PlayerTestingTask>();
            if (!player.holdingItem)
            {
                playerInRange = true;
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
