using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiting : MonoBehaviour
{
    public void Yes()
    {
        Application.Quit();
    }

    public void No()
    {
        this.gameObject.SetActive(false);
        if(ScoreManager.scoreManager.levelFinished == false)
        {
            MyInput.myInput.EnableInput();
        }
    }

    public void GoToMenu()
    {
        GameManager.GM.GoToMainMenu();
        this.gameObject.SetActive(false);
    }
}
