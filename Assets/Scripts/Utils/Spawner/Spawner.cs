using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject parent;
    public List<GameObject> spawnPrefabs = new List<GameObject>();
    public List<Vector2> takenPositions = new List<Vector2>();

    private void Awake()
    {
        GameEventsHandler.current.onLevelGenerated += OnLevelGenerated;
    }

    private void OnLevelClear()
    {
        Destroy(parent.gameObject);
    }

    private void OnLevelGenerated(object sender, System.EventArgs e)
    {
        parent = new GameObject("SpawnedItems");
        Level.current.onClear += OnLevelClear;
        foreach (var prefab in spawnPrefabs)
        {
            var coord = Level.current.GetRandomPositionInsideRoom();

            var position = Level.CalculatePosition(coord);

            while(takenPositions.Contains(position))
            {
                coord = Level.current.GetRandomPositionInsideRoom();
                position = Level.CalculatePosition(coord);
            }

            takenPositions.Add(position);

            Instantiate<GameObject>(prefab, position, Quaternion.identity, parent.transform);
        }
    }
}
