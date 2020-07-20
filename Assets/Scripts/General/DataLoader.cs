using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataLoader
{
    List<LevelSO> list = new List<LevelSO>();

    private void PopulateList(LevelSO[] levelSOs)
    {
        foreach (LevelSO obj in levelSOs)
        {
            list.Add(obj);
        }
    }

    public List<LevelModel> LoadDefaultData(LevelSO[] levelSOs)
    {
        List<LevelModel> levels = new List<LevelModel>();
        PopulateList(levelSOs);

        foreach (var obj in list)
        {
            if (obj.id == 0)
            {
                levels.Add(new LevelModel(obj, true, false, 0));
            }
            else
            {
                levels.Add(new LevelModel(obj, false, false, 0));
            }
        }

        var levelsSorted = levels.OrderBy(x => x.level.id).ToList();

        return levelsSorted;
    }
}
