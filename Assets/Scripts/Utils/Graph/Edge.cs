using System.Collections;
using System.Collections.Generic;
using System;

public class Edge<W> where W : class
{
    private int StartIndex;
    private int TargetIndex;

    private W Weight;

    public static bool operator ==(Edge<W> a, Edge<W> b) => (a.Weight == b.Weight && a.TargetIndex == b.TargetIndex && a.StartIndex == b.StartIndex);
    public static bool operator !=(Edge<W> a, Edge<W> b) => (a.Weight != b.Weight || a.TargetIndex != b.TargetIndex || a.StartIndex != b.StartIndex);
    
    public Edge(int startIndex, int targetIndex, W weight) : this(startIndex, targetIndex)
    {
        this.Weight = weight;
    }

    public Edge(int startIndex, int targetIndex)
    {
        this.StartIndex = startIndex;
        this.TargetIndex = targetIndex;
    }


    public int GetVertexIndex()
    {
        return TargetIndex;
    }

    public bool IsConnectingVertex(int index)
    {
        return this.TargetIndex == index;
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

    public W GetWeight()
    {
        return Weight;
    }

    public override int GetHashCode()
    {
        return TargetIndex.GetHashCode() + Weight.GetHashCode();
    }
}
