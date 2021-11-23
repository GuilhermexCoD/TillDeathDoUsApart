using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingPlacer : MonoBehaviour
{
    public TrainingManager trainingManager;

    public bool bOnLevelGenerated = true;

    public List<GameObject> objectsToPlace;

    private HashSet<Vector3> takenPositions = new HashSet<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        trainingManager.onGenerated += OnLevelGenerated;

        if (trainingManager.IsLevelGenerated())
        {
            OnLevelGenerated(trainingManager, null);
        }
    }

    private void OnLevelGenerated(object sender, System.EventArgs e)
    {
        takenPositions.Clear();
        UpdateObjectsPositions();
    }

    private void UpdateObjectsPositions()
    {
        for (int i = 0; i < objectsToPlace.Count; i++)
        {
            SetObjectPosition(i);
        }
    }

    private Vector3 GetValidPosition()
    {
        var position = trainingManager.GetRandomPositionInsideRoom();

        while (takenPositions.Contains(position))
            position = trainingManager.GetRandomPositionInsideRoom();

        return position;
    }

    private void SetObjectPosition(int index)
    {
        var go = objectsToPlace[index];

        var position = GetValidPosition();

        takenPositions.Add(position);

        go.transform.position = position;
    }
}
