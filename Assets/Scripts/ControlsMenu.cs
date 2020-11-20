using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    public bool inMenu;
    public string whichMenu;

    public GameObject returnToMainButton;
    public GameObject returnToChoice;
    public GameObject buttons;

    public Image activeSprite;

    public SpriteInfos sprites;

    public int currentPos;

    List<Sprite> currentSpriteList = new List<Sprite>();

    void Start()
    {
        currentPos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        activeSprite.enabled = inMenu;
        buttons.SetActive(!inMenu);
        returnToMainButton.SetActive(!inMenu);
        returnToChoice.SetActive(inMenu);

        if (inMenu)
        {
            switch (whichMenu)
            {
                case "":
                    currentSpriteList = null;
                    break;
                case "Police":
                    currentSpriteList = sprites.police;
                    break;
                case "Spy":
                    currentSpriteList = sprites.spy;
                    break;
                case "Worker":
                    currentSpriteList = sprites.worker;
                    break;
                case "Other":
                    currentSpriteList = sprites.other;
                    break;
            }

            activeSprite.sprite = currentSpriteList[currentPos];
        }
    }

    public void NextImage()
    {
        if(currentPos < currentSpriteList.Count - 1)
        {
            currentPos++;
        }
        else
        {
            currentPos = 0;
        }
    }

    public void PrevImage()
    {
        if (currentPos > 0)
        {
            currentPos--;
        }
        else
        {
            currentPos = currentSpriteList.Count - 1;
        }
    }

    public void OpenControls(string controls)
    {
        currentPos = 0;
        whichMenu = controls;
        inMenu = true;
    }

    public void ExitControls()
    {
        currentPos = 0;
        whichMenu = "";
        inMenu = false;
    }
}
[System.Serializable]
public class SpriteInfos
{
    public List<Sprite> police = new List<Sprite>();
    public List<Sprite> spy = new List<Sprite>();
    public List<Sprite> worker = new List<Sprite>();
    public List<Sprite> other = new List<Sprite>();
}
