using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : Actor
{
    [SerializeField]
    private GameObject[] lights;

    [SerializeField]
    private Color healColorLights;
    [SerializeField]
    private Color healColorAltar;

    [SerializeField]
    private Color damageColorLights;
    [SerializeField]
    private Color damageColorAltar;

    private bool b_isOn;

    [SerializeField]
    private float _rate;

    public bool isOn
    {
        get => b_isOn;
        set
        {
            b_isOn = value;
            foreach (var light in lights)
            {
                light.SetActive(b_isOn);
            }
        }
    }

    [SerializeField]
    private bool b_isHealing;

    [SerializeField]
    private float healAmount = 1;

    [SerializeField]
    private float damageAmount = 1;

    public bool B_isHealing
    {
        get => b_isHealing;
        set
        {
            b_isHealing = value;

            UpdateColor(b_isHealing);
        }
    }

    private void UpdateColor(bool isHealing)
    {
        Color altarColor = isHealing ? healColorAltar : damageColorAltar;
        Color lightsColor = isHealing ? healColorLights : damageColorLights;

        this.GetComponent<SpriteRenderer>().color = altarColor;

        foreach (var light in lights)
        {
            light.GetComponent<SpriteRenderer>().color = lightsColor;
        }
    }

    private void Awake()
    {
        isOn = false;
        B_isHealing = b_isHealing;
    }

    private float GetChangeHealthAmount()
    {
        return (b_isHealing ? healAmount : damageAmount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colision!");
        isOn = true;
        Actor actor = collision.gameObject.GetComponentInParent<Actor>();
        actor.gameObject.AddComponent<ChangeHealthOverTime>().OnInitialize(this, GetChangeHealthAmount(), _rate, b_isHealing);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isOn = false;
        var changeHealth = collision.gameObject.GetComponentInParent<ChangeHealthOverTime>();
        Destroy(changeHealth);
    }
}
