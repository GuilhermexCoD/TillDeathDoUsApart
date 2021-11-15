using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex<T> where T : IEquatable<T>
{
    private T data;
    private string label;

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

    public T GetData()
    {
        return data;
    }
}
