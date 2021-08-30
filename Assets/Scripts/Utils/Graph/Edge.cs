using System.Collections;
using System.Collections.Generic;
using System;

public class Edge<W> where W : class
{
    private int vertexIndex;

    private W weight;

    public static bool operator ==(Edge<W> a, Edge<W> b) => (a.weight == b.weight && a.vertexIndex == b.vertexIndex);
    public static bool operator !=(Edge<W> a, Edge<W> b) => (a.weight != b.weight || a.vertexIndex != b.vertexIndex);

    public Edge(int vertexIndex)
    {
        this.vertexIndex = vertexIndex;
        this.weight = null;
    }

    public int GetVertexIndex()
    {
        return vertexIndex;
    }

    public bool IsConnectingVertex(int index)
    {
        return this.vertexIndex == index;
    }

    public Edge(int vertexIndex,W weight)
    {
        this.vertexIndex = vertexIndex;
        this.weight = weight;
    }

    public override bool Equals(object obj)
    {
        if (obj != null || !(obj is Edge<W>))
        {
            return false;
        }
        else
        {
            return this == ((Edge<W>)obj);
        }
    }

    public override int GetHashCode()
    {
        return  vertexIndex.GetHashCode() + weight.GetHashCode();
    }
}
