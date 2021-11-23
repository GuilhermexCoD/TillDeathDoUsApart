using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "New Prefab", menuName = "Prefab")]
public class PrefabData : ScriptableObject
{
    public string name;
    public GameObject prefab;

}
