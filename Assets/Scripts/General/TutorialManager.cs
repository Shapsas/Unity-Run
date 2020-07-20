using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager TM;

    public TMP_Text[] questTexts;

    private List<string> firstQuest;
    private bool[] quests;

    public GameObject tutorialFinishedPanel;
    public GameObject tutorialFailedPanel;
    public GameObject tutorialPanel;

    // Start is called before the first frame update
    void Awake()
    {
        TM = this;

        tutorialPanel.SetActive(true);
        firstQuest = new List<string>();
        quests = new bool[7];
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if(quests[1] == false)
            {
                FinishQuest1();
                quests[1] = true;
            }
        }

        if(Keyboard.current.leftCtrlKey.wasPressedThisFrame && quests[4] == false)
        {
            FinishQuest4();
            quests[4] = true;
        }

        if(quests[0] == false)
        {
            if(Keyboard.current.wKey.wasPressedThisFrame)
            {
                FinishQuest0("0");
            }
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                FinishQuest0("1");
            }
            if(Keyboard.current.aKey.wasPressedThisFrame)
            {
                FinishQuest0("2");
            }
            if(Keyboard.current.dKey.wasPressedThisFrame)
            {
                FinishQuest0("3");
            }
        }
    }

    public void FinishQuest0(string key)
    {
        if(!firstQuest.Contains(key))
        {
            firstQuest.Add(key);
        }
        questTexts[0].text = "Use WASD Keys to walk around - " + firstQuest.Count + "/4";

        if(firstQuest.Count == 4)
        {
            questTexts[0].fontStyle = FontStyles.Strikethrough;
            quests[0] = true;
        }
    }

    public void FinishQuest1()
    {
        questTexts[1].text = "Press Space bar to Jump - 1/1";
        questTexts[1].fontStyle = FontStyles.Strikethrough;
    }

    public void FinishQuest2()
    {
        questTexts[2].text = "Reach a Checkpoint - 1/1";
        questTexts[2].fontStyle = FontStyles.Strikethrough;
        quests[2] = true;
    }

    public void FinishQuest3()
    {
        questTexts[3].text = "Hold Space bar to Bhop - 1/1";
        questTexts[3].fontStyle = FontStyles.Strikethrough;
        quests[3] = true;
    }

    public void FinishQuest4()
    {
        questTexts[4].text = "Hold CTRL key to slide - 1/1";
        questTexts[4].fontStyle = FontStyles.Strikethrough;
    }

    public bool FinishQuest5()
    {
        questTexts[5].text = "Finish the level - 1/1";
        questTexts[5].fontStyle = FontStyles.Strikethrough;
        quests[5] = true;

        int number = 0;

        foreach(var text in quests)
        {
            if (text == true)
            {
                number++;
            }
        }

        if(number == 7)
        {
            return true;
        }

        return false;
    }

    public void FinishQuest6()
    {
        questTexts[6].text = "Collect the coin - 1/1";
        questTexts[6].fontStyle = FontStyles.Strikethrough;
        quests[6] = true;
    }

    public void FinishTutorial()
    {
        tutorialPanel.SetActive(false);
        tutorialFinishedPanel.SetActive(true);
        MyInput.myInput.DisableInput();
    }

    public void FailTutorial()
    {
        tutorialFailedPanel.SetActive(true);
        tutorialPanel.SetActive(false);
        MyInput.myInput.DisableInput();
    }

    public void ResetTutorial()
    {
        int count = 0;

        foreach(var quest in quests)
        {
            if(quest == true)
            {
                count++;
            }
        }

        if(count != 7)
        {
            MyInput.myInput.EnableInput();
            firstQuest = new List<string>();
            quests = new bool[7];
            questTexts[0].text = "Use WASD Keys to walk around - " + 0 + "/4";
            questTexts[1].text = "Press Space bar to Jump - 0/1";
            questTexts[2].text = "Reach a Checkpoint - 0/1";
            questTexts[3].text = "Hold Space bar to Bhop - 0/1";
            questTexts[4].text = "Hold CTRL key to slide - 0/1";
            questTexts[6].text = "Collect the coin - 0/1";
            questTexts[5].text = "Finish the level - 0/1";

            foreach (var text in questTexts)
            {
                text.fontStyle = FontStyles.Normal;
            }

            tutorialPanel.SetActive(true);

            if (tutorialFailedPanel.activeInHierarchy)
            {
                tutorialFailedPanel.SetActive(false);
            }
        }
    }
}
