using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool isTutorial;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TurnOffMe();

            if (isTutorial)
            {
                TutorialManager.TM.FinishQuest6();
            }
            else
            {
                ScoreManager.scoreManager.IncreaseCoinCount();
            }
        }
    }

    private void TurnOffMe()
    {
        this.gameObject.SetActive(false);
    }
}
