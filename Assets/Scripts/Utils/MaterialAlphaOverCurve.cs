using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAlphaOverCurve : MonoBehaviour
{
    private Material material;

    private Gradient gradient;

    private float time;
    private float timer;

    public void SetMaterial(Material material)
    {
        this.material = material;
    }

    public void SetTime(float time)
    {
        this.time = time;
    }

    public void SetGradientColor(Gradient gradient)
    {
        this.gradient = gradient;
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (material != null)
        {
            timer += Time.deltaTime;
            material.color = gradient.Evaluate(timer / time);

            if (timer >= time)
                Destroy(this.gameObject);
        }
    }
}
