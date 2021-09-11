using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform shotTransform;

    [SerializeField]
    private ProjectileData projectileData;

    public void Shoot()
    {
        print($"Shooting {projectileData.startSpeed}");
        var projectileGo = Instantiate<GameObject>(projectilePrefab, shotTransform.position, shotTransform.rotation);

        var projectile = projectileGo.GetComponent<Projectile>();

        projectile.SetVelocity(shotTransform.right, projectileData.startSpeed);
        projectile.SetLifeSpan(projectileData.lifeSpan);
        projectile.SetAsset(projectileData.visualProjectile, projectileData.color);
    }

    public void SetProjectileData(ProjectileData data)
    {
        projectileData = data;
    }

    public Vector3 GetShotWorldPosition()
    {
        return shotTransform.position;
    }
}
