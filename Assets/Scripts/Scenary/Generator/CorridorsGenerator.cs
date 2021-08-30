using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class CorridorsGenerator : GeneratorRule
{
    public CorridorsGenerator(Vector2Int start, ETheme theme, int size) : base(start, theme, size)
    {
    }

    public static HashSet<Vector2Int> ConnectCoordinates(Vector2Int a , Vector2Int b,bool horizontal)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        //bool horizontal = Random.value > 0.5f;

        Vector2Int direction = b - a;

        int lengthI = (horizontal) ? direction.x : direction.y;
        int lengthJ = (horizontal) ? direction.y : direction.x;

        for (int i = 0; i <= Mathf.Abs(lengthI); i++)
        {
            int walkAmount = (i * (int)Mathf.Sign(lengthI));
            int x = (horizontal)? a.x + walkAmount: a.x;
            int y = (horizontal)? a.y : a.y + walkAmount;
            corridors.Add(new Vector2Int(x, y));
        }

        Vector2Int lastCorridor = corridors.ToList()[corridors.Count - 1] ;
        for (int i = 0; i <= Mathf.Abs(lengthJ); i++)
        {
            int walkAmount = (i * (int)Mathf.Sign(lengthJ));
            int x = (horizontal) ? lastCorridor.x : a.x + walkAmount;
            int y = (horizontal) ? a.y + walkAmount : lastCorridor.y;
            corridors.Add(new Vector2Int(x, y));
        }

        return corridors;
    }

    public override void Generate()
    {
        base.Generate();
    }
}
