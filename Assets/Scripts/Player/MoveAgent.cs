using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAgent : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    private float maxSpeed;
    [SerializeField]
    private Vector2 moveDirection = Vector2.zero;

    [SerializeField]
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private bool blockMovement;

    private void Awake()
    {
        maxSpeed = speed;
    }

    public void OnInitialize(Rigidbody2D rigidbody2D)
    {
        this.rigidbody2D = rigidbody2D;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed()
    {
        return this.speed;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        this.moveDirection = direction.normalized;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    void Move()
    {
        if (!blockMovement)
        {
            if (moveDirection.magnitude > 0.1f)
            {
                rigidbody2D.AddForce(moveDirection * Mathf.Clamp(speed, 0, maxSpeed), ForceMode2D.Impulse);
            }
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        rigidbody2D.velocity = velocity;
        rigidbody2D.angularVelocity = 0;
    }

    public void ResetVelocity()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0;
    }

    public Vector2 GetVelocity()
    {
        return rigidbody2D.velocity;
    }

    public void SetBlockMovement(bool block)
    {
        blockMovement = block;
    }
}

