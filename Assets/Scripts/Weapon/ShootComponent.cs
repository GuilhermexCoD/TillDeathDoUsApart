using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    private GameObject instigator;

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

            var traceEnd = shotTransform.position + (direction.normalized * projectileData.range);

            Debug.DrawRay(shotTransform.position, direction.normalized * projectileData.range, Color.green, 10f);

            Vector3 hitPosition = (shotTransform.position + direction);

            if (hit.collider != null)
            {
                hitPosition = hit.point;

                var hitInfo = new HitResult
                {
                    traceStart = shotTransform.position,
                    traceEnd = traceEnd,
                    hitPosition = hitPosition,
                    direction = direction.normalized,
                    normal = hit.normal.normalized,
                    collider = hit.collider,
                    blockingHit = false
                };

                CallOnHit(hitInfo);

                Debug.DrawRay(hit.point, hit.normal.normalized * 2f, Color.red, 10f);

                Gameplay.ApplyPointDamage(hit.collider.GetComponent<Actor>(), GetDamage(), hitInfo, GetInstigator(), GetDamageType());
            }

            CreateShootTracer(shotTransform.position, hitPosition, eulerZ - 90);
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

    private DamageType GetDamageType()
    {
        return projectileData.damageType;
    }

    private void OnProjectileHit(object sender, HitEventArgs e)
    {
        CallOnHit(e.hitResult);
    }

    private float GetDamage()
    {
        return projectileData.damage;
    }

    private Actor GetInstigator()
    {
        return this.transform.root.GetComponent<Actor>();
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

    private void CallOnHit(HitResult hitResult)
    {
        CallOnHit(new HitEventArgs
        {
            hitResult = new HitResult
            {
                blockingHit = hitResult.blockingHit,
                traceStart = hitResult.traceStart,
                traceEnd = hitResult.traceEnd,
                hitPosition = hitResult.hitPosition,
                collider = hitResult.collider,
                normal = hitResult.normal,
                direction = hitResult.direction
            },
            damage = projectileData.damage,
            projectileIndex = projectileData.GetId(),
        });
    }

    private void CallOnHit(Vector3 start, Vector3 end, Vector3 hitPosition, Vector3 direction, Vector3 normal, Collider2D collider)
    {
        var hitResult = new HitResult
        {
            blockingHit = false,
            traceStart = start,
            traceEnd = end,
            hitPosition = hitPosition,
            collider = collider,
            normal = normal,
            direction = direction
        };

        CallOnHit(hitResult);
    }

    private void CallOnHit(HitEventArgs args)
    {
        onHit?.Invoke(this, args);
    }
}
