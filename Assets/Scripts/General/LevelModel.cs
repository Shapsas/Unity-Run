using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelModel
{
    public LevelSO level;

    public bool unlocked;

    public bool completed;

    public int currentScore;

    public LevelModel(LevelSO level, bool unlock, bool complete, int score)
    {
        this.level = level;
        unlocked = unlock;
        completed = complete;
        currentScore = score;
    }
}
