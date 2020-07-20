using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;

    public Checkpoint[] checkpoints;
    public GameObject spawner;
    public GameObject[] coins;

    private Transform checkpointedPosition;
    public Transform player;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        levelManager = this;
        audioSource = GetComponent<AudioSource>();
        CheckSound();
    }

    private void CheckSound()
    {
        if(PlayerPrefs.GetInt("Sound") == 0)
        {
            audioSource.Play();
        }
    }

    public void LoadLastCheckpoint()
    {
        bool thereAreCheckpoints = false;

        foreach (var check in checkpoints)
        {
            check.ActivateInvisibleBlocks();

            if (check.IAmCheckpointed)
            {
                thereAreCheckpoints = true;
            }
        }

        if (thereAreCheckpoints)
        {
            player.position = new Vector3(checkpointedPosition.position.x, checkpointedPosition.position.y + 2, checkpointedPosition.position.z);
        }
        else
        {
            player.position = spawner.transform.position;
            player.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Update()
    {
        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reset();
        }
    }

    public void SetCurrentCheckpoint(Transform checkpoint)
    {
        checkpointedPosition = checkpoint;
    }

    public void Reset()
    {
        bool tutorialLevel = false;

        player.localScale = new Vector3(1, 1, 1);
        player.position = spawner.transform.position;
        checkpointedPosition = null;

        foreach(var check in checkpoints)
        {
            check.IAmCheckpointed = false;
            check.ActivateInvisibleBlocks();
            check.TurnOffCheckpoint();
            if(check.isTutorial)
            {
                tutorialLevel = true;
            }
        }

        foreach(var coin in coins)
        {
            coin.SetActive(true);
        }

        if(tutorialLevel)
        {
            TutorialManager.TM.ResetTutorial();
        }
        else
        {
            ScoreManager.scoreManager.Reset();
        }
    }

    public void LeaveLevel()
    {
        GameManager.GM.ExitLevel();
    }
}
