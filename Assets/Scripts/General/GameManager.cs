using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public LevelSelectionFactory levelSelectionFactory;

    public GameObject playerPrefab;
    public GameObject playerCameraPrefab;
    public GameObject level;

    private GameObject playerInstance;
    private GameObject cameraInstance;

    public GameObject UIManager;
    public Transform spawnPoint;
    public GameObject AreYouSure;

    public List<LevelModel> leveliai = new List<LevelModel>();

    public LevelModel currentLevel;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        GM = this;
        Initialize();
    }

    private void Initialize()
    {
        levelSelectionFactory.Load();
        leveliai = levelSelectionFactory.model.allLevels;
    }

    public void PrepareLevel(int nr)
    {
        currentLevel = leveliai[nr];
        level = Instantiate(leveliai[nr].level.levelPrefab);
        playerInstance = Instantiate(playerPrefab);
        cameraInstance = Instantiate(playerCameraPrefab);

        spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        playerInstance.transform.position = spawnPoint.position;
        playerInstance.GetComponent<StrafeMovement>().camObj = cameraInstance;
        cameraInstance.GetComponent<MouseLook>().player = playerInstance.gameObject.transform.GetChild(0);

        LevelManager.levelManager.player = playerInstance.transform;
        UIManager.SetActive(false);
    }

    public void ExitLevel()
    {
        foreach(var lvl in leveliai)
        {
            if(lvl.level.id == currentLevel.level.id)
            {
                lvl.completed = true;
                if(lvl.level.id != 0)
                {
                    if (lvl.currentScore <= ScoreManager.scoreManager.lastScore)
                    {
                        lvl.currentScore = ScoreManager.scoreManager.lastScore;
                    }
                }
            }
        }

        UpdateLevelValues(levelSelectionFactory.model);

        spawnPoint = null;
        Destroy(level);
        Destroy(playerInstance);
        Destroy(cameraInstance);
        UIManager.SetActive(true);
    }

    public void GoToMainMenu()
    {
        spawnPoint = null;
        Destroy(level);
        Destroy(playerInstance);
        Destroy(cameraInstance);
        UIManager.SetActive(true);
    }


    private void UpdateLevelValues(LevelSelectionModel model)
    {
        var allLevels = model.allLevels;

        if (leveliai.Count > 0)
        {
            for (int i = 0; i < allLevels.Count; i++)
            {
                if (allLevels[i].level.id == currentLevel.level.id && currentLevel.completed == true)
                {
                    var index = i;
                    allLevels[index].completed = true;
                }
            }
        }
        string jsonString = JsonUtility.ToJson(model);
        PlayerPrefs.SetString("Levels", jsonString);

        Initialize();
    }

    public void ConfirmQuit()
    {
        AreYouSure.SetActive(true);
    }
}
