using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRule :  IGeneratorRule
{
    public Vector2Int start;
    public ETheme theme;
    public int size;
    protected int currentSize;

    public GeneratorRule(Vector2Int start,ETheme theme,int size)
    {
        this.start = start;
        this.theme = theme;
        this.size = size;
        currentSize = 0;
    }

    public virtual void Generate()
    {
        throw new NotImplementedException();
    }

    public event Action<Vector2Int> onCoordinateGenerated;
    public void CoordinateGenerated(Vector2Int coord)
    {
        if (currentSize < size)
        {
            if (onCoordinateGenerated != null)
            {
                onCoordinateGenerated(coord);
            }

            if (ScenaryEvents.current != null)
            {
                ScenaryEvents.current.CoordinatesCreated(coord);
            }

            currentSize++;
        }

    }
}
