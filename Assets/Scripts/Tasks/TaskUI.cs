using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    RoundManager manager;

    public List<WorkerUI> workerUis = new List<WorkerUI>();
    public List<Sprite> workerObjSprites = new List<Sprite>();
    public List<Sprite> workerTargetSprites = new List<Sprite>();

    public List<Task> activeTasks = new List<Task>();
    public List<Task> finishedTasks = new List<Task>();

    public string localPlayerRole;

    public GameObject myGameObject;

    void Start()
    {
        activeTasks.Capacity = 3;
        manager = GetComponent<RoundManager>();
        myGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        LoadTaskProgress();
    }

    public void ShowUiObject(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void CloseUiObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void LoadTaskProgress()
    {
        foreach (Task task in activeTasks)
        {
            if (!task.sabotaged)
            {
                workerUis[task.uiOrder].percentage.text = (Mathf.RoundToInt(task.percentage)) + " %";
                task.myMinimapObj.GetComponent<Image>().sprite = workerTargetSprites[task.targetSprite];
                WorkerTaskGetInfo(task.uiOrder, task.name, task.objSprite, task.targetSprite);
            }
            else if (task.sabotaged)
            {
                LoadSabotageInfo(task);
            }
        }
    }

    void LoadSabotageInfo(Task task)
    {
        workerUis[task.uiOrder].nameText.text = task.name;
        workerUis[task.uiOrder].objectImage.sprite = workerObjSprites[1];
        workerUis[task.uiOrder].targetImage.sprite = workerTargetSprites[1];
        workerUis[task.uiOrder].objective.sprite = workerObjSprites[3];
        workerUis[task.uiOrder].percentage.text = (Mathf.RoundToInt(task.repairPercentage)) + " %";
        task.myMinimapObj.GetComponent<Image>().sprite = workerTargetSprites[1];
        if (!task.sabotaged)
        {
            WorkerTaskGetInfo(task.uiOrder, task.name, task.objSprite, task.targetSprite);
        }
    }

    public void RemoveTask(Task task)
    {
        finishedTasks.Add(task);
        int t = activeTasks.IndexOf(task);
        workerUis[t].myGameObject.SetActive(false);
        Debug.Log("Removed " + task.goalName + " of index: " + t);
        activeTasks[t] = null;
        GetNewTask(t);
    }

    void GetNewTask(int index)
    {
        if (manager.tasks.Count > 1)
        {
            int i = 0;
            foreach (Task task in manager.tasks)
            {
                Debug.Log("Count " + i);
                //int o = Random.Range(0, manager.tasks.Count);
                //Debug.Log(o);
                if (!activeTasks.Contains(task) && !finishedTasks.Contains(task))
                {
                    Debug.Log("Added " + task.goalName);
                    //activeTasks.RemoveAt(index);
                    workerUis[index].myGameObject.SetActive(true);
                    manager.tasks[i].LoadTask(index);
                    return;
                }
                else if (i == manager.tasks.Count)
                {
                    Debug.Log("No available tasks");
                    return;
                }
                i++;
            }
        }
        else
        {
            Debug.Log("Not more than one task available");
        }
    }

    public void LoadUI()
    {
        if (localPlayerRole == "spy" || localPlayerRole == null)
        {
            myGameObject.SetActive(false);
        }
        else
        {
            myGameObject.SetActive(true);
        }
    }

    public void GetStarterTasks(string role)
    {
        localPlayerRole = role;
        if (localPlayerRole != "spy")
        {
            int o = Random.Range(0, manager.tasks.Count);
            //Debug.Log(o);
            manager.tasks[o].LoadTask(0);
            int t = SecondRand(o);
            //Debug.Log(t);
            manager.tasks[t].LoadTask(1);
            int tr = ThirdRand(o, t);
            //Debug.Log(tr);
            manager.tasks[tr].LoadTask(2);
        }
    }

    int SecondRand(int before)
    {
        int r = Random.Range(0, manager.tasks.Count);
        if (r != before)
        {
            return r;
        }
        else
        {
            return SecondRand(before);
        }
    }

    int ThirdRand(int first, int before)
    {
        int r = Random.Range(0, manager.tasks.Count);
        if (r != before && r != first)
        {
            return r;
        }
        else
        {
            return ThirdRand(first, before);
        }
    }

    public void WorkerTaskGetInfo(int uiPlacement, string name, int objImage, int trgtImage)
    {
        workerUis[uiPlacement].nameText.text = name;
        workerUis[uiPlacement].objectImage.sprite = workerObjSprites[objImage];
        workerUis[uiPlacement].targetImage.sprite = workerTargetSprites[trgtImage];
        workerUis[uiPlacement].objective.sprite = workerObjSprites[2];
    }
}

[System.Serializable]
public class WorkerUI
{
    public GameObject myGameObject;
    public TextMeshProUGUI nameText;
    public Image objectImage;
    public TextMeshProUGUI percentage;
    public Image targetImage;
    public Image objective;
}
