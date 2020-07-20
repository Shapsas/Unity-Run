using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{

    public bool isTutorial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(isTutorial)
            {
                EndTutorial();
            }
            else
            {
                EndLevel();
            }
        }
    }

    private void EndTutorial()
    {
        if (TutorialManager.TM.FinishQuest5())
        {
            TutorialManager.TM.FinishTutorial();
        }
        else
        {
            TutorialManager.TM.FailTutorial();
        }
    }

    private void EndLevel()
    {
        ScoreManager.scoreManager.ShowScore();
    }
}
