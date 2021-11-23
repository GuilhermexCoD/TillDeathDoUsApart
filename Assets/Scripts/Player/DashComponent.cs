using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class DashComponent : MonoBehaviour
{
    private PlayerControls _input;

    [SerializeField]
    private Movement MovementComponent;

    [SerializeField]
    private float Cooldown = 2.0f;

    [SerializeField]
    private float Force = 10;

    [SerializeField]
    private string ActionName;

    private float LastDashTime;

    [SerializeField]
    private float DashTime = 0.1f;
    private float DashTimer;

    [SerializeField]
    private bool IsDashing;
    private bool PressedDash;

    private Vector2 Direction = Vector2.zero;

    public event EventHandler<OnDashEventArgs> onDash;

    public void Subscribe(EventHandler<OnDashEventArgs> func)
    {
        onDash += func;
    }

    public void Unsubscribe(EventHandler<OnDashEventArgs> func)
    {
        onDash -= func;
    }

    void Awake()
    {
        _input = new PlayerControls();

        if (!MovementComponent)
        {
            MovementComponent = this.GetComponent<Movement>();
        }

        LastDashTime = Time.time;
        DashTimer = DashTime;
    }

    private void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        if (IsDashing)
        {
            DashTimer -= Time.fixedDeltaTime;
            
            MovementComponent.SetBlockMovement(true);
            MovementComponent.SetVelocity(Direction * Force);

            if (DashTimer <= 0)
            {
                IsDashing = false;
                DashTimer = DashTime;
                MovementComponent.SetBlockMovement(false);
                MovementComponent.SetVelocity(Vector2.zero);
            }
        }
    }

    private void ProcessInputs()
    {

        //PressedDash = Input.GetButtonDown(ActionName);
        PressedDash = _input.Player.Dash.triggered;

        if (CanDash() && PressedDash)
        {
            IsDashing = true;
            Direction = MovementComponent.GetMoveDirection();
            LastDashTime = Time.time;

            Vector2 start = this.transform.position;
            Vector2 direction = Direction * Force * DashTime;

            Debug.DrawRay(start, direction, Color.white, 1);

            CallOnDash(direction);

            PressedDash = false;
        }
    }

    private bool CanDash()
    {
        return !IsDashing && Time.time >= LastDashTime + Cooldown ;
    }

    private void CallOnDash(Vector2 direction)
    {
        onDash?.Invoke(this,new OnDashEventArgs { direction = direction});
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
