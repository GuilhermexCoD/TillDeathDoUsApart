using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected WeaponData data;

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
            weaponVisual.color = data.color;
        }
    }

    public virtual void Attack()
    {

    }

    public virtual D GetData<D>() where D : WeaponData
    {
        return (D)data;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IInteractable PickUp(object actor)
    {
        
        return this;
    }

    public void Interact(object actor)
    {
        throw new System.NotImplementedException();
    }

    public string GetInfo()
    {
        return data.ToString();
    }
}
