using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentDamageTarget : Agent
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
    private float _attackRadius;

    [SerializeField]
    private float _listenRadius;

    [SerializeField]
    private float _attackDamage = 10f;
    [SerializeField]
    private float _attackCooldown;
    private float _lastAttackTime;
    [SerializeField]
    private LayerMask hitLayer;

    public event EventHandler<EventArgs> onEndEpisode;

    private Actor _damagedActor;
    private Vector2 _targetPosition;

    public override void Initialize()
    {
        _move = this.GetComponent<MoveAgent>();
        _move.OnInitialize(this.GetComponent<Rigidbody2D>());

        _healthSystem = this.GetComponent<HealthSystem>();

        _actor = this.GetComponent<Actor>();

        //m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }

    private void Attack()
    {
        var circleColliders = Physics2D.OverlapCircleAll(this.transform.position, _attackRadius, hitLayer);
        bool attackHit = false;
        foreach (var collider in circleColliders)
        {
            if (collider != null && collider.gameObject != this.gameObject)
            {
                if (collider.GetComponent<Actor>() != null)
                {
                    if (_damagedActor != collider.GetComponent<Actor>())
                    {
                        if (_damagedActor != null)
                        {
                            _damagedActor.GetComponent<HealthSystem>().OnHealthEqualsZero -= OnActorDamagedDead;
                        }
                        else
                        {
                            _damagedActor = collider.GetComponent<Actor>();

                            var actorHealthSystem = _damagedActor.GetComponent<HealthSystem>();

                            if (actorHealthSystem != null)
                            {
                                actorHealthSystem.OnHealthEqualsZero += OnActorDamagedDead;
                            }
                        }
                    }

                    //var actor = collider.GetComponent<Actor>();
                    //var healthSystem = actor.GetComponent<HealthSystem>();
                    //bool isGoingToDie = healthSystem.GetHealth() - _attackDamage <= 0;
                    //if (true)
                    //{

                    //}

                    //targetPosition to save on Observation
                    _targetPosition = _damagedActor.transform.localPosition;

                    //Apply Damage
                    float healthRegen = Gameplay.ApplyDamage(_damagedActor, _attackDamage, _actor, new DamageType(0, false));
                    _healthSystem.IncreaseHealth(healthRegen, _damagedActor);

                    SetReward(0.2f);

                    attackHit = true;
                }
            }

        }

        if (!attackHit)
        {
            Listen();
            SetReward(-0.1f);
        }

        _lastAttackTime = Time.time;

    }

    private void Listen()
    {
        var circleColliders = Physics2D.OverlapCircleAll(this.transform.position, _listenRadius, hitLayer);

        foreach (var collider in circleColliders)
        {
            if (collider != null)
            {
                if (collider.GetComponent<Actor>() != null)
                {
                    _targetPosition = collider.transform.localPosition;
                    return;
                }
            }
        }
    }

    private void OnActorDamagedDead(object sender, EventArgs e)
    {
        //sender.GetComponent<HealthSystem>().OnHealthEqualsZero -= OnActorDamagedDead;
        SetReward(1f);
        CallEndEpisode();
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
        _damagedActor = null;

        _targetPosition = Vector2.zero;

        _move.ResetVelocity();

        if (!_healthSystem.GetCanDestroy())
        {
            _healthSystem.OnInitialize(false);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (_bUseVectorObs)
        {
            if (_damagedActor != null)
            {
                sensor.AddObservation((Vector2)_damagedActor.transform.localPosition);
            }
            else
            {
                sensor.AddObservation(Vector2.zero);
            }


            sensor.AddObservation(_targetPosition);


            sensor.AddObservation((Vector2)this.transform.localPosition);
            sensor.AddObservation(_move.GetVelocity());
            sensor.AddObservation(_healthSystem.GetHealthNormalized());
            sensor.AddObservation(GetNormilizedTimeToNextAttack());
        }
    }

    private float GetNormilizedTimeToNextAttack()
    {
        return Mathf.Clamp01(Time.time / (_lastAttackTime + _attackCooldown));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.ContinuousActions);

        if (CanAttack(actionBuffers.ContinuousActions[2]))
        {
            Attack();
        }
    }

    private void CallEndEpisode()
    {
        EndEpisode();
        onEndEpisode?.Invoke(this, new EventArgs());
    }

    private bool CanAttack(float action)
    {
        return _lastAttackTime + _attackCooldown <= Time.time && action > 0;
    }

    public void MoveAgent(ActionSegment<float> act)
    {
        Vector2 direction = new Vector2(act[0], act[1]);

        _move.SetMoveDirection(direction);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[2] = Input.GetAxisRaw("Attack");
    }

    private void OnDrawGizmosSelected()
    {
        if (Time.time > _lastAttackTime && Time.time <= _lastAttackTime + _attackCooldown)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.transform.position, _attackRadius);
        }
    }

}
