using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private PlayerControls _input;

    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private Vector2 moveDirection = Vector2.zero;

    [SerializeField]
    private Rigidbody2D goRigidbody;

    [SerializeField]
    private bool blockMovement;

    public event EventHandler<Vector2> onSpeedChanged;

    //Chamado uma unica vez e antes do Start
    private void Awake()
    {
        _input = new PlayerControls();

        if (!goRigidbody)
        {
            goRigidbody = this.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Move();
        onSpeedChanged?.Invoke(this, goRigidbody.velocity);
    }

    void ProcessInputs()
    {

        //float moveX = Input.GetAxisRaw("Horizontal");
        //float moveY = Input.GetAxisRaw("Vertical");
        var direction = _input.Player.Move.ReadValue<Vector2>();
        moveDirection = direction.normalized;

        if (moveDirection.magnitude == 0)
        {
            goRigidbody.drag = 10;
        }
        else
        {
            goRigidbody.drag = 1;
        }
    }

    public void AddForce(Vector2 force, ForceMode2D mode)
    {
        //SetBlockMovement(true);
        goRigidbody.AddForce(force * Time.deltaTime, mode);
        //SetBlockMovement(false);
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
            //AddForce(moveDirection * speed, ForceMode2D.Impulse);
            goRigidbody.velocity = (moveDirection * speed);
        }
    }

    public void SetBlockMovement(bool block)
    {
        blockMovement = block;
    }

    private void OnEnable()
    {
        _input?.Enable();
    }

    private void OnDisable()
    {
        _input?.Disable();
    }
}
