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
            Vector2Int coord = new Vector2Int(0, 0);
            switch (currentWalk)
            {
                case EWalkType.Random:
                    coord = GetRandomDirection();

                    currentCoord = currentCoord + coord;

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

    private Vector2Int GetRandomDirection()
    {
        EOrientation orientation = (EOrientation)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EOrientation)).Length);

        Vector2Int direction = new Vector2Int(0,0);
        switch (orientation)
        {
            case EOrientation.Up:
                direction = new Vector2Int(0, 1);
                break;
            case EOrientation.Right:
                direction = new Vector2Int(1, 0);
                break;
            case EOrientation.Down:
                direction = new Vector2Int(0, -1);
                break;
            case EOrientation.Left:
                direction = new Vector2Int(-1, 0);
                break;
            default:
                break;
        }

        return direction;
    }
}
