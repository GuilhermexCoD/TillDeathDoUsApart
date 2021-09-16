using System;
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

    public event EventHandler<OnShootEventArgs> onShoot;

    private void Awake()
    {
        //TODO find better way to add to the GameEventsHandler
        GameEventsHandler.current?.SubcribeToShoot(this);
    }

    public void Shoot()
    {
        print($"Shooting {projectileData.startSpeed}");
        var projectileGo = Instantiate<GameObject>(projectilePrefab, shotTransform.position, shotTransform.rotation);

        var projectile = projectileGo.GetComponent<Projectile>();

        projectile.SetVelocity(shotTransform.right, projectileData.startSpeed);
        projectile.SetLifeSpan(projectileData.lifeSpan);
        projectile.SetAsset(projectileData.visualProjectile, projectileData.color);

        Vector3 direction = shotTransform.right * projectileData.range;
        Vector3 targetPosition = (shotTransform.position + direction);

        float eulerZ = Util.GetAngleFromVectorFloat(direction);

        CreateShootTracer(shotTransform.position, targetPosition, eulerZ - 90);
        CreateMuzzleFlash(shotTransform.position, Quaternion.Euler(0, 0, eulerZ));

        CallOnShoot(new OnShootEventArgs 
        { 
            damage = projectileData.damage,
            position = shotTransform.position,
            direction = direction,
            range = projectileData.range,
            projectileIndex = projectileData.item.id
        });
    }

    private void CreateMuzzleFlash(Vector3 position, Quaternion rotation)
    {
        var goMuzzle = Instantiate(projectileData.muzzleFlashParticle, position, rotation, shotTransform);

        Destroy(goMuzzle.gameObject, 1f);
    }

    private void CreateShootTracer(Vector3 fromPosition, Vector3 targetPosition, float eulerZ)
    {
        Vector3 direction = (targetPosition - fromPosition).normalized;
        float distance = Vector3.Distance(fromPosition, targetPosition);
        Vector3 tracerStartPosition = fromPosition + direction * distance * 0.5f;
        var tmpTracerMaterial = new Material(projectileData.tracerMaterial);

        tmpTracerMaterial.color = projectileData.color;

        tmpTracerMaterial.SetTextureScale("_MainTex", new Vector2(1f, distance / projectileData.range));
        WorldMesh worldMesh = WorldMesh.Create(tracerStartPosition, eulerZ, 3f, distance, tmpTracerMaterial, null, 10000);

        float timer = 0.1f;

    }

    public void SetProjectileData(ProjectileData data)
    {
        projectileData = data;
    }

    public Vector3 GetShotWorldPosition()
    {
        return shotTransform.position;
    }

    private void CallOnShoot(OnShootEventArgs args)
    {
        onShoot?.Invoke(this, args);
    }
}
