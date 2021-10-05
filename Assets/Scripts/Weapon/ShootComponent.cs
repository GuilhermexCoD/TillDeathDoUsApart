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
    public event EventHandler<HitEventArgs> onHit;

    private ParticleSystem muzzleParticle;
    private ParticleSystem flashParticle;

    [SerializeField]
    private bool isRaytraced;

    [SerializeField]
    private LayerMask hitLayer;

    private void Awake()
    {
        //TODO find better way to add to the GameEventsHandler
        GameEventsHandler.current?.SubcribeToShoot(this);
    }

    public void Shoot()
    {
        Vector3 direction = shotTransform.right * projectileData.range;

        float eulerZ = Util.GetAngleFromVectorFloat(direction);

        PlayParticle(projectileData.muzzleFlashParticle, ref muzzleParticle);

        PlayParticle(projectileData.flashParticle, ref flashParticle);

        CallOnShoot(new OnShootEventArgs
        {
            damage = projectileData.damage,
            shellPosition = shellEjection,
            position = shotTransform.position,
            direction = direction,
            range = projectileData.range,
            projectileIndex = projectileData.GetId()
        });

        if (isRaytraced)
        {
            var hit = Physics2D.Raycast(shotTransform.position, direction.normalized, projectileData.range, hitLayer);

            Debug.DrawRay(shotTransform.position, direction.normalized * projectileData.range, Color.green, 10f);

            Vector3 targetPosition = (shotTransform.position + direction);

            if (hit.collider != null)
            {
                targetPosition = hit.point;

                CallOnHit(shotTransform.position, targetPosition, hit.normal.normalized, hit.collider);

                Debug.DrawRay(hit.point, hit.normal.normalized * 2f, Color.red, 10f);
            }

            CreateShootTracer(shotTransform.position, targetPosition, eulerZ - 90);
        }
        else
        {
            var projectileGo = Instantiate<GameObject>(projectilePrefab, shotTransform.position, shotTransform.rotation);

            var projectile = projectileGo.GetComponent<Projectile>();

            projectile.SetVelocity(shotTransform.right, projectileData.startSpeed);
            projectile.SetLifeSpan(projectileData.lifeSpan);
            projectile.SetAsset(projectileData.visualProjectile, projectileData.color);

            projectile.onHit += OnProjectileHit;
        }
    }

    private void OnProjectileHit(object sender, HitEventArgs e)
    {
        CallOnHit(e.startPosition, e.hitPosition, e.direction, e.hitCollider);
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

            particle.Clear();
            particle.Play();
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

    private void CallOnHit(Vector3 start, Vector3 end, Vector3 direction, Collider2D collider)
    {
        CallOnHit(new HitEventArgs
        {
            startPosition = start,
            hitPosition = end,
            direction = direction,
            hitCollider = collider,
            damage = projectileData.damage,
            range = Vector3.Distance(start, end),
            projectileIndex = projectileData.GetId()
        });
    }

    private void CallOnHit(HitEventArgs args)
    {
        onHit?.Invoke(this, args);
    }
}
