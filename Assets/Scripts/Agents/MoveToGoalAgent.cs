using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveToGoalAgent : Agent
{
    [SerializeField]
    private MoveAgent _move;
    [SerializeField]
    private HealthSystem _healthSystem;
    [SerializeField]
    private Actor _actor;

    private float _lastHealth;
    [SerializeField]
    private bool _bUseVectorObs;

    private float _hungry;

    [SerializeField]
    private float _attackRadius;
    [SerializeField]
    private float _attackCooldown;
    private float _lastAttackTime;
    [SerializeField]
    private LayerMask hitLayer;

    public event EventHandler<EventArgs> onEndEpisode;

    private Transform damageCauserTransform;
    private Transform healingCauserTransform;

    public override void Initialize()
    {
        _move = this.GetComponent<MoveAgent>();
        _move.OnInitialize(this.GetComponent<Rigidbody2D>());

        _healthSystem = this.GetComponent<HealthSystem>();
        _healthSystem.OnHealthChanged += OnHealthChanged;

        _actor = this.GetComponent<Actor>();
        _actor.OnAnyDamage += OnActorAnyDamage;

        //m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }

    private void Attack()
    {
        var rayHit = Physics2D.OverlapCircle(this.transform.position, _attackRadius, hitLayer);

        if (rayHit != null)
        {
            Debug.Log($"Attack hit: {rayHit.gameObject.name}");
            var damagedActor = rayHit.GetComponent<Actor>();
            float healthRegen = Gameplay.ApplyDamage(damagedActor, 10, _actor, new DamageType(0, false));

            _healthSystem.IncreaseHealth(healthRegen, damagedActor);

            float _decreaseHunger = _healthSystem.NormalizeByHealthMax(healthRegen);
            DecreaseHunger(_decreaseHunger);
        }
        else
        {
            SetReward(-0.1f);
        }

        Debug.Log($"ATTACK : {_lastAttackTime}");
        _lastAttackTime = Time.time;
    }

    private void OnActorAnyDamage(object sender, AnyDamageArgs e)
    {
        damageCauserTransform = e.damageCauser.transform;
    }

    public override void OnEpisodeBegin()
    {
        _move.SetVelocity(Vector2.zero);
        _hungry = Random.Range(0, 0.3f);
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }

    public void SetResetParameters()
    {
        _healthSystem.OnInitialize(false);
        _lastHealth = _healthSystem.GetHealth();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (_bUseVectorObs)
        {
            //if (damageCauserTransform != null)
            //{
            //    sensor.AddObservation((Vector2)damageCauserTransform.position);
            //}
            //else
            //{
            //    sensor.AddObservation(Vector2.zero);
            //}

            //if (healingCauserTransform != null)
            //{
            //    sensor.AddObservation((Vector2)healingCauserTransform.position);
            //}
            //else
            //{
            //    sensor.AddObservation(Vector2.zero);
            //}

            sensor.AddObservation(_move.GetVelocity());
            sensor.AddObservation(_healthSystem.GetHealthNormalized());
            sensor.AddObservation(_hungry);
            sensor.AddObservation(GetNormilizedTimeToNextAttack());
        }
    }

    private float GetNormilizedTimeToNextAttack()
    {
        return Mathf.Clamp01(Time.time / (_lastAttackTime + _attackCooldown));
    }

    private void OnHealthChanged(object sender, HealthArgs e)
    {
        bool healed = (_lastHealth <= e.health);

        bool isFullHealth = (_lastHealth == e.health);

        //float reward = healed ? 0.1f : -0.1f * (isFullHealth ? 0.05f : 1f);
        float reward = healed ? 0.1f : -0.1f;

        _lastHealth = e.health;

        Debug.Log($"{this.gameObject.name} : reward({(healed ? "Heal" : (string)(isFullHealth ? "Full Health" : "Damaged"))}) = {reward}");

        SetReward(reward);

        //if (healed)
        //    healingCauserTransform = e.causer.transform;
        //else
        //    damageCauserTransform = e.causer.transform;

        if (e.health == 0)
        {
            DeadReward();
        }
    }

    private void DeadReward()
    {
        float reward = -1f;
        Debug.Log($"{this.gameObject.name} : reward(Death) = {reward}");
        SetReward(reward);
        CallEndEpisode();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.ContinuousActions);

        if (CanAttack(actionBuffers.ContinuousActions[2]))
        {
            Attack();
        }

        IncreaseHunger(0.0001f);
    }

    private void IncreaseHunger(float amount)
    {
        _hungry = Mathf.Clamp01(_hungry - amount);
    }

    private void DecreaseHunger(float amount)
    {
        _hungry = Mathf.Clamp01(_hungry + amount);

        if (_hungry >= 0.98)
        {
            SetReward(1f);
            CallEndEpisode();
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

    private void DebugActionBuffer(ActionSegment<float> act)
    {
        for (int i = 0; i < act.Length; i++)
        {
            Debug.Log($"{this.gameObject.name}: {i} = {(float)act[i]}");
        }
    }
}
