using System;
using UnityEngine;

[RequireComponent(typeof(ShootComponent))]
public class RangedWeapon : Weapon
{
    [SerializeField]
    private ShootComponent shootComponent;

    public event EventHandler<OnShootEventArgs> onShoot;

    private int currentAmmo;

    private bool isReloading;

    private float reloadTimer;

    private float _lastTimeShot;

    public override void Attack()
    {
        base.Attack();

        if (currentAmmo > 0 && !isReloading)
        {
            if (Time.time >= _lastTimeShot + GetData<RangedWeaponData>().fireRate)
            {
                shootComponent.Shoot();
                currentAmmo--;
                _lastTimeShot = Time.time;
            }
        }
        else if (!isReloading)
        {
            Util.CreateWorldTextPopup("Click!...", this.transform.position, 20, Vector3.one * 0.2f, 2, 1);
        }
    }

    public void Reload()
    {
        if (currentAmmo != GetData<RangedWeaponData>().magazineSize)
        {
            isReloading = true;
            Util.CreateWorldTextPopup("Reloading!...", this.transform.position, 20, Vector3.one * 0.2f, 2, 1);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        var data = GetData<RangedWeaponData>();

        GetShootComponent().SetProjectileData(data.projectile);

        shootComponent.onShoot += OnShootEvent;

        currentAmmo = data.magazineSize;
        reloadTimer = data.reloadTime;
    }

    private void Update()
    {
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                isReloading = false;
                var data = GetData<RangedWeaponData>();
                currentAmmo = data.magazineSize;
                reloadTimer = data.reloadTime;
                Util.CreateWorldTextPopup("Ready to shoot!", this.transform.position, 20, Vector3.one * 0.2f, 2, 1);
            }
        }
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
