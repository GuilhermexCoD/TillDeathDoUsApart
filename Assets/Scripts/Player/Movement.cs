using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private Vector2 moveDirection = Vector2.zero;

    [SerializeField]
    private Rigidbody2D goRigidbody;

    [SerializeField]
    private bool blockMovement;

    //Chamado uma unica vez e antes do Start
    private void Awake()
    {
        if (!goRigidbody)
        {
            goRigidbody = this.GetComponent<Rigidbody2D>();
        }
    }

    //Chamado uma unica vez e antes do Start
    private void Start()
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

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    public void AddForce(Vector2 force)
    {
        goRigidbody.AddForce(force);
    }

    public void SetVelocity(Vector2 velocity)
    {
        Vector2 start = this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(start, velocity);

        if (hit.collider != null)
        {
            goRigidbody.velocity = velocity;
        }
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    void Move()
    {
        if (!blockMovement)
        {
            goRigidbody.velocity = (moveDirection * speed);
        }
    }

    public void SetBlockMovement(bool block){
        blockMovement = block;
    }

    private void OnAnimatorMove()
    {

    }
}
