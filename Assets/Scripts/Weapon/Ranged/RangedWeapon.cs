using UnityEngine;

[RequireComponent(typeof(ShootComponent))]
public class RangedWeapon : Weapon
{
    [SerializeField]
    private ShootComponent shootComponent;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
