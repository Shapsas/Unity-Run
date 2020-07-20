using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LevelSelectionModel
{
    public List<LevelModel> allLevels = new List<LevelModel>();

    public LevelSelectionModel(List<LevelModel> levels)
    {
        allLevels = levels;
    }

    public void UnlockNewLevels()
    {
        int amount = 0;
        for (int i = 0; i < allLevels.Count; i++)
        {
            if (allLevels[i].completed == true)
            {
                amount += 1;

                if (allLevels.Exists(x => x.level.id == allLevels[i].level.id + 1) == true)
                {
                    var index2 = allLevels.FindIndex(x => x.level.id == allLevels[i].level.id + 1);
                    allLevels[index2].unlocked = true;
                }
            }
        }

    }
}
