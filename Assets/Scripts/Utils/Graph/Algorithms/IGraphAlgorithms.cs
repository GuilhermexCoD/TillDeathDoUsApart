using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraphAlgorithms<V,E> where E : class where V : IEquatable<V>
{
    void Execute(Graph<V, E> graph, Vertex<V> source);
    void Execute(Graph<V, E> graph, Vertex<V> source, Vertex<V> target);
}
