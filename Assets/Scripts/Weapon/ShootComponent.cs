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
    private Transform shellEjection;

    [SerializeField]
    private ProjectileData projectileData;

    public event EventHandler<OnShootEventArgs> onShoot;

    private ParticleSystem muzzleParticle;
    private ParticleSystem flashParticle;

    private void Awake()
    {
        //TODO find better way to add to the GameEventsHandler
        GameEventsHandler.current?.SubcribeToShoot(this);
    }

    public void Shoot()
    {
        var projectileGo = Instantiate<GameObject>(projectilePrefab, shotTransform.position, shotTransform.rotation);

        var projectile = projectileGo.GetComponent<Projectile>();

        projectile.SetVelocity(shotTransform.right, projectileData.startSpeed);
        projectile.SetLifeSpan(projectileData.lifeSpan);
        projectile.SetAsset(projectileData.visualProjectile, projectileData.color);

        Vector3 direction = shotTransform.right * projectileData.range;
        Vector3 targetPosition = (shotTransform.position + direction);

        float eulerZ = Util.GetAngleFromVectorFloat(direction);

        CreateShootTracer(shotTransform.position, targetPosition, eulerZ - 90);

        PlayParticle(projectileData.muzzleFlashParticle,ref muzzleParticle);

        PlayParticle(projectileData.flashParticle,ref flashParticle);

        CallOnShoot(new OnShootEventArgs
        {
            damage = projectileData.damage,
            shellPosition = shellEjection,
            position = shotTransform.position,
            direction = direction,
            range = projectileData.range,
            projectileIndex = projectileData.id
        });
    }

    private ParticleSystem CreateParticle(GameObject prefab)
    {
        float eulerZ = Util.GetAngleFromVectorFloat(shotTransform.right);
        var go = Instantiate(prefab, shotTransform.position, Quaternion.Euler(0, 0, eulerZ), shotTransform);
        return go.GetComponent<ParticleSystem>();
    }

    private void PlayParticle(GameObject prefab, ref ParticleSystem particle)
    {
        if (prefab != null)
        {
            if (particle == null)
            {
                particle = CreateParticle(prefab);
                UpdateParticleColor(ref particle);
            }
            else
            {
                particle.Stop();
                particle.Clear();
                particle.Play();
            }
        }
    }

    private void UpdateParticleColor(ref ParticleSystem particle)
    {
        var colorGradient = particle.colorOverLifetime;
        colorGradient.color = projectileData.tracerGradient;
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

        var materialCurve = worldMesh.gameObject.AddComponent<MaterialAlphaOverCurve>();
        materialCurve.SetTime(projectileData.tracerTime);
        materialCurve.SetGradientColor(projectileData.tracerGradient.gradient);
        materialCurve.SetMaterial(tmpTracerMaterial);
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
