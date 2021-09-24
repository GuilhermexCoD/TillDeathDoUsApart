using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class GraphTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void IfGraphEmptyConstructorWhenAddEdgeThenConnectVertexA_B()
    {
        //Arrange
        var graph = new Graph<string, Weight>();

        string a = "1";
        string b = "2";

        int indexA = graph.AddVertex(a);
        int indexB = graph.AddVertex(b);

        //Act
        graph.AddEdge(indexA, indexB);

        //Assert
        Assert.IsTrue(graph.IsVertexLinkedTo(indexA, indexB));
        Assert.IsTrue(graph.GetAmountVertexConnections(indexA) == 1);

        Assert.IsTrue(!graph.IsVertexLinkedTo(indexB, indexA));
        Assert.IsTrue(graph.GetAmountVertexConnections(indexB) == 0);
    }

    [Test]
    public void IfGraphEmptyConstructorWhenAddEdgeThenConnectVertexA_B_AndA_B()
    {
        //Arrange
        var graph = new Graph<string, Weight>();

        string a = "1";
        string b = "2";

        int indexA = graph.AddVertex(a);
        int indexB = graph.AddVertex(b);

        //Act
        graph.AddEdge(indexA, indexB);
        graph.AddEdge(indexA, indexB);

        //Assert
        Assert.IsTrue(graph.IsVertexLinkedTo(indexA, indexB));
        Assert.IsTrue(graph.GetAmountVertexConnections(indexA) == 1);

        Assert.IsTrue(!graph.IsVertexLinkedTo(indexB, indexA));
        Assert.IsTrue(graph.GetAmountVertexConnections(indexB) == 0);

    }

    [Test]
    public void IfGraphConstructorWhenAddEdgeThenConnectVertexA_B()
    {
        //Arrange
        string a = "1";
        string b = "2";

        var vertexA = new Vertex<string>(a);
        var vertexB = new Vertex<string>(b);

        var vertices = new List<Vertex<string>>();

        vertices.Add(vertexA);
        vertices.Add(vertexB);

        var graph = new Graph<string, Weight>(vertices);

        //Act
        graph.AddEdge(vertexA, vertexB);

        //Assert
        Assert.IsTrue(graph.IsVertexLinkedTo(vertexA, vertexB));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexA) == 1);

        Assert.IsTrue(!graph.IsVertexLinkedTo(vertexB, vertexA));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexB) == 0);
    }

    [Test]
    public void IfGraphEmptyConstructorWhenRemovingVertexThenDisconnectVertexA_B()
    {
        //Arrange
        var graph = new Graph<string, Weight>();

        string a = "1";
        string b = "2";

        int indexA = graph.AddVertex(a);
        int indexB = graph.AddVertex(b);

        graph.AddEdge(indexA, indexB);

        //Act
        graph.RemoveVertex(indexA);

        //Assert
        Assert.IsTrue(!graph.IsVertexLinkedTo(indexA, indexB));
        Assert.IsTrue(graph.GetAmountVertexConnections(indexA) == 0);

        Assert.IsTrue(!graph.IsVertexLinkedTo(indexB, indexA));
        Assert.IsTrue(graph.GetAmountVertexConnections(indexB) == 0);
    }

    [Test]
    public void IfGraphEmptyConstructorWhenRemovingVertexThenDisconnectVertexA_B_AndB_A()
    {
        //Arrange
        var graph = new Graph<string, Weight>();

        string a = "1";
        string b = "2";

        int indexA = graph.AddVertex(a);
        int indexB = graph.AddVertex(b);

        graph.AddEdge(indexA, indexB);
        graph.AddEdge(indexB, indexA);

        //Act
        graph.RemoveVertex(indexA);

        //Assert
        Assert.IsTrue(!graph.IsVertexLinkedTo(indexA, indexB));
        Assert.IsTrue(graph.GetAmountVertexConnections(indexA) == 0);

        Assert.IsTrue(!graph.IsVertexLinkedTo(indexB, indexA));
        Assert.IsTrue(graph.GetAmountVertexConnections(indexB) == 0);
    }

    [Test]
    public void IfGraphConstructorWhenRemovingVertexThenDisconnectVertexA_B_AndB_A()
    {
        //Arrange
        string a = "1";
        string b = "2";

        var vertexA = new Vertex<string>(a);
        var vertexB = new Vertex<string>(b);

        var vertices = new List<Vertex<string>>();

        vertices.Add(vertexA);
        vertices.Add(vertexB);

        var graph = new Graph<string, Weight>(vertices);

        graph.AddEdge(vertexA, vertexB);
        graph.AddEdge(vertexB, vertexA);

        //Act
        graph.RemoveVertex(vertexA);

        //Assert
        Assert.IsTrue(!graph.IsVertexLinkedTo(vertexA, vertexB));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexA) == 0);

        Assert.IsTrue(!graph.IsVertexLinkedTo(vertexB, vertexA));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexB) == 0);
    }

    [Test]
    public void IfGraphConstructorWhenRemovingEdgeThenDisconnectVertexA_B_NotB_A_And_NotA_C()
    {
        //Arrange
        string a = "1";
        string b = "2";
        string c = "3";

        var vertexA = new Vertex<string>(a);
        var vertexB = new Vertex<string>(b);
        var vertexC = new Vertex<string>(c);

        var vertices = new List<Vertex<string>>();

        vertices.Add(vertexA);
        vertices.Add(vertexB);
        vertices.Add(vertexC);

        var graph = new Graph<string, Weight>(vertices);

        graph.AddEdge(vertexA, vertexB);
        graph.AddEdge(vertexA, vertexC);

        graph.AddEdge(vertexB, vertexA);

        //Act
        graph.RemoveEdge(vertexA, vertexB);

        //Assert
        Assert.IsTrue(!graph.IsVertexLinkedTo(vertexA, vertexB));
        Assert.IsTrue(graph.IsVertexLinkedTo(vertexA, vertexC));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexA) == 1);

        Assert.IsTrue(graph.IsVertexLinkedTo(vertexB, vertexA));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexB) == 1);

        Assert.IsTrue(graph.GetAmountVertexConnections(vertexC) == 0);
        Assert.IsTrue(!graph.IsVertexLinkedTo(vertexC, vertexA));
        Assert.IsTrue(!graph.IsVertexLinkedTo(vertexC, vertexB));

    }

    [Test]
    public void IfGraphConstructorWhenConnectingAllVerticesThenConnect()
    {
        //Arrange
        string a = "1";
        string b = "2";
        string c = "3";

        var vertexA = new Vertex<string>(a);
        var vertexB = new Vertex<string>(b);
        var vertexC = new Vertex<string>(c);

        var vertices = new List<Vertex<string>>();

        vertices.Add(vertexA);
        vertices.Add(vertexB);
        vertices.Add(vertexC);

        var graph = new Graph<string, Weight>(vertices);

        //Act
        graph.ConnectAllVertices();

        //Assert
        Assert.IsTrue(graph.IsVertexLinkedTo(vertexA, vertexB));
        Assert.IsTrue(graph.IsVertexLinkedTo(vertexA, vertexC));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexA) == 2);

        Assert.IsTrue(graph.IsVertexLinkedTo(vertexB, vertexC));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexB) == 1);

    }

    [Test]
    public void IfGraphConstructorWhenLoopConnectionThenConnect()
    {
        //Arrange
        string a = "1";

        var vertexA = new Vertex<string>(a);

        var vertices = new List<Vertex<string>>();

        vertices.Add(vertexA);

        var graph = new Graph<string, Weight>(vertices);

        //Act
        graph.AddEdge(vertexA, vertexA);

        //Assert
        Assert.IsTrue(graph.IsVertexLinkedTo(vertexA, vertexA));
        Assert.IsTrue(graph.GetAmountVertexConnections(vertexA) == 1);
    }

    [Test]
    public void IfCompleteGraphHowMuchEdges()
    {
        //Arrange
        int vertexCount = 3;

        //Act
        int edgeCount = Graph.GetEdgeCountForCompleteGraph(vertexCount);

        //Assert
        Assert.IsTrue(edgeCount == 3);
    }

    [Test]
    public void IfDepthFirstSearchThenVisitAllVertex()
    {
        //Arrange
        string a = "1";
        string b = "2";
        string c = "3";
        string d = "4";
        string e = "5";

        var vertexA = new Vertex<string>(a);
        var vertexB = new Vertex<string>(b);
        var vertexC = new Vertex<string>(c);
        var vertexD = new Vertex<string>(d);
        var vertexE = new Vertex<string>(e);

        var vertices = new List<Vertex<string>>();

        vertices.Add(vertexA);
        vertices.Add(vertexB);
        vertices.Add(vertexC);
        vertices.Add(vertexD);
        vertices.Add(vertexE);

        var graph = new Graph<string, Weight>(vertices);

        graph.AddEdge(vertexA, vertexB);

        graph.AddEdge(vertexB, vertexC);
        graph.AddEdge(vertexB, vertexD);

        graph.AddEdge(vertexC, vertexD);
        graph.AddEdge(vertexC, vertexE);

        //Act
        var visited = graph.DepthFirstSearch().ToList();

        //Assert
        Assert.IsTrue(visited.Count == vertices.Count);
        Assert.IsTrue(visited.TrueForAll(c => c == true));
    }
}
