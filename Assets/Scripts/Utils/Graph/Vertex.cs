using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex<T> where T : IEquatable<T>
{
    private T data;
    private string label;
    private ENodeColor _color;
    private int startTime;
    private int endTime;

    public event Action<ENodeColor> onColorChanged;

    public Vertex(T data)
    {
        this.label = data.GetHashCode().ToString();
        this.data = data;
    }

    public Vertex(string label, T data)
    {
        this.label = label;
        this.data = data;
    }

    public int GetStartTime()
    {
        return this.startTime;
    }

    public void SetStartTime(int time)
    {
        this.startTime = time;
    }    
    
    public int GetEndTime()
    {
        return this.endTime;
    }

    public void SetEndTime(int time)
    {
        this.endTime = time;
    }

    public T GetData()
    {
        return data;
    }

    public void SetVertexColor(ENodeColor color)
    {
        this._color = color;

        onColorChanged?.Invoke(color);
    }

    public ENodeColor GetVertexColor()
    {
        return this._color;
    }
}
