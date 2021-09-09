using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected WeaponData Data;

    [SerializeField]
    protected SpriteRenderer WeaponVisual;

    private void Awake()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (WeaponVisual != null)
        {
            WeaponVisual.sprite = Data.Asset;
            WeaponVisual.color = Data.Color;
        }
    }

    public virtual void Attack()
    {

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
