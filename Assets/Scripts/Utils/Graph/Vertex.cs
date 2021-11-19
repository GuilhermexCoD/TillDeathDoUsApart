using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex<T> where T : IEquatable<T>
{
    private T data;
    private string _label;
    private ENodeColor _color;
    private int distance;
    private int startTime;
    private int endTime;

    public event Action<ENodeColor> onColorChanged;
    public event Action<int> onStartTimeChanged;
    public event Action<int> onEndTimeChanged;
    public event Action<int> onDistanceChanged;
    public event Action<T> onDataChanged;

    public Vertex(T data)
    {
        this._label = data.GetHashCode().ToString();
        this.data = data;
    }

    public Vertex(string label, T data)
    {
        this._label = label;
        this.data = data;
    }

    public int GetDistance()
    {
        return this.distance;
    }

    public void SetDistance(int value)
    {
        this.distance = value;
        onDistanceChanged?.Invoke(value);
    }

    public int GetStartTime()
    {
        return this.startTime;
    }

    public void SetStartTime(int time)
    {
        this.startTime = time;
        onStartTimeChanged?.Invoke(time);
    }    
    
    public int GetEndTime()
    {
        return this.endTime;
    }

    public void SetEndTime(int time)
    {
        this.endTime = time;
        onEndTimeChanged?.Invoke(time);
    }

    public void SetLabel(string label)
    {
        this._label = label;
    }

    public string GetLabel()
    {
        return _label;
    }

    public void SetData(T data)
    {
        this.data = data;
        onDataChanged?.Invoke(data);
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
