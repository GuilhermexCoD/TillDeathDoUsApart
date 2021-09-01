using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5;

    [SerializeField]
    private Vector2 MoveDirection = Vector2.zero;

    [SerializeField]
    private Rigidbody2D GoRigidbody;

    [SerializeField]
    private bool BlockMovement;

    private void Awake()
    {
        if (!GoRigidbody)
        {
            GoRigidbody = this.GetComponent<Rigidbody2D>();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector2(moveX, moveY).normalized;
    }

    public void AddForce(Vector2 force)
    {
        GoRigidbody.AddForce(force);
    }

    public void SetVelocity(Vector2 velocity)
    {
        Vector2 start = this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(start, velocity);

        if (hit.collider != null)
        {
            GoRigidbody.velocity = velocity;
        }
    }

    public Vector2 GetMoveDirection()
    {
        return MoveDirection;
    }

    void Move()
    {
        if (!BlockMovement)
        {
            GoRigidbody.velocity = (MoveDirection * Speed);
        }
    }

    public void SetBlockMovement(bool block){
        BlockMovement = block;
    }

    private void OnAnimatorMove()
    {

    }
}
