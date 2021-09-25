using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable
{
    protected WeaponData weaponData { get { return GetData<WeaponData>(); } }

    [SerializeField]
    protected SpriteRenderer weaponVisual;

    private void Awake()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (weaponVisual != null)
        {
            weaponVisual.sprite = data.spriteAsset;
            weaponVisual.color = weaponData.color;
        }
    }

    public virtual void Attack()
    {

    }
}
