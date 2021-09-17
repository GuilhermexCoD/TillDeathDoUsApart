using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Particle", menuName = "Particle/PartileData")]

public class ParticleData : ScriptableObject
{
    public Material material;
    public Gradient colorOverLifeTime;
}
