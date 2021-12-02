using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public event Action<Vector2> onMove;

    public float minDistance = 0.2f;

    public GameObject nodePrefab;
    public float updateRate;

    private Transform _target;
    private List<PathNode> nodes = new List<PathNode>();

    public void CreatePath(List<Vector2> path, Transform target)
    {
        _target = target;

        PathNode previews = null;
        foreach (var position in path)
        {
            var node = Instantiate<GameObject>(nodePrefab, position, Quaternion.identity, this.transform)
                        .GetComponent<PathNode>();
            node.OnInitialize(this, previews);
            nodes.Add(node);
            previews = node;
        }

        StartCoroutine(UpdateTargetPosition());
    }

    private IEnumerator UpdateTargetPosition()
    {
        while (nodes.Count > 0)
        {
            onMove?.Invoke(_target.position);

            yield return new WaitForSeconds(updateRate);
        }
    }
}
