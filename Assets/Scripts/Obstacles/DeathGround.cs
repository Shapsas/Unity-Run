using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGround : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            LevelManager.levelManager.LoadLastCheckpoint();

            if(!LevelManager.levelManager.checkpoints[0].isTutorial)
            {
                ScoreManager.scoreManager.IncreaseDeathCount();
            }
        }
    }
}
