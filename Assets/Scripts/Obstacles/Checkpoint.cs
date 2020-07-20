using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject[] invisibleBlocks;

    private Light checkLight;

    public bool isTutorial;

    public bool IAmCheckpointed;

    private void Awake()
    {
        checkLight = GetComponentInChildren<Light>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SetCheckPoint(other.gameObject.transform);

            if(isTutorial)
            {
                TutorialManager.TM.FinishQuest2();
            }
        }
    }

    private void SetCheckPoint(Transform player)
    {
        checkLight.color = new Color32(255, 255, 0, 255);
        LevelManager.levelManager.SetCurrentCheckpoint(this.gameObject.transform);
        IAmCheckpointed = true;
    }

    public void TurnOffCheckpoint()
    {
        checkLight.color = new Color32(255, 255, 255, 255);
    }

    public void ActivateInvisibleBlocks()
    {
        if(invisibleBlocks.Length > 0)
        {
            foreach (var block in invisibleBlocks)
            {
                block.SetActive(true);
            }
        }
    }
}
