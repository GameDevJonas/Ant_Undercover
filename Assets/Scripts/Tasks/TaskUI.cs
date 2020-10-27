using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    RoundManager manager;

    public List<WorkerUI> workerUis = new List<WorkerUI>();
    public List<WorkerUI> policeUis = new List<WorkerUI>();
    public List<Sprite> workerObjSprites = new List<Sprite>();
    public List<Sprite> workerTargetSprites = new List<Sprite>();

    public List<Task> activeTasks = new List<Task>();
    public List<Task> finishedTasks = new List<Task>();

    public string localPlayerRole;

    public GameObject myGameObject, workerUI, policeUI;

    public Animator myAnim;

    void Start()
    {
        activeTasks.Capacity = 3;
        Debug.Log(activeTasks.Count);
        manager = GetComponent<RoundManager>();
        myGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (localPlayerRole == "civillian" && myGameObject.activeSelf)
        {
            LoadTaskProgress();
            policeUI.SetActive(false);
            workerUI.SetActive(true);
        }
        else if (localPlayerRole == "police" && myGameObject.activeSelf)
        {
            //Debug.Log("player is police");
            LoadPoliceProgress();
            workerUI.SetActive(false);
            policeUI.SetActive(true);
            foreach(Task task in manager.tasks)
            {
                if(task.spyMinimapObj.activeSelf)
                task.spyMinimapObj.SetActive(false);
            }
        }
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

    public void LoadPoliceProgress()
    {
        foreach (Task task in manager.tasks)
        {
            if (!task.sabotaged)
            {
                policeUis[task.uiOrder].percentage.text = (Mathf.RoundToInt(task.percentage)) + " %";
                task.myMinimapObj.GetComponent<Image>().sprite = workerTargetSprites[task.targetSprite];
                PoliceTaskGetInfo(task.uiOrder, task.name, task.objSprite, task.targetSprite);
            }
            else if (task.sabotaged)
            {
                LoadPoliceSabotageInfo(task);
            }
        }
    }

    void LoadPoliceSabotageInfo(Task task)
    {
        policeUis[task.uiOrder].nameText.text = task.name;
        policeUis[task.uiOrder].objectImage.sprite = workerObjSprites[1];
        policeUis[task.uiOrder].targetImage.sprite = workerTargetSprites[1];
        policeUis[task.uiOrder].objective.sprite = workerObjSprites[3];
        policeUis[task.uiOrder].percentage.text = (Mathf.RoundToInt(task.repairPercentage)) + " %";
        task.myMinimapObj.GetComponent<Image>().sprite = workerTargetSprites[1];
        if (!task.sabotaged)
        {
            WorkerTaskGetInfo(task.uiOrder, task.name, task.objSprite, task.targetSprite);
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
        if (localPlayerRole == "civillian")
        {
            finishedTasks.Add(task);
            int t = activeTasks.IndexOf(task);
            workerUis[t].myGameObject.SetActive(false);
            Debug.Log("Removed " + task.goalName + " of index: " + t);
            activeTasks[t] = null;
            task.active = false;
            GetNewTask(t);
        }
        else if (localPlayerRole == "police")
        {
            finishedTasks.Add(task);
            int t = task.uiOrder;
            policeUis[t].myGameObject.SetActive(false);
            Debug.Log("Removed " + task.goalName + " of index: " + t);
            activeTasks[t] = null;
        }
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
        if (localPlayerRole == "civillian")
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
        else if (localPlayerRole == "police")
        {
            manager.tasks[0].LoadPoliceTask(0);
            manager.tasks[1].LoadPoliceTask(1);
            manager.tasks[2].LoadPoliceTask(2);
            manager.tasks[3].LoadPoliceTask(3);
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

    public void GetTaskInfo(int uiPlacement, string name, int objImage, int trgtImage)
    {
        if (localPlayerRole == "civillian")
        {
            WorkerTaskGetInfo(uiPlacement, name, objImage, trgtImage);
        }
        else if (localPlayerRole == "police")
        {
            PoliceTaskGetInfo(uiPlacement, name, objImage, trgtImage);
        }
    }

    public void WorkerTaskGetInfo(int uiPlacement, string name, int objImage, int trgtImage)
    {
        workerUis[uiPlacement].nameText.text = name;
        workerUis[uiPlacement].objectImage.sprite = workerObjSprites[objImage];
        workerUis[uiPlacement].targetImage.sprite = workerTargetSprites[trgtImage];
        workerUis[uiPlacement].objective.sprite = workerObjSprites[2];
    }
    public void PoliceTaskGetInfo(int uiPlacement, string name, int objImage, int trgtImage)
    {
        policeUis[uiPlacement].nameText.text = name;
        policeUis[uiPlacement].objectImage.sprite = workerObjSprites[objImage];
        policeUis[uiPlacement].targetImage.sprite = workerTargetSprites[trgtImage];
        policeUis[uiPlacement].objective.sprite = workerObjSprites[2];
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
