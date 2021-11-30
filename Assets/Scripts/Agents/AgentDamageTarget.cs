using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentDamageTarget : Agent, IAgent
{
    [SerializeField]
    public MoveAgent _move;
    [SerializeField]
    private HealthSystem _healthSystem;
    [SerializeField]
    private Actor _actor;

    [SerializeField]
    private bool _bUseVectorObs;

    [SerializeField]
    private float _reachedDistance = 0.3f;

    [SerializeField]
    private bool _bTargetIsPlayer = false;
    [SerializeField]
    private GameObject _target;

    [Header("Attack")]
    [SerializeField]
    private float _attackRadius;

    [SerializeField]
    private float _attackCooldown;
    [SerializeField]
    private float _attackDamage;
    private float _lastAttackTime;
    [SerializeField]
    private LayerMask hitLayer;
    [SerializeField]
    private GameObject _attackFxPrefab;
    [SerializeField]
    private float _attackFxRadiusMultiplier;

    public event EventHandler<EventArgs> onEndEpisode;

    private Vector3 _lastPosition;
    private float _lastPositionTime;

    private PlayerControls _input;
    private bool _bAttackPressed;
    private bool _bAttackHitted;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsInHeuristicMode())
        {
            _input.Enable();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (IsInHeuristicMode())
        {
            _input.Disable();
        }
    }

    private bool IsInHeuristicMode()
    {
        return GetComponent<BehaviorParameters>().IsInHeuristicMode();
    }

    public override void Initialize()
    {
        if (IsInHeuristicMode())
        {
            _input = new PlayerControls();
            _input.Player.Attack.performed += OnAttackPerformed;
            _input.Player.Attack.canceled += OnAttackCanceled;
        }

        _move = this.GetComponent<MoveAgent>();
        _move.OnInitialize(this.GetComponent<Rigidbody2D>());

        _healthSystem = this.GetComponent<HealthSystem>();

        _actor = this.GetComponent<Actor>();

        SetResetParameters();

        if (_bTargetIsPlayer)
            _target = GameEventsHandler.current.playerGo;
    }

    private void OnAttackCanceled(InputAction.CallbackContext obj)
    {
        _bAttackPressed = false;
    }

    private void OnAttackPerformed(InputAction.CallbackContext obj)
    {
        _bAttackPressed = true;
    }
    private void SpawnAttackFX()
    {
        var fx = Instantiate<GameObject>(_attackFxPrefab, this.transform.position, Quaternion.identity);
        fx.transform.localScale = Vector3.one * _attackRadius * _attackFxRadiusMultiplier;
    }

    private void Attack()
    {
        var circleColliders = Physics2D.OverlapCircleAll(this.transform.position, _attackRadius, hitLayer);
        bool attackHit = false;
        SpawnAttackFX();
        foreach (var collider in circleColliders)
        {
            if (collider != null && collider.gameObject != this.gameObject)
            {
                if (collider.GetComponent<Actor>() != null)
                {
                    attackHit = true;

                    var damagedActor = collider.GetComponent<Actor>();
                    float healthRegen = Gameplay.ApplyDamage(damagedActor, _attackDamage, _actor, new DamageType(0, false));

                    var healthSystem = damagedActor.GetComponent<HealthSystem>();

                    if (healthSystem != null && healthSystem.GetHealthNormalized() <= 0)
                    {
                        SetReward(1f);
                        CallEndEpisode();
                    }

                    SetReward(0.5f);
                }
            }
        }

        if (!attackHit)
        {
            SetReward(-0.5f);
        }

        _bAttackHitted = attackHit;
        _lastAttackTime = Time.time;

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

        if (!_healthSystem.GetCanDestroy())
        {
            _healthSystem.OnInitialize(false);
        }

        if (_target != null && !_bTargetIsPlayer)
        {
            var targetHealthSystem = _target.GetComponent<HealthSystem>();
            targetHealthSystem.SetHealthNormalized(1, _actor);
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
            sensor.AddObservation(IsReadyToAttack());
            sensor.AddObservation(_bAttackHitted);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.ContinuousActions);

        if (CanAttack(actionBuffers.ContinuousActions[2]))
        {
            Attack();
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

    private bool IsReadyToAttack()
    {
        return _lastAttackTime + _attackCooldown <= Time.time;
    }

    private bool CanAttack(float action)
    {
        return IsReadyToAttack() && action > 0;
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

        continuousActionsOut[2] = _bAttackPressed ? 1 : 0;
    }
}
