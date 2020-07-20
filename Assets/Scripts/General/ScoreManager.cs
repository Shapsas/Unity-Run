using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManager;

    public TMP_Text scoreText;
    public GameObject finishLevelPanel;
    public GameObject speedPanel;
    public GameObject bigBlock;

    public TMP_Text timerText;
    public TMP_Text speedText;
    public TMP_Text timeText;
    public TMP_Text newHighScoreText;

    public TMP_Text finishTimeText;
    public TMP_Text restartText;

    private int deathCount = 0;
    private int coins = 0;
    public int lastScore = 0;

    public bool levelFinished = false;
    private bool go;

    float timeT;
    TimeSpan ts;

    private void Awake()
    {
        scoreManager = this;
    }

    public void SetSpeed(float spd)
    {
        speedText.text = "Speed: " + spd.ToString("0.0000");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown(3));
    }

    private void Update()
    {
        if(go)
        {
            timeT += Time.deltaTime;
            int totalSeconds = (int)timeT;
            ts = TimeSpan.FromSeconds(timeT);
            timeText.text = (totalSeconds / 60).ToString() + ":" + (totalSeconds % 60).ToString("00");
        }
    }

    void DoStuff()
    {
        if(restartText.text != "")
        {
            restartText.text = "";
        }
        go = true;
        timerText.text = "GO";
        speedPanel.SetActive(true);
        bigBlock.SetActive(false);
        Invoke("ClearText", 1f);
    }

    IEnumerator Countdown(int seconds)
    {
        timerText.transform.parent.gameObject.SetActive(true);
        speedPanel.SetActive(false);
        bigBlock.SetActive(true);
        int counter = seconds;
        while (counter > 0)
        {
            timerText.text = "" + counter;
            yield return new WaitForSeconds(1);
            counter--;
        }
        DoStuff();
    }

    public void ShowScore()
    {
        levelFinished = true;
        int time = (int)timeT;
        finishTimeText.text = "Time: " + (time / 60).ToString() + ":" + (time % 60).ToString("00");
        go = false;
        lastScore = ((GameManager.GM.currentLevel.level.id * 2) * 1000) + coins + (100000/time) - (deathCount * 50);
        scoreText.text = "Score: " + lastScore;

        if(lastScore > GameManager.GM.currentLevel.currentScore)
        {
            newHighScoreText.gameObject.SetActive(true);
        }

        MyInput.myInput.DisableInput();
        finishLevelPanel.SetActive(true);
        speedPanel.SetActive(false);
    }

    public void IncreaseDeathCount()
    {
        deathCount += 1;
    }

    public void IncreaseCoinCount()
    {
        coins += 200;
    }

    public void Reset()
    {
        if(levelFinished == false)
        {
            go = false;
            StartCoroutine(Countdown(3));
            coins = 0;
            deathCount = 0;
            timeT = 0;
            if (newHighScoreText.gameObject.activeInHierarchy)
            {
                newHighScoreText.gameObject.SetActive(false);
            }
        }
    }

    private void ClearText()
    {
        timerText.text = "";
        timerText.transform.parent.gameObject.SetActive(false);
    }
}
