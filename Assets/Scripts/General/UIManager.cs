using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button[] levelButtons;
    public Image[] levelImage;
    public Text[] levelScoreText;

    public GameObject playPanel;
    public GameObject levelSelectionPanel;

    public AudioSource audioSource;

    public GameObject mutedSoundGameObject;
    public GameObject notMutedSoundGameObject;

    public List<LevelModel> levels = new List<LevelModel>();

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            audioSource.Play();
            notMutedSoundGameObject.SetActive(true);
            if (mutedSoundGameObject.activeInHierarchy)
            {
                mutedSoundGameObject.SetActive(false);
            }
        }
        else
        {
            mutedSoundGameObject.SetActive(true);
            if(notMutedSoundGameObject.activeInHierarchy)
            {
                notMutedSoundGameObject.SetActive(false);
            }
        }
    }

    public void MuteSound()
    {
        PlayerPrefs.SetInt("Sound", 1);
        audioSource.Stop();
        mutedSoundGameObject.SetActive(true);
        notMutedSoundGameObject.SetActive(false);
    }

    public void UnmuteSound()
    {
        PlayerPrefs.SetInt("Sound", 0);
        audioSource.Play();
        mutedSoundGameObject.SetActive(false);
        notMutedSoundGameObject.SetActive(true);
    }

    public void OpenLevels()
    {
        playPanel.SetActive(false);
        levelSelectionPanel.SetActive(true);

        levels = GameManager.GM.leveliai;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelImage[i].sprite = levels[i].level.levelSprite;

            if (levels[i].unlocked == true)
            {
                levelButtons[i].interactable = true;

                if (levels[i].completed == true)
                {
                    if (i == 0)
                    {
                        levelScoreText[i].text = "Tutorial completed";
                    }
                    else
                    {
                        levelScoreText[i].text = "Score: " + levels[i].currentScore;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GameManager.GM.ConfirmQuit();
        }
    }

    public void StartLevel(int id)
    {
        GameManager.GM.PrepareLevel(id);
        DisableUI();
    }

    private void DisableUI()
    {
        playPanel.SetActive(true);
        levelSelectionPanel.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
