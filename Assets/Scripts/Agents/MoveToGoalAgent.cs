using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent, IAgent
{
    [SerializeField]
    public MoveAgent _move;
    [SerializeField]
    private HealthSystem _healthSystem;
    [SerializeField]
    private Actor _actor;

    [SerializeField]
    private float _reachedDistance = 0.3f;

    [SerializeField]
    private bool _bUseVectorObs;

    public event EventHandler<EventArgs> onEndEpisode;

    [SerializeField]
    private bool _bTargetIsPlayer = false;
    [SerializeField]
    private GameObject _target;

    private PlayerControls _input;

    private Vector3 _lastPosition;
    private float _lastPositionTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        _input.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _input.Disable();
    }

    public override void Initialize()
    {
        _input = new PlayerControls();
        _move = this.GetComponent<MoveAgent>();
        _move.OnInitialize(this.GetComponent<Rigidbody2D>());

        _healthSystem = this.GetComponent<HealthSystem>();

        _actor = this.GetComponent<Actor>();

        SetResetParameters();

        if (_bTargetIsPlayer)
            _target = GameEventsHandler.current.playerGo;
    }

    public override void OnEpisodeBegin()
    {
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }

    public void SetPositionVelocityZero(Vector3 position)
    {
        _move.SetBlockMovement(true);
        _move.ResetVelocity();

        this.transform.position = position;
        _move.SetBlockMovement(false);
    }

    public void SetResetParameters()
    {
        _move.ResetVelocity();
        _lastPosition = this.transform.position;

        if (!_healthSystem.GetCanDestroy())
        {
            _healthSystem.OnInitialize(false);
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (_bUseVectorObs)
        {
            sensor.AddObservation(GetDirectionToTarget());
            sensor.AddObservation(HasReachedTarget());
            sensor.AddObservation(_move.GetVelocity());
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.ContinuousActions);

        if (HasReachedTarget())
        {
            SetReward(1f);
            CallEndEpisode();
        }

        if (!_bTargetIsPlayer)
            IsStuck();
    }

    public Vector2 GetDirectionToTarget()
    {
        if (_target == null)
            return Vector2.zero;

        return ((Vector2)_target.transform.position - (Vector2)this.transform.position).normalized;
    }

    public float GetDistanceToTarget()
    {
        if (_target == null)
            return float.MaxValue;

        return Vector3.Distance(this.transform.position, _target.transform.position);
    }

    private void IsStuck()
    {
        if (Vector3.Distance(this.transform.position, _lastPosition) <= _reachedDistance)
        {
            if (_lastPositionTime + 3f <= Time.time)
            {
                Debug.LogWarning("Agent Stuck Ending Episode");
                SetReward(-1f);
                CallEndEpisode();
            }
        }
        else
        {
            _lastPosition = this.transform.position;
            _lastPositionTime = Time.time;
        }
    }

    private bool HasReachedTarget()
    {
        if (_target == null)
            return false;

        return GetDistanceToTarget() <= _reachedDistance;
    }


    private void CallEndEpisode()
    {
        EndEpisode();
        onEndEpisode?.Invoke(this, new EventArgs());
    }

    public void MoveAgent(ActionSegment<float> act)
    {
        Vector2 direction = new Vector2(act[0], act[1]);

        _move.SetMoveDirection(direction);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        var moveDirection = _input.Player.Move.ReadValue<Vector2>();
        continuousActionsOut[0] = moveDirection.x;
        continuousActionsOut[1] = moveDirection.y;
    }
}
