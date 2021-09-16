using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MeshParticleSystem;

[CreateAssetMenu(fileName = "New MeshParticle", menuName = "MeshParticle/MeshParticle")]
public class MeshParticleData : ScriptableObject
{
    public Material material;
    public ParticleUV_Pixels[] uV_PixelsArray;
}
