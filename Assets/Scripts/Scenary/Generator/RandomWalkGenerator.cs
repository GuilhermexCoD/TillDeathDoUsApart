using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class RandomWalkGenerator : GeneratorRule
{

    public List<EWalkType> walkTypes;
    public Vector2Int currentCoord;

    public RandomWalkGenerator(Vector2Int start, ETheme theme,int size, List<EWalkType> walkTypes) : base(start, theme, size)
    {
        this.walkTypes = walkTypes;
        currentCoord = start;
    }

    public RandomWalkGenerator(Vector2Int start, ETheme theme, int size, params object[] args) : base(start, theme,size)
    {
        walkTypes = new List<EWalkType>();

        foreach (var arg in args)
        {
            walkTypes.Add((EWalkType)arg);
        }
    }
    public override void Generate()
    {
        while(currentSize < size)
        {
            int currentWalkQuantity = UnityEngine.Random.Range(1, size);
            EWalkType currentWalk = walkTypes[UnityEngine.Random.Range(0, walkTypes.Count)];
            Vector2Int coord = Vector2Int.zero;
            switch (currentWalk)
            {
                case EWalkType.Random:
                    coord = Direction2D.GetRandomDirection();

                    currentCoord += coord;

                    CoordinateGenerated(currentCoord);

                    break;
                case EWalkType.Soldier:

                    break;
                case EWalkType.Snake:

                    break;
                default:
                    break;
            }
        }
    }
}
