using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    public Vector2Int Start;
    public int maxQuantityOfRooms = 10;
    public EDifficulty difficulty = EDifficulty.Easy;

    public List<ETheme> themes = new List<ETheme>();

    public int maxQuantityOfEnemies = 20;

    public bool useBSP = true;

    public Vector2Int roomMaxSize;
    public RangeInteger quantityRangeBonusRooms;

    public Vector2Int size;
}
