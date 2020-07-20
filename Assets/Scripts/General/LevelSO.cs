using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class LevelSO : ScriptableObject
{
    public int id;
    public string levelName;
    public Sprite levelSprite;
    public GameObject levelPrefab;
}
