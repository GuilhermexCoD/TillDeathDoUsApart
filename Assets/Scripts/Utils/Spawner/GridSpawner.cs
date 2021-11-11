using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public Vector2Int size;

    public Vector2 offSet;

    public GameObject prefab;

    public void SpawnGrid()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 pos = offSet * new Vector2(i, j);
                pos += this.transform.position;

                GameObject go = Instantiate<GameObject>(prefab, pos, Quaternion.identity, this.transform);

                go.GetComponent<TrainingManager>().offsetIndex = new Vector2Int(i, j);
                go.GetComponent<TrainingManager>().startOffset = new Vector2Int((int)offSet.x, (int)offSet.y);
                go.name = $"{prefab.name}_{i}_{j}";
            }
        }
    }

    public void ClearChilds()
    {
        Debug.Log($"TrainingChilds: {this.transform.childCount}");

        while(this.transform.childCount > 0)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }
    }
}
