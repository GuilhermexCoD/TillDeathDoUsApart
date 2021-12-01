using System;
using UnityEngine;
using UnityEngine.UI;

public class PathNode : MonoBehaviour
{
    public event Action onNodeDestroy;

    private float _minDistance;

    private bool _bIsArrow;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite _circle;
    [SerializeField]
    private Sprite _arrow;

    private NodeManager _nodeManager;
    private PathNode _previewsNode;
    public void OnInitialize(NodeManager nodeManager, PathNode previewsNode)
    {
        _nodeManager = nodeManager;
        _previewsNode = previewsNode;

        _nodeManager.onMove += OnMove;
        _minDistance = _nodeManager.minDistance;

        _bIsArrow = (_previewsNode != null);

        UpdateImage(_bIsArrow);

        if (_bIsArrow)
        {
            _previewsNode.onNodeDestroy += OnPreviewsNodeDestroy;
            UpdateAngle(_previewsNode.transform.position);
        }
    }

    private void UpdateImage(bool isArrow)
    {
        _image.sprite = isArrow ? _arrow : _circle;
    }

    private void UpdateAngle(Vector3 endPosition)
    {
        var direction = endPosition - this.transform.position;

        float angle = Util.GetAngleFromVectorFloat(direction);
        this.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void OnMove(Vector2 obj)
    {
        if (HasReachedNode(obj))
        {

            Destroy(this.gameObject);
        }
    }

    private bool HasReachedNode(Vector2 targetPosition)
    {
        return Vector2.Distance(this.transform.position, targetPosition) <= _minDistance;
    }

    private void OnPreviewsNodeDestroy()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        _nodeManager.onMove -= OnMove;
        onNodeDestroy?.Invoke();

        if (_previewsNode != null)
            _previewsNode.onNodeDestroy -= OnPreviewsNodeDestroy;
    }
}
