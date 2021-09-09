using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Weapon/Projectile")]

public class ProjectileData : ScriptableObject
{
    public ItemData Item;

    public float StartSpeed;
    public float Damage;
    public DamageType Type;

    public float LifeSpan;
    public ParticleSystem HitEffect;

    public Sprite VisualProjectile;
    public Color Color = Color.white;
    public Sprite Round;
}
