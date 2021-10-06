using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private float health = 100;

    // Start is called before the first frame update
    void Awake()
    {
        var actor = this.GetComponent<Actor>();
        if (actor != null)
        {
            actor.OnAnyDamage += OnActorAnyDamage;
        }
    }

    private void OnActorAnyDamage(object sender, AnyDamageArgs e)
    {
        health -= e.damageType.ProccessDamage(e.baseDamage);
        Util.CreateWorldTextPopup($"Health: {health}", this.transform.position, 20, Vector3.one * 0.2f, Color.red, 2, 1);

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
