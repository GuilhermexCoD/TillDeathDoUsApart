using UnityEngine;

public class RangedWeapon : Weapon
{
    public Transform MuzzleTransform;

    [SerializeField]
    private GameObject projectilePrefab;

    public override void Attack()
    {
        base.Attack();

        var projectileGo = Instantiate<GameObject>(projectilePrefab, MuzzleTransform.position,transform.rotation);

        var projectile = projectileGo.GetComponent<Projectile>();

        projectile.SetVelocity(MuzzleTransform.right, Data.Projectile.StartSpeed);
        projectile.SetLifeSpan(Data.Projectile.LifeSpan);
        projectile.SetAsset(Data.Projectile.VisualProjectile,Data.Projectile.Color);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
