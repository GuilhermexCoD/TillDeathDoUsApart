using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraphAlgorithms
{
    void Execute<V, E>(Graph<V, E> graph, Vertex<V> source) where E : class where V : IEquatable<V>;
}
