using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public TaskUI taskUI;

    public GameObject myMinimapObj, spyMinimapObj;

    public enum TaskType { pile };
    public TaskType myTaskType;

    public string goalName;

    public int objSprite, targetSprite, uiOrder;

    public bool done, sabotaged, active, removed, showOnUi;

    public float percentage, repairPercentage;

    void Start()
    {
        myMinimapObj.SetActive(false);
        spyMinimapObj.SetActive(false);
        active = false;
        removed = false;
        //taskUI = FindObjectOfType<TaskUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (taskUI == null)
        {
            taskUI = FindObjectOfType<TaskUI>();
        }

        if (active && showOnUi)
        {
            taskUI.ShowUiObject(myMinimapObj);
            myMinimapObj.SetActive(true);
        }

        else if (!showOnUi)
        {
            taskUI.CloseUiObject(myMinimapObj);
            myMinimapObj.SetActive(false);
        }
        else
        {
            myMinimapObj.SetActive(false);
        }

        if (active && done && !removed)
        {
            taskUI.RemoveTask(this);
            removed = true;
            Invoke("TurnOff", .5f);
        }
    }

    void TurnOff()
    {
        this.enabled = false;
    }

    public void LoadTask(int uiPlacement)
    {
        uiOrder = uiPlacement;
        taskUI.GetTaskInfo(uiOrder, goalName, objSprite, targetSprite);
        taskUI.LoadUI();
        taskUI.activeTasks[uiOrder] = this;
        active = true;
        //myMinimapObj.SetActive(true);
    }

    public void LoadPoliceTask(int uiPlacement)
    {
        uiOrder = uiPlacement;
        taskUI.GetTaskInfo(uiOrder, goalName, objSprite, targetSprite);
        taskUI.LoadUI();
        //taskUI.activeTasks[uiOrder] = this;
        active = true;
    }
}
