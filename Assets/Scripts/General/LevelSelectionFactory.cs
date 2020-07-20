using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionFactory : MonoBehaviour
{
    public LevelSelectionModel model { get; private set; }

    private LevelSelectionModel tempModel;
    private DataLoader dataLoader;

    public LevelSO[] levelSOs;

    public void Load()
    {
        dataLoader = new DataLoader();
        this.model = GetModel();
    }

    private LevelSelectionModel GetModel()
    {
        if (!PlayerPrefs.HasKey("Levels"))
        {
            model = new LevelSelectionModel(dataLoader.LoadDefaultData(levelSOs));
        }
        else
        {
            DeserializeFromJson();
            CheckUpdatesOnLevels();
            model.UnlockNewLevels();
        }
        SerializeToJson();
        return model;
    }

    private void CheckUpdatesOnLevels()
    {
        tempModel = new LevelSelectionModel(dataLoader.LoadDefaultData(levelSOs));

        if (tempModel.allLevels.Count > model.allLevels.Count)
        {
            foreach (var list1 in tempModel.allLevels)
            {
                var instanceId = list1.level.GetInstanceID();

                if (model.allLevels.Exists(x => x.level.GetInstanceID() == instanceId) == false)
                {
                    model.allLevels.Add(list1);
                }
            }
        }
        else if (model.allLevels.Count == tempModel.allLevels.Count)
        {
            for (int i = 0; i < model.allLevels.Count; i++)
            {
                model.allLevels[i].level = tempModel.allLevels[i].level;
            }
        }
        else if (model.allLevels.Count > tempModel.allLevels.Count)
        {
            model.allLevels = tempModel.allLevels;
        }
    }

    public void SerializeToJson()
    {
        string jsonString = JsonUtility.ToJson(model);
        PlayerPrefs.SetString("Levels", jsonString);
    }

    private void DeserializeFromJson()
    {
        string jsonString = PlayerPrefs.GetString("Levels");
        model = (LevelSelectionModel)JsonUtility.FromJson(jsonString, typeof(LevelSelectionModel));
    }
}
