using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Rigidbody;
    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    private Vector2 Direction;
    private float Speed;

    private float LifeSpan = 10;

    private Sprite VisualAsset;
    private Color ColorAsset;

    private void Awake()
    {
        if (Rigidbody == null)
        {
            SetRigidbody2D(GetComponent<Rigidbody2D>());
        }

        if (SpriteRenderer == null)
        {
            SetSpriteRenderer(GetComponent<SpriteRenderer>());
        }

        //SetLifeSpan(LifeSpan);
    }

    public void UpdateVisuals()
    {
        if (SpriteRenderer != null)
        {
            SpriteRenderer.sprite = VisualAsset;
            SpriteRenderer.color = ColorAsset;
        }
    }

    public void SetAsset(Sprite visual, Color color)
    {
        VisualAsset = visual;
        ColorAsset = color;

        UpdateVisuals();
    }

    public void SetVelocity(Vector2 direction, float speed)
    {
        SetStartSpeed(speed);
        SetDirection(direction);

        if (Rigidbody != null)
        {
            Rigidbody.velocity = direction.normalized * speed;
        }
    }

    public void SetSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        SpriteRenderer = spriteRenderer;
    }

    public void SetRigidbody2D(Rigidbody2D rigidbody)
    {
        Rigidbody = rigidbody;

        Rigidbody.gravityScale = 0;

        rigidbody.velocity = Direction.normalized * Speed;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    public void SetStartSpeed(float speed)
    {
        Speed = speed;
    }

    public void SetLifeSpan(float lifeSpan)
    {
        LifeSpan = lifeSpan;
        Destroy(this.gameObject, LifeSpan);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
