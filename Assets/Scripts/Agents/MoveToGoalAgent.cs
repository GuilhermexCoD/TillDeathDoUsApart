using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

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

    private void OnActorAnyDamage(object sender, AnyDamageArgs e)
    {
        damageCauserTransform = e.damageCauser.transform;
    }

    public override void OnEpisodeBegin()
    {
        _move.SetVelocity(Vector2.zero);

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
            if (damageCauserTransform != null)
            {
                sensor.AddObservation((Vector2)damageCauserTransform.position);
                //sensor.AddObservation(damageCauserTransform.position.y);
            }

            if (healingCauserTransform != null)
            {
                sensor.AddObservation((Vector2)healingCauserTransform.position);
                //sensor.AddObservation(healingCauserTransform.position.y);
            }

            sensor.AddObservation(_move.GetVelocity());
        }

        //for (int i = 0; i < sensor.GetObservationShape().Length; i++)
        //{
        //    var observation = sensor.GetObservationShape()[i];
        //    Debug.Log($"{this.gameObject.name} : Observations{i} {observation}");
        //}
    }

    private void OnHealthChanged(object sender, HealthArgs e)
    {
        bool healed = (_lastHealth < e.health);

        bool isFullHealth = (_lastHealth == e.health);

        float reward = healed ? 1f : -1f * (isFullHealth ? 0.5f : 1f);

        _lastHealth = e.health;

        Debug.Log($"{this.gameObject.name} : reward({(healed ? "Heal" : (string)(isFullHealth ? "Full Health" : "Damaged"))}) = {reward}");
        
        SetReward(reward);

        if (e.health == 0)
        {
            DeadReward();
        }
    }

    private void DeadReward()
    {
        float reward = -10f;
        Debug.Log($"{this.gameObject.name} : reward(Death) = {reward}");
        SetReward(reward);
        EndEpisode();
        onEndEpisode?.Invoke(this, new EventArgs());
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.ContinuousActions);
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
    }

    private void DebugActionBuffer(ActionSegment<float> act)
    {
        for (int i = 0; i < act.Length; i++)
        {
            Debug.Log($"{this.gameObject.name}: {i} = {(float)act[i]}");
        }
    }
}
