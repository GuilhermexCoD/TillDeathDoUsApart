using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Weapon/Projectile")]
public class ProjectileData : ItemData
{
    [Header("Bullet Properties")]
    public float startSpeed;
    public float damage;
    public float range;
    public DamageType type;

    public float lifeSpan;

    [Header("Visuals")]
    public ParticleSystem hitEffect;
    public AudioClip shotSound;

    public Sprite visualProjectile;
    public Color color = Color.white;
    public Sprite round;

    public MeshParticleData shellParticleData;

    public GameObject muzzleFlashParticle;
    public GameObject flashParticle;

    public float tracerTime;
    public Material tracerMaterial;
    public MinMaxGradient tracerGradient;

}
