using System;
using UnityEngine;
//IEquatable<AStarVertexData<T>> where T : IEquatable < T >
public class AStarVertexData<T> : IEquatable<AStarVertexData<T>>, IVertexData<AStarVertexData<T>> where T : IEquatable<T>
{
    private T _data;

    private Color _color = Color.white;
    private float _gCost = 0;
    private float _hCost = 0;
    private Vertex<AStarVertexData<T>> _parent;

    public event Action<float> onGCostChanged;
    public event Action<float> onHCostChanged;
    public event Action<Vertex<AStarVertexData<T>>> onParentChanged;
    public event Action<Color> onColorChanged;

    private float _fCost { get { return _gCost + _hCost; } }

    public AStarVertexData(T data)
    {
        SetVertexData(data);
        SetParent(null);
    }

    public void SetColor(Color color)
    {
        _color = color;
        onColorChanged?.Invoke(color);
    }

    public Color GetColor()
    {
        return _color;
    }

    public T GetVertexData()
    {
        return _data;
    }

    public void SetVertexData(T data)
    {
        _data = data;
    }

    public void SetGCost(float gCost)
    {
        _gCost = gCost;
        onGCostChanged?.Invoke(gCost);
    }

    public float GetHCost()
    {
        return _hCost;
    }

    public void SetHCost(float hCost)
    {
        _hCost = hCost;
        onHCostChanged?.Invoke(hCost);
    }

    public float GetGCost()
    {
        return _gCost;
    }

    public float GetFCost()
    {
        return _fCost;
    }

    public Vertex<AStarVertexData<T>> GetParent()
    {
        return _parent;
    }

    public void SetParent(Vertex<AStarVertexData<T>> parent)
    {
        _parent = parent;
        onParentChanged?.Invoke(parent);
    }

    public bool Equals(AStarVertexData<T> other)
    {
        return _data.Equals(other._data) && _gCost == other._gCost && _hCost == other._hCost;
    }

    AStarVertexData<T> IVertexData<AStarVertexData<T>>.GetData()
    {
        return this;
    }
}
