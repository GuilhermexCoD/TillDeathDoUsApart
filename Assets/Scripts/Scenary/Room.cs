using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room<TGenerator> where TGenerator : GeneratorRule
{

    public Vector2Int start;
    public ETheme theme;
    public int size;
    public Transform transform;

    public HashSet<Vector2Int> map;

    public TGenerator generator;

    public Room()
    {
        this.map = new HashSet<Vector2Int>();
    }

    public Room(Vector2Int start, ETheme theme, int size, Transform transform, TGenerator generator)
    {
        this.start = start;
        this.theme = theme;
        this.size = size;
        this.map = new HashSet<Vector2Int>();
        this.generator = generator;
        this.generator.size = size;

        if (generator != null)
        {
            generator.onCoordinateGenerated += OnGeneratedCoordinate;
        }
    }

    public Room(Vector2Int start,ETheme theme,int size, Transform transform, params object[] args)
    {
        this.start = start;
        this.theme = theme;
        this.size = size;
        this.map = new HashSet<Vector2Int>();
        this.generator = (TGenerator) Activator.CreateInstance(typeof(TGenerator),start,theme,size,transform,args);
        this.generator.size = size;

        if (generator != null)
        {
            generator.onCoordinateGenerated += OnGeneratedCoordinate;
        }
    }

    public void Generate()
    {
        if (generator != null)
        {
            generator.Generate();

            RoomGenerated(map);
        }
    }

    private void OnGeneratedCoordinate(Vector2Int coord)
    {
        map.Add(coord);
    }

    public void Subscribe(Action<HashSet<Vector2Int>> func)
    {
        onRoomGeneration += func;
    }

    public void Unsubscribe(Action<HashSet<Vector2Int>> func)
    {
        onRoomGeneration -= func;
    }

    public event Action<HashSet<Vector2Int>> onRoomGeneration;

    public void RoomGenerated(HashSet<Vector2Int> coords)
    {
        if (onRoomGeneration != null)
        {
            onRoomGeneration(coords);
        }
    }

    public Vector2Int GetCenterOfMass()
    {
        var center = new Vector2Int(0,0);

        foreach (var coord in map)
        {
            center += coord;
        }

        return center /= map.Count;
    }

    public Vector2Int GetCenterCoord()
    {
        var centerOfMass = GetCenterOfMass();

        var closestCoord = new Vector2Int();

        float dist = float.MaxValue;

        foreach (var coord in map)
        {
            float currentDist = Vector2Int.Distance(centerOfMass, coord);

            if (currentDist < dist)
            {
                dist = currentDist;
                closestCoord = coord;
            }
        }

        return closestCoord;
    }

    public Vector2Int GetRandomCoord()
    {
        int randomCoordIndex = Random.Range(0, map.Count);
        return map.ToArray()[randomCoordIndex];
    }

    private void OnDestroy()
    {
        if (generator != null)
        {
            generator.onCoordinateGenerated -= OnGeneratedCoordinate;
        }
    }
}
