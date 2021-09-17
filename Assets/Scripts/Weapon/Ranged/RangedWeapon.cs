using System;
using UnityEngine;

[RequireComponent(typeof(ShootComponent))]
public class RangedWeapon : Weapon
{
    [SerializeField]
    private ShootComponent shootComponent;

    public event EventHandler<OnShootEventArgs> onShoot;

    public override void Attack()
    {
        base.Attack();

        shootComponent.Shoot();
    }

    // Start is called before the first frame update
    void Awake()
    {
        var data = GetData<RangedWeaponData>();

        GetShootComponent().SetProjectileData(data.projectile);

        shootComponent.onShoot += OnShootEvent;
    }

    private void OnShootEvent(object sender, OnShootEventArgs e)
    {
        onShoot?.Invoke(sender, e);
    }

    public ShootComponent GetShootComponent()
    {
        if (shootComponent == null)
        {
            shootComponent = GetComponent<ShootComponent>();
        }

        return shootComponent;
    }

    public Vector3 GetFireWorldPosition()
    {
        return GetShootComponent().GetShotWorldPosition();
    }
}
