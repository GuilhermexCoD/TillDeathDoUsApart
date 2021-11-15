using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingSpawner : MonoBehaviour
{
    public TrainingManager trainingManager;

    public GameObject prefab;

    public int spawnAmountMin;
    public int spawnAmountMax;

    public List<GameObject> spawnedItems = new List<GameObject>();
    public List<Vector3> takenPositions = new List<Vector3>();

    public bool createMoreWhenFinished;

    // Start is called before the first frame update
    void Awake()
    {
        trainingManager.onGenerated += OnLevelGenerated;

        if (trainingManager.IsLevelGenerated())
        {
            Spawn();
        }
    }

    private void OnLevelGenerated(object sender, System.EventArgs e)
    {
        Spawn();
    }

    public void Clear()
    {
        foreach (var item in spawnedItems)
        {
            Destroy(item.gameObject);
        }

        spawnedItems.Clear();
        takenPositions.Clear();
    }

    public void Spawn()
    {
        Clear();
        int amount = Random.Range(spawnAmountMin, spawnAmountMax);
        for (int i = 0; i < amount; i++)
        {
            var item = Instantiate(prefab, trainingManager.transform);
            var position = trainingManager.GetRandomPositionInsideRoom();

            while (takenPositions.Contains(position))
            {
                position = trainingManager.GetRandomPositionInsideRoom();
            }

            item.transform.position = position;
            takenPositions.Add(position);
            spawnedItems.Add(item);

            var actor = item.GetComponent<Actor>();

            if (actor != null)
            {
                actor.OnDestroyed += OnActorDestroyed;
            }
        }
    }

    private void OnActorDestroyed(Actor obj)
    {
        if (createMoreWhenFinished)
        {
            spawnedItems.Remove(obj.gameObject);

            if (spawnedItems.Count == 0)
            {
                Spawn();
            }
        }
    }
}
